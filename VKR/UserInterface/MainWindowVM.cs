using System.Diagnostics;
using System.Windows.Controls;

namespace VKR.ViewModel;

public class MainWindowVM : ViewModelBase
{
    private UserControl _content;

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
}
