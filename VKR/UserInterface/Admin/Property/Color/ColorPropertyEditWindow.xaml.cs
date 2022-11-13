using DataAccess.Models;

using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для ColorPropertyWindow.xaml
/// </summary>
public partial class ColorPropertyEditWindow
{
    public ColorPropertyEditWindow(Color color)
    {
        InitializeComponent();
        var vm = new ColorPropertyEditWindowVM(color);
        DataContext = vm;

        //vm.ClosingRequest += (sender, e) => Close();
    }
}
