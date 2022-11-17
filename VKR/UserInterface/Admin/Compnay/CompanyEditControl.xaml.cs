using System.Windows.Controls;

using Autofac;

using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для CompanyEditControl.xaml
/// </summary>
public partial class CompanyEditControl
{
    private CompanyEditControlVM _viewModel;

    public CompanyEditControl(CompanyEditControlVM vm)
    {
        InitializeComponent();
        DataContext = vm;
        _viewModel = vm;
    }

    public IContainer Container { get; set; }
}