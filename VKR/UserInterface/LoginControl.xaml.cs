using System.Windows.Controls;

using VKR.Utils;
using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для ColorPropertyControl.xaml
/// </summary>
public partial class LoginControl : UserControl, IPasswordSupplier
{
    private LoginControlVM _viewModel;

    public LoginControl()
    {
        InitializeComponent();
        _viewModel = (LoginControlVM?)VMLocator.Resolve<LoginControl>();
        DataContext = _viewModel;
    }

    public string GetPassword()
    {
        return pwdBox.Password;
    }
}