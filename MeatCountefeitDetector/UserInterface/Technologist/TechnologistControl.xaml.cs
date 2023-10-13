using System.Windows.Controls;

using MeatCounterfeitDetector.Utils;


namespace MeatCounterfeitDetector.UserInterface.Technologist;

public partial class TechnologistControl : UserControl
{
    private readonly TechnologistControlVM _viewModel;

    public TechnologistControl()
    {
        InitializeComponent();
        _viewModel = (TechnologistControlVM?)VmLocator.Resolve<TechnologistControl>();
        DataContext = _viewModel;
    }
}

