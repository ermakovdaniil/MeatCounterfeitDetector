﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

using VKR.Utils.Dialog;
using VKR.Utils.MainWindowControlChanger;

namespace VKR.ViewModel;

public class MainWindowVM : ViewModelBase
{
    private UserControl _content;

    public MainWindowVM()
    {
        Parameters = new WindowParameters()
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

    internal void SetNewContent(UserControl content)
    {
        ContentWindow = content;
        //MenuControl.Content = content.TryFindResource("Menu") as Menu;
    }

    public Menu? Menu
    {
        get
        {
            var m = ContentWindow?.TryFindResource("Menu") as Menu;

            if (m != null)
            {
                m.DataContext = ContentWindow.DataContext;
            }
            return m;
        }
        set
        {
            throw new NotImplementedException();
        }
    }

    public WindowParameters Parameters { get; set; }
}
