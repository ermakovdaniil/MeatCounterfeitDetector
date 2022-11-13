using System.Windows.Controls;

using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для ColorPropertyControl.xaml
/// </summary>
public partial class ResultControl : UserControl
{
    public ResultControl()
    {
        InitializeComponent();
        DataContext = new ResultControlVM();
    }
}
