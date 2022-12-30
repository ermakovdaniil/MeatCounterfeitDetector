using System.Windows;

using VKR.UserInterface.Admin.Abstract;
using VKR.Utils;
using VKR.Utils.Dialog;
using VKR.Utils.MainWindowControlChanger;

using MessageBox = HandyControl.Controls.MessageBox;


namespace VKR.UserInterface.Admin;

public class MainAdminControlVM : ViewModelBase
{
    private readonly NavigationManager _navigationManager;


#region Functions

    public MainAdminControlVM(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

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
                    Height = 350,
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
                MessageBox.Show("Данный программный комплекс предназначен для обработки\n" +
                                "входного изображения среза мясной продукции в задаче\n" +
                                "обнаружения фальсификата.\n" +
                                "\n" +
                                "Вам доступны следующие возможности:\n" +
                                "   * Администрирование базы знаний фальсификатов.\n" +
                                "   * Администрирование базы данных результатов анализа.\n" +
                                "   * Администрирование базы данных пользователейй.\n" +
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
