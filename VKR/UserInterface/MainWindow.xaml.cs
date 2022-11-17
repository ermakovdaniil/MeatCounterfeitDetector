using VKR.ViewModel;

namespace VKR.View;

/// <summary>
///     Логика взаимодействия для MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindowVM _viewModel;

    public MainWindow(MainWindowVM vm)
    {
        InitializeComponent();
        _viewModel = vm;
        DataContext = vm;
    }
}
