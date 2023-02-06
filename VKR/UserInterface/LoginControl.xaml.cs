using System.Windows.Controls;

using VKR.Utils;


namespace VKR.UserInterface;

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
