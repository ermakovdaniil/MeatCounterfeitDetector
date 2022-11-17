using System.Windows.Controls;

using Autofac;

using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для ShapePropertyEditControl.xaml
/// </summary>
public partial class ShapePropertyEditControl
{
    private ShapePropertyEditControlVM _viewModel;

    public ShapePropertyEditControl(ShapePropertyEditControlVM vm)
    {
        InitializeComponent();
        DataContext = vm;
        _viewModel = vm;
    }

    public IContainer Container { get; set; }
}
