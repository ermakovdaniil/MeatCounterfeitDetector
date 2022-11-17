using System.Windows.Controls;

using Autofac;

using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для CounterfeitExplorerControl.xaml
/// </summary>
public partial class CounterfeitExplorerControl : UserControl
{ 
    private CounterfeitExplorerControlVM _viewModel;

    public CounterfeitExplorerControl(CounterfeitExplorerControlVM vm)
    {
        InitializeComponent();
        DataContext = vm;
        _viewModel = vm;
    }

    public IContainer Container { get; set; }
}
