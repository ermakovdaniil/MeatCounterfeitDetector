using System.Windows.Controls;

using Autofac;

using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для CounterfeitEditControl.xaml
/// </summary>
public partial class CounterfeitEditControl
{
    private CounterfeitEditControlVM _viewModel;

    public CounterfeitEditControl(CounterfeitEditControlVM vm)
    {
        InitializeComponent();
        DataContext = vm;
        _viewModel = vm;
    }

    public IContainer Container { get; set; }

}
