using DataAccess.Data;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using MeatCounterfeitDetector.UserInterface.Admin;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.UserInterface.Technologist;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.Dialog;
using MeatCounterfeitDetector.Utils.MainWindowControlChanger;
using MeatCounterfeitDetector.Utils.MessageBoxService;
using MeatCounterfeitDetector.Utils.UserService;
using ClientAPI;
using MeatCountefeitDetector.Utils.AuthService;
using ClientAPI.DTO.Login;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace MeatCounterfeitDetector.UserInterface;

public class LoginControlVM : ViewModelBase
{

    #region Functions

    #region Constructors

    public LoginControlVM(UserClient userClient,
                          NavigationManager navigationManager,
                          IUserService userService,
                          IMessageBoxService messageBoxService,
                          IAuthService authService)
    {
        _messageBoxService = messageBoxService;
        _userClient = userClient;
        _navigationManager = navigationManager;
        _userService = userService;
        _authService = authService;
    }

    #endregion

    #endregion


    #region Fields

    private readonly UserClient _userClient;
    private readonly NavigationManager _navigationManager;
    private readonly IMessageBoxService _messageBoxService;
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    #endregion

    public string Username { get; set; } = "admin";
    public string Password { get; set; } = "SuperMegaSecretPassword123!!!";

    #region Commands

    private RelayCommand _enterCommand;

    public RelayCommand EnterCommand
    {
        get
        {
            return _enterCommand ??= new RelayCommand(async o =>
            {
                try
                {
                    try
                    {
                        await _authService.LoginAsync(new LoginModel { Password = Password, Username = Username });

                        var token = _authService.GetToken();
                        _userService.SetUserByToken(token);

                        if (_userService.IsAdmin)
                        {
                            _navigationManager.Navigate<MainAdminControl>(new WindowParameters
                            {
                                WindowState = WindowState.Maximized,
                                Title = " | Панель администратора | ",
                            });
                        }
                        else
                        {
                            _navigationManager.Navigate<TechnologistControl>(new WindowParameters
                            {
                                WindowState = WindowState.Maximized,
                                Title = " | Панель технолога | ",
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        //todo как-то обработать, месаг бох например, на разные эксепшены разные кэтчи и разные месаг бохи
                        Debug.WriteLine(e);
                    };


                    //var userType = _userClient.GetType(User.TypeId);

                    // getUserIdByLoginAndPassword
                    // getUserTypeById public async Task<ActionResult<GetUserTypeDTO>> Get(Guid id)

                    //if (user.Type.Name == "Администратор")
                    //{
                    //    _navigationManager.Navigate<MainAdminControl>(new WindowParameters
                    //    {
                    //        WindowState = WindowState.Maximized,
                    //        Title = " | Панель администратора | ",
                    //    });
                    //}

                    //if (user.Type.Name == "Технолог")
                    //{
                    //_navigationManager.Navigate<TechnologistControl>(new WindowParameters
                    //{
                    //    WindowState = WindowState.Maximized,
                    //    Title = " | Панель технолога | ",
                    //});
                    //}
                    //_userService.User = user;
                }
                catch (Exception ex)
                {
                    _messageBoxService.ShowMessage("Неверное имя пользователя или пароль! Повторите попытку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }, _ => !(string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password)));
        }
    }

    #endregion
}
