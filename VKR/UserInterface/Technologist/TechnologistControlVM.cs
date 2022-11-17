using Microsoft.Win32;

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
    #endregion
}
