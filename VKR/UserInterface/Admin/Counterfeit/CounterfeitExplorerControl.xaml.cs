using System.Windows.Controls;

using VKR.Utils;
using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для CounterfeitExplorerControl.xaml
/// </summary>
public partial class CounterfeitExplorerControl : UserControl
{ 
    private CounterfeitExplorerControlVM _viewModel;

    public CounterfeitExplorerControl()
    {
        InitializeComponent();
        _viewModel = (CounterfeitExplorerControlVM?)VMLocator.Resolve<CounterfeitExplorerControl>();
        DataContext = _viewModel;
    }
}
