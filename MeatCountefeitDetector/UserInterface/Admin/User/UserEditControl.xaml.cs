using MeatCounterfeitDetector.Utils;


namespace MeatCounterfeitDetector.UserInterface.Admin.User;

public partial class UserEditControl
{
    private readonly UserEditControlVM _viewModel;

    public UserEditControl()
    {
        InitializeComponent();
        _viewModel = (UserEditControlVM?)VmLocator.Resolve<UserEditControl>();
        DataContext = _viewModel;
    }
}

