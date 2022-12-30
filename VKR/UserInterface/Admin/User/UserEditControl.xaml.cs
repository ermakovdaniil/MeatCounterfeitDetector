using VKR.Utils;


namespace VKR.UserInterface.Admin.User;

/// <summary>
///     Логика взаимодействия для UserEditControl.xaml
/// </summary>
public partial class UserEditControl
{
    private readonly UserEditControlVM _viewModel;

    public UserEditControl()
    {
        InitializeComponent();
        _viewModel = (UserEditControlVM?) VmLocator.Resolve<UserEditControl>();
        DataContext = _viewModel;
    }
}

