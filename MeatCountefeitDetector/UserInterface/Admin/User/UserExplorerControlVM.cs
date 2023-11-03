using DataAccess.Data;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.Dialog;
using MeatCounterfeitDetector.Utils.MessageBoxService;
using ClientAPI;
using ClientAPI.DTO.User;
using Mapster;
using ClientAPI.DTO.Counterfeit;
using MeatCounterfeitDetector.UserInterface.Admin.Counterfeit;

namespace MeatCounterfeitDetector.UserInterface.Admin.User;

public class UserExplorerControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public UserExplorerControlVM(UserClient userClient,
                                 DialogService dialogService,
                                 IMessageBoxService messageBoxService)
    {
        _userClient = userClient;
        _messageBoxService = messageBoxService;
        _dialogService = dialogService;

        _userClient.UserGetAsync()
                   .ContinueWith(c => { UserVMs = c.Result.ToList().Adapt<List<UserVM>>(); });
    }

    #endregion

    #endregion


    #region Properties

    private readonly UserClient _userClient;
    private readonly IMessageBoxService _messageBoxService;
    private readonly DialogService _dialogService;

    public List<GetUserDTO> UserDTOs { get; set; }
    public List<UserVM> UserVMs { get; set; }
    public UserVM SelectedUser { get; set; }

    #endregion


    #region Commands

    private RelayCommand _addUser;
    public RelayCommand AddUser
    {
        get
        {
            return _addUser ??= new RelayCommand(async o =>
            {
                //Application.Current.Dispatcher.Invoke(async () =>
                //{
                var result = (await _dialogService.ShowDialog<UserEditControl>(new WindowParameters
                {
                    Height = 380,
                    Width = 300,
                    Title = "Добавление пользователя",
                },
                data: new UserVM())) as UserVM;

                if (result is null)
                {
                    return;
                }

                if (!UserVMs.Any(rec => rec.Login == result.Login))
                {
                    var editingUserCreateDTO = result.Adapt<CreateUserDTO>();
                    await _userClient.UserPostAsync(editingUserCreateDTO)
                                     .ContinueWith(c => { UserVMs.Add(result); });

                    _messageBoxService.ShowMessage($"Объект успешно добавлен!", "Готово!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _messageBoxService.ShowMessage($"Такая запись уже существует!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                //});
            });
        }
    }

    private RelayCommand _editUser;
    public RelayCommand EditUser
    {
        get
        {
            return _editUser ??= new RelayCommand(async o =>
            {
                //Application.Current.Dispatcher.Invoke(async () =>
                //{
                var result = (await _dialogService.ShowDialog<UserEditControl>(new WindowParameters
                {
                    Height = 380,
                    Width = 300,
                    Title = "Добавление пользователя",
                },
                data: SelectedUser)) as UserVM;

                if (result is null)
                {
                    return;
                }
                if (!UserVMs.Any(rec => rec.Login == result.Login))
                {
                    var editingUserCreateDTO = result.Adapt<UpdateUserDTO>();
                    await _userClient.UserPutAsync(editingUserCreateDTO)
                                     .ContinueWith(c => { UserVMs.FirstOrDefault(x => x.Login == result.Login).Login = result.Login; }); ;

                    _messageBoxService.ShowMessage($"Объект успешно обновлён!", "Готово!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _messageBoxService.ShowMessage($"Такая запись уже существует!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                //});
            }, _ => SelectedUser is not null);
        }
    }

    private RelayCommand _deleteUser;
    public RelayCommand DeleteUser
    {
        get
        {
            return _deleteUser ??= new RelayCommand(async o =>
            {
                if (_messageBoxService.ShowMessage($"Вы действительно хотите удалить пользователя: \"{SelectedUser.Name}\"?", "Удаление пользователя", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    //var analysisResultDTO = AnalysisResult.Adapt<CreateResultDTO>();
                    //Application.Current.Dispatcher.Invoke(async () =>
                    //{
                    await _userClient.UserDeleteAsync(SelectedUser.Id)
                                     .ContinueWith(c => { UserVMs.Remove(SelectedUser); });
                    //});
                }
            }, _ => SelectedUser is not null);
        }
    }

    #endregion
}

