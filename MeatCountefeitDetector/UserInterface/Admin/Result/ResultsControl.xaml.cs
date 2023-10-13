using System.Windows.Controls;

using MeatCounterfeitDetector.Utils;


namespace MeatCounterfeitDetector.UserInterface.Admin.Result;

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

