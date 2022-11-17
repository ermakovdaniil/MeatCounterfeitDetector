using System.Windows.Controls;

using Autofac;

using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для UserEditControl.xaml
/// </summary>
public partial class UserEditControl
{
    private UserEditControlVM _viewModel;

    public UserEditControl(UserEditControlVM vm)
    {
        InitializeComponent();
        DataContext = vm;
        _viewModel = vm;
    }

    public IContainer Container { get; set; }

}
