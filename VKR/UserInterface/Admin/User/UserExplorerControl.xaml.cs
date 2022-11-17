using System.Windows.Controls;

using Autofac;

using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для UserExplorerControl.xaml
/// </summary>
public partial class UserExplorerControl : UserControl
{
    private UserExplorerControlVM _viewModel;

    public UserExplorerControl(UserExplorerControlVM vm)
    {
        InitializeComponent();
        DataContext = vm;
        _viewModel = vm;
    }

    public IContainer Container { get; set; }
}
