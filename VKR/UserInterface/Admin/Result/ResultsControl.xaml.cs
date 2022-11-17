using System.Windows.Controls;

using Autofac;

using DataAccess.Data;

using VKR.Utils;
using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для ResultControl.xaml
/// </summary>
public partial class ResultControl : UserControl
{
    private ResultControlVM _viewModel;

    public ResultControl()
    {
        InitializeComponent();
        _viewModel = (ResultControlVM?)VMLocator.Resolve<ResultControl>();
        DataContext = _viewModel;
    }

    public IContainer Container { get; set; }
}
