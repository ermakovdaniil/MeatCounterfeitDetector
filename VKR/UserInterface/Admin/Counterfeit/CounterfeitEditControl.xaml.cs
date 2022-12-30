using VKR.Utils;


namespace VKR.UserInterface.Admin.Counterfeit;

/// <summary>
///     Логика взаимодействия для CounterfeitEditControl.xaml
/// </summary>
public partial class CounterfeitEditControl
{
    private readonly CounterfeitEditControlVM _viewModel;

    public CounterfeitEditControl()
    {
        InitializeComponent();
        _viewModel = (CounterfeitEditControlVM?) VmLocator.Resolve<CounterfeitEditControl>();
        DataContext = _viewModel;
    }
}
