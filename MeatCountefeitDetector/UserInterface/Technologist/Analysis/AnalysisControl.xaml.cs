using System.Windows.Controls;
using MeatCounterfeitDetector.UserInterface.Technologist.Analysis;
using MeatCounterfeitDetector.Utils;


namespace MeatCounterfeitDetector.UserInterface.Technologist.Analysis;

public partial class AnalysisControl : UserControl
{
    private readonly AnalysisControlVM _viewModel;

    public AnalysisControl()
    {
        InitializeComponent();
        _viewModel = (AnalysisControlVM?)VmLocator.Resolve<AnalysisControl>();
        DataContext = _viewModel;
    }
}

