using System.Windows.Controls;

using VKR.Utils.FrameworkFactory;
using VKR.ViewModel;


namespace VKR.Utils.MainWindowControlChanger;

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
}
