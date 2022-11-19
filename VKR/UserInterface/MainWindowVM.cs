using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

using VKR.Utils.Dialog;


namespace VKR.ViewModel;

public class MainWindowVM : ViewModelBase
{
    private UserControl _content;

    public MainWindowVM()
    {
        //todo ПЕРЕПИСАТЬ СРОЧНО!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        Parameters = new WindowParameters()
        {
            Height = 300,
            Width = 300,
            StartupLocation = WindowStartupLocation.CenterScreen,
            Title = "МЯСОАНАЛИЗАТОР3000",
        };
    }

    public UserControl ContentWindow
    {
        get => _content;
        set
        {
            _content = value;
            OnPropertyChanged();
        }
    }

    internal void SetNewContent(UserControl content)
    {
        ContentWindow = content;
    }

    public WindowParameters Parameters { get; set; }
}
