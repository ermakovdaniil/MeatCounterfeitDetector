using System.Windows.Controls;

using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для UserExplorerControl.xaml
/// </summary>
public partial class UserExplorerControl : UserControl
{
    public UserExplorerControl()
    {
        InitializeComponent();
        DataContext = new UserExplorerControlVM();
    }
}
