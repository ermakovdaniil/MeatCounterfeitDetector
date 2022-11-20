using System.Windows.Controls;

using Autofac;

using DataAccess.Data;

using VKR.Utils;
using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для UserExplorerControl.xaml
/// </summary>
public partial class UserExplorerControl : UserControl
{
    private UserExplorerControlVM _viewModel;

    public UserExplorerControl()
    {
        InitializeComponent();
        _viewModel = (UserExplorerControlVM?)VMLocator.Resolve<UserExplorerControl>();
        DataContext = _viewModel;
    }
}
