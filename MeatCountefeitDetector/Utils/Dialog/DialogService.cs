using HandyControl.Controls;
using System.Windows.Controls;
using MeatCounterfeitDetector.Utils.Dialog.Abstract;
using MeatCounterfeitDetector.Utils.FrameworkFactory;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using System;

namespace MeatCounterfeitDetector.Utils.Dialog;

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

    public Task<object?> ShowDialog<UC>(WindowParameters windowParameters, object datacontext = null, object data = null) where UC : UserControl
    {
        var tcs = new TaskCompletionSource<object?>();
        var window = new System.Windows.Window
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

        window.Closed += OnClosed;

        void OnClosed(object sender, EventArgs args)
        {
            if (viewModel is IResultHolder resultHolder)
            {
                tcs.SetResult(resultHolder.Result);
            }
            else
            {
                tcs.SetResult(null);
            }
        }

        window.Content = uc;
        window.ShowDialog();

        return tcs.Task;
    }
}

