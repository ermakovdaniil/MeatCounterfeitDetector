using System.Windows.Controls;

using MeatCountefeitDetector.Utils;


namespace MeatCountefeitDetector.UserInterface.Admin.Result;

public partial class ResultControl : UserControl
{
    private readonly ResultControlVM _viewModel;

    public ResultControl()
    {
        InitializeComponent();
        _viewModel = (ResultControlVM?)VmLocator.Resolve<ResultControl>();
        DataContext = _viewModel;
    }
}

