using System.Windows;

using Microsoft.Win32;

using VKR.Utils;

using MessageBox = HandyControl.Controls.MessageBox;


namespace VKR.ViewModel;

public class TechnologistControlVM : ViewModelBase
{
#region Functions

#endregion


#region Properties

    private string _displayedImagePath;

    public string DisplayedImagePath
    {
        get => _displayedImagePath;
        set
        {
            _displayedImagePath = value;
            OnPropertyChanged();
        }
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
            });
        }
    }

    private RelayCommand _changePathImage;

    public RelayCommand ChangePathImageCommand
    {
        get
        {
            return _changePathImage ??= new RelayCommand(_ =>
            {
                var open = new OpenFileDialog();
                open.DefaultExt = ".png";
                open.Filter = "Pictures (*.jpg;*.gif;*.png)|*.jpg;*.gif;*.png";

                if (open.ShowDialog() == true)
                {
                    DisplayedImagePath = open.FileName;
                }
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
                MessageBox.Show("Данный программный комплекс позволяет обработать входное изображение среза мясной продукции" +
                                "для обнаружения фальсификата, по ряду его характеристик, такие как форма, цвет, размер.\n" +
                                "В программе реализована функция формирования отчёта в файл формата PDF.\n" +
                                "Автор:  Ермаков Даниил Игоревич\n" +
                                "Группа: 494\n" +
                                "Учебное заведение: СПбГТИ (ТУ)", "Справка о программе",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }
    }

#endregion
}
