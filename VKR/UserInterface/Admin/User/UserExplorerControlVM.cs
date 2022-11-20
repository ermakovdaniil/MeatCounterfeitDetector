using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using DataAccess.Data;
using DataAccess.Models;

using Microsoft.EntityFrameworkCore;

using VKR.Utils;
using VKR.Utils.Dialog;
using VKR.View;

using MessageBox = HandyControl.Controls.MessageBox;

namespace VKR.ViewModel;

public class UserExplorerControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public UserExplorerControlVM(UserDBContext context, DialogService ds)
    {
        _context = context;
        _context.UserTypes.Load();
        _ds = ds;
    }

    #endregion

    #endregion


    #region Properties

    private DialogService _ds;

    private readonly UserDBContext _context;
    public User SelectedUser { get; set; }
    public List<User> Users
    {
        get => _context.Users.ToList();
    }

    #endregion


    #region Commands

    private RelayCommand _addUser;

    /// <summary>
    ///     Команда, открывающая окно создания пользователя
    /// </summary>
    public RelayCommand AddUser
    {
        get
        {
            return _addUser ??= new RelayCommand(o =>
            {
                _ds.ShowDialog<UserEditControl>(
                windowParameters: new WindowParameters
                {
                    Height = 300,
                    Width = 300,
                    Title = "Добавление пользователя"
                },
                data: new User()
                {

                });
                OnPropertyChanged(nameof(Users));
            });
        }
    }

    private RelayCommand _editUser;

    /// <summary>
    ///     Команда, открывающая окно редактирования пользователя
    /// </summary>
    public RelayCommand EditUser
    {
        get
        {
            return _editUser ??= new RelayCommand(o =>
            {
                _ds.ShowDialog<UserEditControl>(
                                                windowParameters: new WindowParameters
                                                {
                                                    Height = 400,
                                                    Width = 300,
                                                    Title = "Добавление пользователя"
                                                },
                                                data: SelectedUser
                                               );
                OnPropertyChanged(nameof(Users));

            }, _ => SelectedUser != null);
        }
    }

    private RelayCommand _deleteUser;

    /// <summary>
    ///     Команда, удаляющая пользователя
    /// </summary>
    public RelayCommand DeleteUser
    {
        get
        {
            return _deleteUser ??= new RelayCommand(o =>
            {
                if (MessageBox.Show($"Вы действительно хотите удалить пользователя: \"{SelectedUser.Name}\"?",
                                    "Удаление пользователя", MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                    MessageBoxResult.Yes)
                {
                    _context.Users.Remove(SelectedUser);
                    _context.SaveChanges();
                }
                OnPropertyChanged(nameof(Users));

            }, _ => SelectedUser != null);
        }
    }

    #endregion
}
