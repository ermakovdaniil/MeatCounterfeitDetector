using VKR.Utils;
using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для CompanyEditControl.xaml
/// </summary>
public partial class CompanyEditControl
{
    private CompanyEditControlVM _viewModel;

    public CompanyEditControl()
    {
        InitializeComponent();
        _viewModel = (CompanyEditControlVM?)VMLocator.Resolve<CompanyEditControl>();
        DataContext = _viewModel;
    }
}