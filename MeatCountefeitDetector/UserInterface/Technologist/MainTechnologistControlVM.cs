using System.Windows;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.Dialog;
using MeatCounterfeitDetector.Utils.MainWindowControlChanger;
using MeatCounterfeitDetector.Utils.MessageBoxService;

namespace MeatCounterfeitDetector.UserInterface.Technologist;

public class MainTechnologistControlVM : ViewModelBase
{
    private readonly IMessageBoxService _messageBoxService;
    private readonly NavigationManager _navigationManager;


    #region Functions

    public MainTechnologistControlVM(NavigationManager navigationManager,
                                     IMessageBoxService messageBoxService)
    {
        _navigationManager = navigationManager;
        _messageBoxService = messageBoxService;
    }

    #endregion


    #region Properties

    #endregion


    #region Commands

    private RelayCommand _changeUser;

    public RelayCommand ChangeUser
    {
        get
        {
            return _changeUser ??= new RelayCommand(_ =>
            {
                _navigationManager.Navigate<LoginControl>(new WindowParameters
                {
                    Height = 300,
                    Width = 350,
                    Title = "Вход в систему",
                    StartupLocation = WindowStartupLocation.CenterScreen,
                });
            });
        }
    }

    private RelayCommand _showInfo;

    public RelayCommand ShowInfo
    {
        get
        {
            return _showInfo ??= new RelayCommand(_ =>
            {
                _messageBoxService.ShowMessage("Данный программный комплекс предназначен для обработки\n" +
                                               "входного изображения среза мясной продукции в задаче\n" +
                                               "обнаружения фальсификата.\n" +
                                               "\n" +
                                               "Вам доступны следующие возможности:\n" +
                                               "   * Анализ изображения.\n" +
                                               "   * Сохранение результата анализа в виде отчёта.\n" +
                                               "\n" +
                                               "Автор:  Ермаков Даниил Игоревич\n" +
                                               "Группа: 494\n" +
                                               "Учебное заведение: СПбГТИ (ТУ)", "Справка о программе",
                                               MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }
    }

    private RelayCommand _exit;

    public RelayCommand Exit
    {
        get
        {
            return _exit ??= new RelayCommand(_ =>
            {
                Application.Current.Shutdown();
            });
        }
    }

    #endregion
}
