using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;

using DataAccess.Data;
using DataAccess.Models;

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
        Users = _context.Users.Local.ToObservableCollection();
        UserTypes = _context.UserTypes.Local.ToObservableCollection();
        _ds = ds;
    }

    #endregion

    #endregion


    #region Properties

    private DialogService _ds;

    private readonly UserDBContext _context;
    public User SelectedUser { get; set; }
    public ObservableCollection<User> Users { get; set; }
    public ObservableCollection<UserType> UserTypes { get; set; }

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
                _ds.ShowDialog<UserExplorerControl>(
                windowParameters: new WindowParameters
                {
                    Height = 300,
                    Width = 300,
                    Title = "Добавление пользователя"
                });
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
                _ds.ShowDialog<UserExplorerControl>(
                windowParameters: new WindowParameters
                {
                    Height = 400,
                    Width = 300,
                    Title = "Добавление цвета"
                },
                data: new User()
                {
                    Name = SelectedUser.Name,
                    Password = SelectedUser.Password,
                    TypeId = SelectedUser.TypeId,
                });
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
            }, _ => SelectedUser != null);
        }
    }

    #endregion
}
