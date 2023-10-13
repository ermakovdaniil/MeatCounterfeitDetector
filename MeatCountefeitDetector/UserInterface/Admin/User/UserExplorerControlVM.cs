using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.Dialog;
using MeatCounterfeitDetector.Utils.MessageBoxService;


namespace MeatCounterfeitDetector.UserInterface.Admin.User;

public class UserExplorerControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public UserExplorerControlVM(ResultDBContext context, DialogService ds, IMessageBoxService messageBoxService)
    {
        _messageBoxService = messageBoxService;
        _context = context;
        //_context.UserTypes.Load();
        _ds = ds;
    }

    #endregion

    #endregion


    #region Properties

    private readonly DialogService _ds;
    private readonly ResultDBContext _context;
    private readonly IMessageBoxService _messageBoxService;
    public DataAccess.Models.User SelectedUser { get; set; }

    public List<DataAccess.Models.User> Users => _context.Users.ToList();

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
                _ds.ShowDialog<UserEditControl>(new WindowParameters
                {
                    Height = 380,
                    Width = 300,
                    Title = "Добавление пользователя",
                },
                data: new DataAccess.Models.User());

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
                _ds.ShowDialog<UserEditControl>(new WindowParameters
                {
                    Height = 380,
                    Width = 300,
                    Title = "Добавление пользователя",
                },
                data: SelectedUser);

                OnPropertyChanged(nameof(Users));
            }, _ => SelectedUser is not null);
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
                if (_messageBoxService.ShowMessage($"Вы действительно хотите удалить пользователя: \"{SelectedUser.Name}\"?", "Удаление пользователя", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _context.Users.Remove(SelectedUser);
                    _context.SaveChanges();
                }

                OnPropertyChanged(nameof(Users));
            }, _ => SelectedUser is not null);
        }
    }

    #endregion
}

