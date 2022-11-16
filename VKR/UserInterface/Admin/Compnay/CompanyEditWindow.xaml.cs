using DataAccess.Models;

using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для CompanyEditWindow.xaml
/// </summary>
public partial class CompanyEditWindow
{
    private CompanyEditWindowVM _viewModel;

    public CompanyEditWindow(Company company, CompanyEditWindowVM vm)
    {
        InitializeComponent();
        DataContext = vm;
        _viewModel = vm;
    }
}