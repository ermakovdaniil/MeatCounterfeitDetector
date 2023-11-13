using System;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.Dialog.Abstract;
using ClientAPI;
using System.Linq;
using Mapster;
using System.Collections.ObjectModel;
using MeatCounterfeitDetector.UserInterface.EntityVM;
using System.Collections.Generic;
using DataAccess.Models;

namespace MeatCounterfeitDetector.UserInterface.Admin.User;

public class UserEditControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    #region Functions

    #region Constructors

    public UserEditControlVM(UserRoleClient userRoleClient)
    {
        //_userRoleClient = userRoleClient;

        //_userRoleClient.UserRoleGetAsync()
        //               .ContinueWith(c =>
        //               {
        //                   UserRoleVMs = c.Result.ToList().Adapt<ObservableCollection<UserRoleVM>>();
        //                   UserRoles = new ObservableCollection<string>(UserRoleVMs.Select(userRole => userRole.Name));
        //                   //UserRoles = UserRoleVMs.Select(userRole => userRole.Name).ToList();
        //               });
    }

    #endregion

    #endregion


    #region Properties

    private readonly UserRoleClient _userRoleClient;

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
                UserName = EditingUser.UserName,
                Password = EditingUser.Password,
                Name = EditingUser.Name,
                Roles = EditingUser.Roles,
            };

            if(TempUser.Roles is not null)
            {
                if (TempUser.Roles.Contains(UserRolesConstants.Admin))
                {
                    AdminIsChosen = true;
                }

                if (TempUser.Roles.Contains(UserRolesConstants.Technologist))
                {
                    TechnologistIsChosen = true;
                }
            }

            OnPropertyChanged(nameof(TempUser));
        }
    }

    public Action FinishInteraction { get; set; }
    public object? Result { get; set; }

    //public ObservableCollection<UserRoleVM> UserRoleVMs { get; set; }
    //public ObservableCollection<string> UserRoles { get; set; }
    //public ObservableCollection<string> SelectedUserRole { get; set; }

    public bool AdminIsChosen { get; set; }
    public bool TechnologistIsChosen { get; set; }

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
                EditingUser.UserName = TempUser.UserName;
                EditingUser.Password = TempUser.Password;
                EditingUser.Name = TempUser.Name;

                TempUser.Roles = new List<string>();

                if (AdminIsChosen is true)
                {
                    TempUser.Roles.Add(UserRolesConstants.Admin);
                }
                if (TechnologistIsChosen is true)
                {
                    TempUser.Roles.Add(UserRolesConstants.Technologist);
                }

                TempUser.Roles = TempUser.Roles.Distinct().ToList();

                EditingUser.Roles = TempUser.Roles;
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