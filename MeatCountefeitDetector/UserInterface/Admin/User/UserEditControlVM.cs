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
using MeatCounterfeitDetector.Utils.MessageBoxService;
using System.Windows;
using System.Text.RegularExpressions;
using MeatCountefeitDetector.Utils.Exceptions;

namespace MeatCounterfeitDetector.UserInterface.Admin.User;

public class UserEditControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    #region Functions

    #region Constructors

    public UserEditControlVM(IMessageBoxService messageBoxService)
    {
        _messageBoxService = messageBoxService;
    }

    #endregion

    #endregion


    #region Properties

    private readonly IMessageBoxService _messageBoxService;

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

            if (TempUser.Id != noId)
            {
                DontChangePassword = true;
                PasswordChangeCheckBoxIsEnabled = true;
                TempUser.Password = "";
            }
            else
            {
                PasswordChangeCheckBoxIsEnabled = false;
                DontChangePassword = false;
            }

            if (TempUser.Roles is not null)
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


    private bool _dontChangePassword { get; set; }
    public bool DontChangePassword
    {
        get { return _dontChangePassword; }
        set
        {
            if (_dontChangePassword != value)
            {
                _dontChangePassword = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PasswordIsEnabled));
            }
        }
    }
    public bool PasswordIsEnabled => !DontChangePassword;
    public bool PasswordChangeCheckBoxIsEnabled { get; set; }
    public bool AdminIsChosen { get; set; }
    public bool TechnologistIsChosen { get; set; }

    public UserVM TempUser { get; set; }
    public UserVM EditingUser => (UserVM)Data;

    private Guid noId { get; set; } = Guid.Parse("00000000-0000-0000-0000-000000000000");

    #endregion


    #region Commands

    private RelayCommand _saveUser;
    public RelayCommand SaveUser
    {
        get
        {
            return _saveUser ??= new RelayCommand(o =>
            {
                try
                {
                    if (TempUser.UserName is null || (!DontChangePassword && TempUser.Password is null))
                    {
                        throw new NullReferenceException();
                    }

                    if (!Regex.IsMatch((TempUser.UserName), @"^[a-zA-Z0-9-._@+]+$") && ((!DontChangePassword && !Regex.IsMatch((TempUser.Password), @"^[a-zA-Z0-9-._@+]+$"))))
                    {
                        throw new IncorrectSymbolsInTextException();
                    }

                    if (!DontChangePassword && TempUser.Password.Length < 8)
                    {
                        throw new ShortTextException();
                    }

                    var regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[-._@+]).+$");
                    Match match = regex.Match(TempUser.Password);

                    if (!DontChangePassword && !match.Success)
                    {
                        throw new NoRequiredSymbolsInTextException();
                    }

                    if (!AdminIsChosen && !TechnologistIsChosen)
                    {
                        throw new RoleException();
                    }

                    EditingUser.Id = TempUser.Id;
                    EditingUser.UserName = TempUser.UserName;

                    if (!DontChangePassword)
                    {
                        EditingUser.Password = TempUser.Password;
                    }

                    EditingUser.Name = TempUser.Name;

                    TempUser.Roles = new List<string>();

                    if (AdminIsChosen)
                    {
                        TempUser.Roles.Add(UserRolesConstants.Admin);
                    }
                    if (TechnologistIsChosen)
                    {
                        TempUser.Roles.Add(UserRolesConstants.Technologist);
                    }

                    TempUser.Roles = TempUser.Roles.Distinct().ToList();

                    EditingUser.Roles = TempUser.Roles;
                    Result = EditingUser;
                    FinishInteraction();
                }
                catch (NullReferenceException ex)
                {
                    string message = null;

                    if (TempUser.UserName is null)
                    {
                        message += $"Логин не может быть пустым!\n";
                    }

                    if (TempUser.Id == noId && TempUser.Password is null)
                    {
                        message += $"Пароль не может быть пустым!";
                    }

                    _messageBoxService.ShowMessage(message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (IncorrectSymbolsInTextException ex)
                {
                    string message = null;

                    if (!Regex.IsMatch((TempUser.UserName), @"^[a-zA-Z0-9-._@+]+$"))
                    {
                        message += $"Некорректные символы в логине!\n";
                    }

                    if (!Regex.IsMatch((TempUser.Password), @"^[a-zA-Z0-9-._@+]+$"))
                    {
                        message += $"Некорректные символы в пароле!\n";
                    }

                    message += $"Доступные символы:\n\"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\n0123456789-._@+\"";
                    _messageBoxService.ShowMessage(message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (ShortTextException ex)
                {
                    _messageBoxService.ShowMessage($"Пароль должен быть не меньше 8 символов!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (NoRequiredSymbolsInTextException ex)
                {
                    _messageBoxService.ShowMessage($"Пароль должен содержать строчные, прописные,\nспециальные символы и цифры!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (RoleException ex)
                {
                    _messageBoxService.ShowMessage($"Нужно выбрать хотя бы одну роль!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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