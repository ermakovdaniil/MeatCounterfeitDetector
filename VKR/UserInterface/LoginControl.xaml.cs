using System.Windows.Controls;

using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для ColorPropertyControl.xaml
/// </summary>
public partial class LoginControl : UserControl
{
    private LoginControlVM _viewModel;

    public LoginControl(LoginControlVM vm)
    {
        InitializeComponent();
        DataContext = vm;
        _viewModel = vm;
    }
}