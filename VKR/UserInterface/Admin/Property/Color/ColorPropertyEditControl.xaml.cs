using System.Windows.Controls;

using Autofac;

using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для ColorPropertyEditControl.xaml
/// </summary>
public partial class ColorPropertyEditControl
{
    private ColorPropertyEditControlVM _viewModel;

    public ColorPropertyEditControl(ColorPropertyEditControlVM vm)
    {
        InitializeComponent();
        DataContext = vm;
        _viewModel = vm;

        //_viewModel = Container.Resolve<ColorPropertyControlVM>();
    }

    public IContainer Container { get; set; }
}
