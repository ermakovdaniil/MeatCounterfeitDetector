using System.Windows.Controls;

using VKR.UserInterface;
using VKR.Utils.Dialog;
using VKR.Utils.FrameworkFactory;


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

    public void Navigate<UC>(WindowParameters wp) where UC : UserControl
    {
        var uc = _userControlFactory.CreateUserControl<UC>(null);
        _mainWindowVm.Parameters = wp;
        _mainWindowVm.SetNewContent(uc);
    }
}

