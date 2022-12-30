using System.Windows.Controls;

using VKR.Utils;


namespace VKR.UserInterface.Admin.User;

/// <summary>
///     Логика взаимодействия для UserExplorerControl.xaml
/// </summary>
public partial class UserExplorerControl : UserControl
{
    private readonly UserExplorerControlVM _viewModel;

    public UserExplorerControl()
    {
        InitializeComponent();
        _viewModel = (UserExplorerControlVM?) VmLocator.Resolve<UserExplorerControl>();
        DataContext = _viewModel;
    }
}

