using System.Windows.Controls;

using MeatCounterfeitDetector.Utils;


namespace MeatCounterfeitDetector.UserInterface.Technologist;

public partial class MainTechnologistControl : UserControl
{
    private readonly MainTechnologistControlVM _viewModel;

    public MainTechnologistControl()
    {
        InitializeComponent();
        _viewModel = (MainTechnologistControlVM?)VmLocator.Resolve<MainTechnologistControl>();
        DataContext = _viewModel;
    }
}
