using System.Windows.Controls;


namespace VKR.ViewModel;

public class ControlViewModel : ViewModelBase
{
    private readonly IUserControlFactory _factory;
    protected MainWindowVM _mainWindowVm;

    public ControlViewModel(IUserControlFactory factory, MainWindowVM mainWindowVm)
    {
        _factory = factory;
        _mainWindowVm = mainWindowVm;
    }

    protected void changeControl<T>(object param) where T : UserControl
    {
        var control = _factory.CreateUserControl<T>(null);
        _mainWindowVm.SetNewContent(control);
    }
}
