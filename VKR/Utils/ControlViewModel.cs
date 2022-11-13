using System;
using System.Windows.Controls;

using Autofac;

using DataAccess.Models;

using VKR.Utils;


namespace VKR.ViewModel;

public class ControlViewModel: ViewModelBase
{
    private readonly IUserControlFactory _factory;
    protected MainWindowVM _mainWindowVm;

    public ControlViewModel(IUserControlFactory factory, MainWindowVM mainWindowVm)
    {
        _factory = factory;
        _mainWindowVm = mainWindowVm;
    }
    
    protected void changeControl<T>(object param) where T: UserControl
    {
        UserControl control = _factory.CreateUserControl<T>(null);
        _mainWindowVm.SetNewContent(control);
    }
    
}
