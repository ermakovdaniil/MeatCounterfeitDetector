using DataAccess.Data;

using System.Windows.Controls;

using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для CompanyControl.xaml
/// </summary>
public partial class CompanyControl : UserControl
{
    private readonly ResultDBContext _context;

    private CompanyControlVM _viewModel;

    public CompanyControl(CompanyControlVM vm)
    {
        InitializeComponent();
        DataContext = vm;
        _viewModel = vm;
    }
}
