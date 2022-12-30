using System.Windows.Controls;

using VKR.Utils;


namespace VKR.UserInterface;

/// <summary>
///     Логика взаимодействия для ColorPropertyControl.xaml
/// </summary>
public partial class LoginControl : UserControl
{
    private readonly LoginControlVM _viewModel;

    public LoginControl()
    {
        InitializeComponent();
        _viewModel = (LoginControlVM?) VmLocator.Resolve<LoginControl>();
        DataContext = _viewModel;
    }
}
