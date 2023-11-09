using System.Windows;
using MeatCounterfeitDetector.UserInterface.Admin;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.UserInterface.Technologist;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.Dialog;
using MeatCounterfeitDetector.Utils.MainWindowControlChanger;
using MeatCounterfeitDetector.Utils.MessageBoxService;
using MeatCounterfeitDetector.Utils.UserService;
using ClientAPI;

namespace MeatCounterfeitDetector.UserInterface;

public class UserInterfaceSelectControlVM : ViewModelBase
{

    #region Functions

    #region Constructors

    public UserInterfaceSelectControlVM(UserClient userClient,
                                        NavigationManager navigationManager,
                                        IUserService userService,
                                        IMessageBoxService messageBoxService)
    {
        _messageBoxService = messageBoxService;
        _userClient = userClient;
        _navigationManager = navigationManager;
        _userService = userService;
    }

    #endregion

    #endregion


    #region Fields

    private readonly UserClient _userClient;
    private readonly NavigationManager _navigationManager;
    private readonly IMessageBoxService _messageBoxService;
    private readonly IUserService _userService;

    #endregion

    #region Commands

    private RelayCommand _enterAdmin;

    public RelayCommand EnterAdmin
    {
        get
        {
            return _enterAdmin ??= new RelayCommand(_ =>
            {
                _navigationManager.Navigate<MainAdminControl>(new WindowParameters
                {
                    WindowState = WindowState.Maximized,
                    Title = " | Панель администратора | ",
                });
            });
        }
    }

    private RelayCommand _enterTechnologist;

    public RelayCommand EnterTechnologist
    {
        get
        {
            return _enterTechnologist ??= new RelayCommand(_ =>
            {
                _navigationManager.Navigate<MainTechnologistControl>(new WindowParameters
                {
                    WindowState = WindowState.Maximized,
                    Title = " | Панель технолога | ",
                });
            });
        }
    }

    #endregion
}
