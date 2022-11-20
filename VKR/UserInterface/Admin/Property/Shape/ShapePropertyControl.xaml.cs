using System.Windows.Controls;

using VKR.Utils;
using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для ShapePropertyControl.xaml
/// </summary>
public partial class ShapePropertyControl : UserControl
{
    private ShapePropertyControlVM _viewModel;
    
    public ShapePropertyControl()
    {
        InitializeComponent();
        _viewModel = (ShapePropertyControlVM?)VMLocator.Resolve<ShapePropertyControl>();
        DataContext = _viewModel;
    }
}
