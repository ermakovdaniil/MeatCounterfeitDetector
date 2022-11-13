using DataAccess.Models;

using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для ShapePropertyWindow.xaml
/// </summary>
public partial class ShapePropertyEditWindow
{
    public ShapePropertyEditWindow(Shape shape)
    {
        InitializeComponent();
        var vm = new ShapePropertyEditWindowVM(shape);
        DataContext = vm;

        //vm.ClosingRequest += (sender, e) => Close();
    }
}
