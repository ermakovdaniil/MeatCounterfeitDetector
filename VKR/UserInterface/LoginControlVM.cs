using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using Autofac;

using DataAccess.Data;
using DataAccess.Models;

using VKR.Utils;
using VKR.Utils.MainWindowControlChanger;
using VKR.View;


namespace VKR.ViewModel;

public class LoginControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public LoginControlVM(UserDBContext context)
    {
        _context = context;
        User = new User();
    }

    #endregion

    #endregion


    #region Properties

    public IContainer Container { get; set; }

    public User User { get; set; }

    private readonly UserDBContext _context;

    #endregion


    #region Commands

    private RelayCommand _enterCommand;

    public RelayCommand EnterCommand
    {
        get
        {
            return _enterCommand ??= new RelayCommand(o =>
            {
                if (string.IsNullOrEmpty(User.Name) || string.IsNullOrEmpty(User.Password))
                {
                    MessageBox.Show("Введите имя пользователя и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    var user = _context.Users.Include(u => u.Type).First(u => u.Name == User.Name && u.Password == User.Password);
                    if (user.Type.Type == "Администратор")
                    {
                        var navigator = Container.Resolve<NavigationManager>();
                        navigator.Navigate<MainAdminControl>();
                    }

                    if (user.Type.Type == "Исследователь")
                    {
                        var navigator = Container.Resolve<NavigationManager>();
                        navigator.Navigate<TechnologistControl>();
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Неверное имя пользователя или пароль! Повторите попытку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

    }

    #endregion
}