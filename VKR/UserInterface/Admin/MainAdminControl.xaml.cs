using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Autofac;

using VKR.Utils;
using VKR.ViewModel;

using MessageBox = HandyControl.Controls.MessageBox;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для MainAdminControl.xaml
/// </summary>
public partial class MainAdminControl : UserControl
{
    private MainAdminControlVM _viewModel;

    public MainAdminControl()
    {
        InitializeComponent();
        _viewModel = (MainAdminControlVM?)VMLocator.Resolve<MainAdminControl>();
        DataContext = _viewModel;
    }
}