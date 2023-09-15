using System;
using System.Windows;
using System.Windows.Controls;

using MeatCountefeitDetector.UserInterface.Admin.Abstract;
using MeatCountefeitDetector.Utils.Dialog;


namespace MeatCountefeitDetector.UserInterface;

public class MainWindowVM : ViewModelBase
{
    private UserControl _content;

    public MainWindowVM()
    {
        Parameters = new WindowParameters
        {
            Height = 300,
            Width = 300,
            StartupLocation = WindowStartupLocation.CenterScreen,
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

    public Menu? Menu
    {
        get
        {
            var m = ContentWindow?.TryFindResource("Menu") as Menu;

            if (m is not null)
            {
                m.DataContext = ContentWindow.DataContext;
            }

            return m;
        }
        set => throw new NotImplementedException();
    }

    public WindowParameters Parameters { get; set; }

    internal void SetNewContent(UserControl content)
    {
        ContentWindow = content;

        //MenuControl.Content = content.TryFindResource("Menu") as Menu;
    }
}

