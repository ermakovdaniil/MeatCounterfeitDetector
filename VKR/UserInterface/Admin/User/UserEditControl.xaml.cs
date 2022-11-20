using VKR.Utils;
using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для UserEditControl.xaml
/// </summary>
public partial class UserEditControl
{
    private UserEditControlVM _viewModel;

    public UserEditControl()
    {
        InitializeComponent();
        _viewModel = (UserEditControlVM?)VMLocator.Resolve<UserEditControl>();
        DataContext = _viewModel;
    }
}
