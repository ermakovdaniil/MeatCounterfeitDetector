using Microsoft.Win32;
using System.Windows;
using VKR.Utils;

namespace VKR.ViewModel;

public class TechnologistControlVM : ViewModelBase
{
    #region Functions
    public TechnologistControlVM() { }

    #endregion

    #region Properties
    private string _displayedImagePath;
    public string DisplayedImagePath
    {
        get
        {
            return _displayedImagePath;
        }
        set
        {
            _displayedImagePath = value;
            OnPropertyChanged();
        }
    }
    #endregion

    #region Commands
    private RelayCommand _changePathImage;

    public RelayCommand ChangePathImageCommand
    {
        get
        {
            return _changePathImage ??= new RelayCommand(_ =>
            {
                OpenFileDialog open = new OpenFileDialog();
                open.DefaultExt = (".png");
                open.Filter = "Pictures (*.jpg;*.gif;*.png)|*.jpg;*.gif;*.png";

                if (open.ShowDialog() == true)
                    DisplayedImagePath = open.FileName;
            });
        }
    }

    private RelayCommand _scanImage;

    public RelayCommand ScanImage
    {
        get
        {
            return _scanImage ??= new RelayCommand(_ =>
            {
                // TODO: основа
            });
        }
    }

    private RelayCommand _createFile;

    public RelayCommand CreateFile
    {
        get
        {
            return _createFile ??= new RelayCommand(_ =>
            {
                // TODO: вызов FileSystem из Utils
            });
        }
    }

    private RelayCommand _changeUser;

    public RelayCommand ChangeUser
    {
        get
        {
            return _changeUser ??= new RelayCommand(_ =>
            {
                // TODO: вызов LoginControl | OnChangingRequest(new LoginControl());
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
                MessageBox.Show($"Данная программа... мысли...", "Справка о программе", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
                // TODO: exit(0);
            });
        }
    }

    #endregion
}
