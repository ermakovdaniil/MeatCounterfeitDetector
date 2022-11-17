using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;

using DataAccess.Data;
using DataAccess.Models;

using VKR.Utils;
using VKR.Utils.Dialog.Abstract;

using MessageBox = HandyControl.Controls.MessageBox;

namespace VKR.ViewModel;

public class UserExplorerControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public UserExplorerControlVM(UserDBContext context)
    {
        _context = context;
        Users = _context.Users.Local.ToObservableCollection();
        UserTypes = _context.UserTypes.Local.ToObservableCollection();
    }

    #endregion

    #endregion


    #region Properties

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
                //ShowChildWindow(new UserEditWindow(new User()));
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
                //ShowChildWindow(new UserEditWindow(SelectedUser));
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
