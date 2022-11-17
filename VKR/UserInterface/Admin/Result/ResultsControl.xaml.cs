using System.Windows.Controls;

using Autofac;

using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для ResultControl.xaml
/// </summary>
public partial class ResultControl : UserControl
{
    private ResultControlVM _viewModel;

    public ResultControl(ResultControlVM vm)
    {
        InitializeComponent();
        DataContext = vm;
        _viewModel = vm;

        //_viewModel = Container.Resolve<ColorPropertyControlVM>();
    }

    public IContainer Container { get; set; }
}
