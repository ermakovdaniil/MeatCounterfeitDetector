using DataAccess.Models;

using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для ColorPropertyWindow.xaml
/// </summary>
public partial class ColorPropertyEditControl
{
    public ColorPropertyEditControl()
    {
        InitializeComponent();
        var vm = new ColorPropertyEditWindowVM();
        DataContext = vm;

        //vm.ClosingRequest += (sender, e) => Close();
    }
}
