using System.Windows.Controls;

using VKR.Utils;


namespace VKR.UserInterface.Admin.Result;

public partial class ResultControl : UserControl
{
    private readonly ResultControlVM _viewModel;

    public ResultControl()
    {
        InitializeComponent();
        _viewModel = (ResultControlVM?) VmLocator.Resolve<ResultControl>();
        DataContext = _viewModel;
    }
}

