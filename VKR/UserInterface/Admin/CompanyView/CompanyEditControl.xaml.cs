using VKR.Utils;


namespace VKR.UserInterface.Admin.CompanyView;

/// <summary>
///     Логика взаимодействия для CompanyEditControl.xaml
/// </summary>
public partial class CompanyEditControl
{
    private readonly CompanyEditControlVM _viewModel;

    public CompanyEditControl()
    {
        InitializeComponent();
        _viewModel = (CompanyEditControlVM?) VmLocator.Resolve<CompanyEditControl>();
        DataContext = _viewModel;
    }
}
