using System;
using System.Windows;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.UserInterface.Technologist;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.Dialog;
using MeatCounterfeitDetector.Utils.MainWindowControlChanger;
using MeatCounterfeitDetector.Utils.MessageBoxService;
using MeatCounterfeitDetector.Utils.UserService;
using ClientAPI;
using MeatCounterfeitDetector.Utils.AuthService;
using ClientAPI.DTO.Login;
using System.Net.Http;

namespace MeatCounterfeitDetector.UserInterface;

public class LoginControlVM : ViewModelBase
{

    #region Functions

    #region Constructors

    public LoginControlVM(UserClient userClient,
                          NavigationManager navigationManager,
                          IUserService userService,
                          IMessageBoxService messageBoxService,
                          IAuthService authService)
    {
        _messageBoxService = messageBoxService;
        _userClient = userClient;
        _navigationManager = navigationManager;
        _userService = userService;
        _authService = authService;
    }

    #endregion

    #endregion


    #region Fields

    private readonly UserClient _userClient;
    private readonly NavigationManager _navigationManager;
    private readonly IMessageBoxService _messageBoxService;
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    #endregion

    public string Username { get; set; } = "admin";
    public string Password { get; set; } = "SuperMegaSecretPassword123!!!";

    #region Commands

    private RelayCommand _enterCommand;

    public RelayCommand EnterCommand
    {
        get
        {
            return _enterCommand ??= new RelayCommand(async o =>
            {
                try
                {
                    try
                    {
                        await _authService.LoginAsync(new LoginModel { Password = Password, Username = Username });

                        var token = _authService.GetToken();
                        _userService.SetUserByToken(token);

                        if (_userService.IsAdmin)
                        {
                            _navigationManager.Navigate<UserInterfaceSelectControl>(new WindowParameters
                            {
                                Height = 260,
                                Width = 430,
                                Title = "Выбор",
                                StartupLocation = WindowStartupLocation.CenterScreen,
                            });
                        }
                        else
                        {
                            _navigationManager.Navigate<MainTechnologistControl>(new WindowParameters
                            {
                                WindowState = WindowState.Maximized,
                                Title = " | Панель технолога | ",
                            });
                        }
                    }
                    catch (ApiException ex)
                    {
                        switch (ex.StatusCode)
                        {
                            case 400:
                                _messageBoxService.ShowMessage("Неверный запрос!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;

                            case 401:
                                _messageBoxService.ShowMessage("Нужна аутентификация!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;

                            case 403:
                                _messageBoxService.ShowMessage("Нет прав доступа к содержимому!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;

                            case 404:
                                _messageBoxService.ShowMessage("Сервер не может найти запрашиваемый ресурс.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;

                            case 500:
                                _messageBoxService.ShowMessage("Произошла внутренняя ошибка.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;

                            case 502:
                                _messageBoxService.ShowMessage("Получен недопустимый ответ от сервера.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;

                            case 503:
                                _messageBoxService.ShowMessage("Сервер не может обработать запрос в данный момент.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;

                            case 504:
                                _messageBoxService.ShowMessage("Время ожидания истекло.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;

                            default:
                                _messageBoxService.ShowMessage("Произошла непредвиденная ошибка.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;
                        }           
                    }
                    catch (HttpRequestException ex)
                    {
                        _messageBoxService.ShowMessage("Нет интернет подключения. Проверьте соединение.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    };
                }
                catch (Exception ex)
                {
                    _messageBoxService.ShowMessage("Неверное имя пользователя или пароль! Повторите попытку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }, _ => !(string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password)));
        }
    }

    #endregion
}
