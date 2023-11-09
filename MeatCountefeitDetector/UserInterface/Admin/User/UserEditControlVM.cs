using System;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.Dialog.Abstract;
using ClientAPI;
using System.Linq;
using Mapster;
using System.Collections.ObjectModel;
using MeatCounterfeitDetector.UserInterface.EntityVM;

namespace MeatCounterfeitDetector.UserInterface.Admin.User;

public class UserEditControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    #region Functions

    #region Constructors

    public UserEditControlVM(UserTypeClient userTypeClient)
    {
        _userTypeClient = userTypeClient;

        _userTypeClient.UserTypeGetAsync()
                       .ContinueWith(c => { UserTypeVMs = c.Result.ToList().Adapt<ObservableCollection<UserTypeVM>>(); });
    }

    #endregion

    #endregion


    #region Properties

    private readonly UserTypeClient _userTypeClient;

    private object _data;
    public object Data
    {
        get => _data;
        set
        {
            _data = value;

            TempUser = new UserVM
            {
                Id = EditingUser.Id,
                Login = EditingUser.Login,
                Password = EditingUser.Password,
                Name = EditingUser.Name,
                UserType = EditingUser.UserType,
            };

            OnPropertyChanged(nameof(TempUser));
        }
    }

    public Action FinishInteraction { get; set; }
    public object? Result { get; set; }

    public ObservableCollection<UserTypeVM> UserTypeVMs { get; set; }
    public UserVM TempUser { get; set; }
    public UserVM EditingUser => (UserVM)Data;

    #endregion


    #region Commands

    private RelayCommand _saveUser;
    public RelayCommand SaveUser
    {
        get
        {
            return _saveUser ??= new RelayCommand(o =>
            {
                EditingUser.Id = TempUser.Id;
                EditingUser.Login = TempUser.Login;
                EditingUser.Password = TempUser.Password;
                EditingUser.Name = TempUser.Name;
                EditingUser.UserType = TempUser.UserType;
                Result = EditingUser;
                FinishInteraction();
            });
        }
    }

    private RelayCommand _closeCommand;
    public RelayCommand CloseCommand
    {
        get
        {
            return _closeCommand ??= new RelayCommand(o =>
            {
                FinishInteraction();
            });
        }
    }

    #endregion
}