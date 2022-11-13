using System.Windows.Controls;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для CounterfeitExplorerControl.xaml
/// </summary>
public partial class CounterfeitExplorerControl : UserControl
{
    public CounterfeitExplorerControl()
    {
        InitializeComponent();
        DataContext = new CounterfeitExplorerControl();
    }
}
