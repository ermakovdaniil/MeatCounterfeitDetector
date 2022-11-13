using System.Windows.Controls;

using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для ShapePropertyControl.xaml
/// </summary>
public partial class ShapePropertyControl : UserControl
{
    public ShapePropertyControl()
    {
        InitializeComponent();
        DataContext = new ShapePropertyControlVM();
    }
}
