using System;

using System.Linq;
using System.Windows;
using Autofac;

using DataAccess.Data;
using DataAccess.Models;

using Microsoft.EntityFrameworkCore;

using VKR.Utils;
using VKR.Utils.Dialog;
using VKR.Utils.MainWindowControlChanger;
using VKR.View;


namespace VKR.ViewModel;

public class LoginControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public LoginControlVM(UserDBContext context, NavigationManager navigationManager)
    {
        _context = context;
        _navigationManager = navigationManager;
        User = new User();
    }

    #endregion

    #endregion


    #region Properties

    public IContainer Container { get; set; }

    public User User { get; set; }

    private readonly UserDBContext _context;
    private readonly NavigationManager _navigationManager;

#endregion


    #region Commands

    private RelayCommand _enterCommand;

    public RelayCommand EnterCommand
    {
        get
        {
            // ReSharper disable once ConstantNullCoalescingCondition
            return _enterCommand ??= new RelayCommand(o =>
            {
                if (string.IsNullOrEmpty(User.Name) || string.IsNullOrEmpty(User.Password))
                {
                    MessageBox.Show("Введите имя пользователя и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    var user = _context.Users.Include(u=> u.Type).First(u => u.Name == User.Name && u.Password == User.Password);
                    if (user.Type.Name == "Администратор")
                    {
                        _navigationManager.Navigate<MainAdminControl>(new WindowParameters()
                        {
                            WindowState = WindowState.Maximized,
                            Title = "Панель администратора"
                        });
                    }

                    if (user.Type.Name == "Исследователь")
                    {
                        _navigationManager.Navigate<TechnologistControl>(new WindowParameters()
                        {
                            WindowState = WindowState.Maximized,
                            Title = "Панель исследователя"
                        });
                    }
                }
                catch (DivideByZeroException )
                {
                    MessageBox.Show("Неверное имя пользователя или пароль! Повторите попытку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

    }

    #endregion
}