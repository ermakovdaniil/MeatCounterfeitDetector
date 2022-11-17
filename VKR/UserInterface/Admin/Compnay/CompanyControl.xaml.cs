using System.Windows.Controls;

using Autofac;

using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для CompanyControl.xaml
/// </summary>
public partial class CompanyControl : UserControl
{
    private CompanyControlVM _viewModel;

    public CompanyControl(CompanyControlVM vm)
    {
        InitializeComponent();
        DataContext = vm;
        _viewModel = vm;

        //_viewModel = Container.Resolve<ColorPropertyControlVM>();
    }

    public IContainer Container { get; set; }
}
