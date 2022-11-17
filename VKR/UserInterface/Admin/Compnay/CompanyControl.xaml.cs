using System.Windows.Controls;

using Autofac;

using DataAccess.Data;

using VKR.Utils;
using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для CompanyControl.xaml
/// </summary>
public partial class CompanyControl : UserControl
{
    private CompanyControlVM _viewModel;

    public CompanyControl()
    {
        InitializeComponent();
        _viewModel = (CompanyControlVM?)VMLocator.Resolve<CompanyControl>();
        DataContext = _viewModel;
    }

    public IContainer Container { get; set; }
}
