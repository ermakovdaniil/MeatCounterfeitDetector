using MeatCounterfeitDetector.Utils;


namespace MeatCounterfeitDetector.UserInterface.Admin.Counterfeit;

public partial class CounterfeitEditControl
{
    private readonly CounterfeitEditControlVM _viewModel;

    public CounterfeitEditControl()
    {
        InitializeComponent();
        _viewModel = (CounterfeitEditControlVM?)VmLocator.Resolve<CounterfeitEditControl>();
        DataContext = _viewModel;
    }
}
