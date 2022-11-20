using VKR.Utils;
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
        _viewModel = (ShapePropertyEditControlVM?)VMLocator.Resolve<ShapePropertyEditControl>();
        DataContext = _viewModel;
    }
}
