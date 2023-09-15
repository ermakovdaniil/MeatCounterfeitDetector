namespace MeatCountefeitDetector.UserInterface;

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

