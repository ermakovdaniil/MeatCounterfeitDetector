using DataAccess.Data;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using VKR.UserInterface.Admin;
using VKR.UserInterface.Admin.Abstract;
using VKR.UserInterface.Technologist;
using VKR.Utils;
using VKR.Utils.Dialog;
using VKR.Utils.MainWindowControlChanger;
using MessageBox = HandyControl.Controls.MessageBox;


namespace VKR.UserInterface;

public class LoginControlVM : ViewModelBase
{

    #region Functions

    #region Constructors

    public LoginControlVM(ResultDBContext context, NavigationManager navigationManager)
    {
        _context = context;
        _navigationManager = navigationManager;
        User = new User();
    }

    #endregion

    #endregion


    #region Properties

    public User User { get; set; }
    public User TempUser { get; set; }
    private readonly ResultDBContext _context;
    private readonly NavigationManager _navigationManager;

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
                    MessageBox.Show("Введите имя пользователя и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;
                }

                try
                {
                    var user = _context.Users.Include(u => u.Type).First(u => u.Login == User.Login && u.Password == User.Password);

                    if (user.Type.Name == "Администратор")
                    {
                        _navigationManager.Navigate<MainAdminControl>(new WindowParameters
                        {
                            WindowState = WindowState.Maximized,
                            Title = " | Панель администратора | ",
                        });
                    }

                    if (user.Type.Name == "Технолог")
                    {
                        _navigationManager.Navigate<TechnologistControl>(new WindowParameters
                        {
                            WindowState = WindowState.Maximized,
                            Title = " | Панель технолога | ",
                        },
                        data: user);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Неверное имя пользователя или пароль! Повторите попытку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }
    }

    #endregion
}
