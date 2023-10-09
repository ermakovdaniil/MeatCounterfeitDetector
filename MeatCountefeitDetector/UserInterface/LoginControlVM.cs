using DataAccess.Data;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using MeatCountefeitDetector.UserInterface.Admin;
using MeatCountefeitDetector.UserInterface.Admin.Abstract;
using MeatCountefeitDetector.UserInterface.Technologist;
using MeatCountefeitDetector.Utils;
using MeatCountefeitDetector.Utils.Dialog;
using MeatCountefeitDetector.Utils.MainWindowControlChanger;
using MeatCountefeitDetector.Utils.MessageBoxService;
using MeatCountefeitDetector.Utils.UserService;


namespace MeatCountefeitDetector.UserInterface;

public class LoginControlVM : ViewModelBase
{

    #region Functions

    #region Constructors

    public LoginControlVM(ResultDBContext context, NavigationManager navigationManager, IUserService userService, IMessageBoxService messageBoxService)
    {
        _messageBoxService = messageBoxService;
        _context = context;
        _navigationManager = navigationManager;
        _userService = userService;
        User = new User();
    }

    #endregion

    #endregion


    #region Properties

    public User User { get; set; }
    private readonly ResultDBContext _context;
    private readonly NavigationManager _navigationManager;
    private readonly IMessageBoxService _messageBoxService;
    private readonly IUserService _userService;

    #endregion


    #region Commands

    private RelayCommand _enterCommand;

    public RelayCommand EnterCommand
    {
        get
        {
            return _enterCommand ??= new RelayCommand(o =>
            {
                if (string.IsNullOrEmpty(User.Login) || string.IsNullOrEmpty(User.Password))
                {
                    _messageBoxService.ShowMessage("Введите имя пользователя и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    //var user = _context.Users.Include(u => u.Type).First(u => u.Login == User.Login && u.Password == User.Password);

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
                        _navigationManager.Navigate<TechnologistControl>(new WindowParameters
                        {
                            WindowState = WindowState.Maximized,
                            Title = " | Панель технолога | ",
                        });
                    //}
                    //_userService.User = user;
                }
                catch (Exception ex)
                {
                    _messageBoxService.ShowMessage("Неверное имя пользователя или пароль! Повторите попытку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }
    }

    #endregion
}
