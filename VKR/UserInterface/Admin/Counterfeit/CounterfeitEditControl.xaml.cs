using VKR.Utils;
using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для CounterfeitEditControl.xaml
/// </summary>
public partial class CounterfeitEditControl
{
    private CounterfeitEditControlVM _viewModel;

    public CounterfeitEditControl()
    {
        InitializeComponent();
        _viewModel = (CounterfeitEditControlVM?)VMLocator.Resolve<CounterfeitEditControl>();
        DataContext = _viewModel;
    }
}