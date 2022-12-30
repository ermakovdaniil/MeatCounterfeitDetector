﻿using System;
using System.Collections.ObjectModel;
using System.Linq;

using DataAccess.Data;
using DataAccess.Models;

using VKR.UserInterface.Admin.Abstract;
using VKR.Utils;
using VKR.Utils.Dialog.Abstract;


namespace VKR.UserInterface.Admin.User;

public class UserEditControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    private object _data;


#region Functions

#region Constructors

    public UserEditControlVM(UserDBContext context)
    {
        _context = context;
        UserTypes = new ObservableCollection<UserType>(_context.UserTypes.ToList());
    }

#endregion

#endregion


    public object Data
    {
        get => _data;
        set
        {
            _data = value;

            TempUser = new DataAccess.Models.User
            {
                Id = EditingUser.Id,
                Name = EditingUser.Name,
                Password = EditingUser.Password,
                Type = EditingUser.Type,
            };

            OnPropertyChanged(nameof(TempUser));
        }
    }

    public Action FinishInteraction { get; set; }

    public object? Result { get; }


#region Properties

    private DataAccess.Models.User _tempUser;

    public DataAccess.Models.User TempUser
    {
        get => _tempUser;
        set
        {
            _tempUser = value;
            OnPropertyChanged();
        }
    }

    public DataAccess.Models.User EditingUser => (DataAccess.Models.User) Data;

    private readonly UserDBContext _context;

    public ObservableCollection<UserType> UserTypes { get; set; }

#endregion


#region Commands

    private RelayCommand _saveUser;

    /// <summary>
    ///     Команда сохраняющая изменение данных о пользователе в базе данных
    /// </summary>
    public RelayCommand SaveUser
    {
        get
        {
            return _saveUser ??= new RelayCommand(o =>
            {
                EditingUser.Id = TempUser.Id;
                EditingUser.Name = TempUser.Name;
                EditingUser.Password = TempUser.Password;
                EditingUser.Type = TempUser.Type;

                if (!_context.Users.Contains(EditingUser))
                {
                    _context.Users.Add(EditingUser);
                }

                _context.SaveChanges();
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

