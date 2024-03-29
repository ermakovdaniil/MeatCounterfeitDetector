﻿using System.Windows.Controls;

using MeatCounterfeitDetector.UserInterface;
using MeatCounterfeitDetector.Utils.Dialog;
using MeatCounterfeitDetector.Utils.Dialog.Abstract;
using MeatCounterfeitDetector.Utils.FrameworkFactory;


namespace MeatCounterfeitDetector.Utils.MainWindowControlChanger;

public class NavigationManager
{
    private readonly MainWindowVM _mainWindowVm;
    private readonly UserControlFactory _userControlFactory;

    public NavigationManager(MainWindowVM mainWindowVm, UserControlFactory userControlFactory)
    {
        _mainWindowVm = mainWindowVm;
        _userControlFactory = userControlFactory;
    }

    public void Navigate<UC>() where UC : UserControl
    {
        var uc = _userControlFactory.CreateUserControl<UC>(null);
        _mainWindowVm.SetNewContent(uc);
    }

    public void Navigate<UC>(WindowParameters wp) where UC : UserControl
    {
        var uc = _userControlFactory.CreateUserControl<UC>(null);
        _mainWindowVm.Parameters = wp;
        _mainWindowVm.SetNewContent(uc);
    }

    public void Navigate<UC>(WindowParameters wp, object data = null) where UC : UserControl
    {
        var uc = _userControlFactory.CreateUserControl<UC>(null);
        _mainWindowVm.Parameters = wp;
        _mainWindowVm.SetNewContent(uc);

        var viewModel = uc.DataContext;

        if (viewModel is IDataHolder dataHolder)
        {
            dataHolder.Data = data;
        }
    }
}

