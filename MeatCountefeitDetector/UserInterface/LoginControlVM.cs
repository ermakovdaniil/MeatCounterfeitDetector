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
using MeatCounterfeitDetector.Utils.AuthService;
using ClientAPI.DTO.Login;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;
using System.Net;

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
                            _navigationManager.Navigate<UserInterfaceSelectControl>(new WindowParameters
                            {
                                Height = 180,
                                Width = 300,
                                Title = "Выбор",
                                StartupLocation = WindowStartupLocation.CenterScreen,
                            });
                        }
                        else
                        {
                            _navigationManager.Navigate<MainTechnologistControl>(new WindowParameters
                            {
                                WindowState = WindowState.Maximized,
                                Title = " | Панель технолога | ",
                            });
                        }
                    }
                    catch (SecurityTokenException ex)
                    {
                        _messageBoxService.ShowMessage("Токен пользователя неверен.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (WebException ex)
                    {
                        _messageBoxService.ShowMessage("Ошибка подключения. Проверьте соединение.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (InvalidOperationException ex)
                    {
                        _messageBoxService.ShowMessage("Произошла непредвиденная ошибка.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (HttpRequestException ex)
                    {
                        _messageBoxService.ShowMessage("Нет интернет подключения. Проверьте соединение.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (DbUpdateException ex)
                    {
                        _messageBoxService.ShowMessage("Запись не найдена в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    };
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
