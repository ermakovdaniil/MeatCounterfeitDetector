using System.Windows.Controls;

using HandyControl.Controls;

using VKR.Utils.Dialog.Abstract;
using VKR.Utils.FrameworkFactory;


namespace VKR.Utils.Dialog;

public class DialogService
{
    private readonly UserControlFactory _userControlFactory;

    public DialogService(UserControlFactory userControlFactory)
    {
        _userControlFactory = userControlFactory;
    }

    public object? ShowDialog<UC>(object dataContext = null, object data = null) where UC : UserControl
    {
        var wp = new WindowParameters();

        return ShowDialog<UC>(wp, dataContext, data);
    }

    public object? ShowDialog<UC>(WindowParameters windowParameters, object datacontext = null, object data = null) where UC : UserControl
    {
        var window = new Window
        {
            Height = windowParameters.Height,
            Width = windowParameters.Width,
            Title = windowParameters.Title,
            WindowStartupLocation = windowParameters.StartupLocation,
        };

        var uc = _userControlFactory.CreateUserControl<UC>(datacontext);
        var viewModel = uc.DataContext;

        if (viewModel is IDataHolder dataHolder)
        {
            dataHolder.Data = data;
        }

        if (viewModel is IInteractionAware interactionAware)
        {
            interactionAware.FinishInteraction = () => window.Close();
        }

        window.Content = uc;
        window.ShowDialog();

        if (viewModel is IResultHolder resultHolder)
        {
            return resultHolder.Result;
        }

        return null;
    }
}

