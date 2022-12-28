using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Autofac;

using VKR.Utils;
using VKR.ViewModel;

using MessageBox = HandyControl.Controls.MessageBox;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для TechnologistControl.xaml
/// </summary>
public partial class TechnologistControl : UserControl
{
    private TechnologistControlVM _viewModel;

    public TechnologistControl()
    {
        InitializeComponent();
        _viewModel = (TechnologistControlVM?)VMLocator.Resolve<TechnologistControl>();
        DataContext = _viewModel;
    }
}
