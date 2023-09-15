using System.Windows.Controls;

using MeatCountefeitDetector.Utils;


namespace MeatCountefeitDetector.UserInterface;

public partial class LoginControl : UserControl
{
    private readonly LoginControlVM _viewModel;

    public LoginControl()
    {
        InitializeComponent();
        _viewModel = (LoginControlVM?)VmLocator.Resolve<LoginControl>();
        DataContext = _viewModel;
    }
}
