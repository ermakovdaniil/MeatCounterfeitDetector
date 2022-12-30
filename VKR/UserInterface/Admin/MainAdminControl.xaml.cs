using System.Windows.Controls;

using VKR.Utils;


namespace VKR.UserInterface.Admin;

/// <summary>
///     Логика взаимодействия для MainAdminControl.xaml
/// </summary>
public partial class MainAdminControl : UserControl
{
    private readonly MainAdminControlVM _viewModel;

    public MainAdminControl()
    {
        InitializeComponent();
        _viewModel = (MainAdminControlVM?) VmLocator.Resolve<MainAdminControl>();
        DataContext = _viewModel;
    }
}
