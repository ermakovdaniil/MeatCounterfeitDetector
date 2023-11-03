using DataAccess.Data;
using System;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.Dialog.Abstract;
using ClientAPI;
using System.Collections.Generic;
using MeatCounterfeitDetector.UserInterface.Admin.UserType;
using System.Linq;
using Mapster;

namespace MeatCounterfeitDetector.UserInterface.Admin.User;

public class UserEditControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    #region Functions

    #region Constructors

    public UserEditControlVM(UserClient userClient,
                             UserTypeClient userTypeClient)
    {
        _userClient = userClient;
        _userTypeClient = userTypeClient;

        _userTypeClient.UserTypeGetAsync()
                       .ContinueWith(c => { UserTypeVMs = c.Result.ToList().Adapt<List<UserTypeVM>>(); });
    }

    #endregion

    #endregion


    #region Properties

    private readonly UserClient _userClient;
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
                Name = EditingUser.Name,
            };

            OnPropertyChanged(nameof(TempUser));
        }
    }

    public Action FinishInteraction { get; set; }
    public object? Result { get; set; }

    public List<UserTypeVM> UserTypeVMs { get; set; }
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
                EditingUser.Name = TempUser.Name;
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