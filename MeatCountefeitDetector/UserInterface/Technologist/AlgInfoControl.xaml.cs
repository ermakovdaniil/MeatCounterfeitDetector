using System.Windows.Controls;

using MeatCounterfeitDetector.Utils;


namespace MeatCounterfeitDetector.UserInterface.Technologist;

public partial class AlgInfoControl : UserControl
{
    private readonly AlgInfoControlVM _viewModel;

    public AlgInfoControl()
    {
        InitializeComponent();
        _viewModel = (AlgInfoControlVM?)VmLocator.Resolve<AlgInfoControl>();
        DataContext = _viewModel;
    }
}
