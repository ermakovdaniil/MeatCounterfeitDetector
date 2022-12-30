using System.Windows.Controls;

using VKR.Utils;


namespace VKR.UserInterface.Admin.CompanyView;

/// <summary>
///     Логика взаимодействия для CompanyControl.xaml
/// </summary>
public partial class CompanyControl : UserControl
{
    private readonly CompanyControlVM _viewModel;

    public CompanyControl()
    {
        InitializeComponent();
        _viewModel = (CompanyControlVM?) VmLocator.Resolve<CompanyControl>();
        DataContext = _viewModel;
    }
}

