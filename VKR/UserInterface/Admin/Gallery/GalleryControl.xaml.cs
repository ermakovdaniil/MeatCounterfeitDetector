using System.Windows.Controls;

using VKR.Utils;
using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для GalleryControl.xaml
/// </summary>
public partial class GalleryControl : UserControl
{
    private GalleryControlVM _viewModel;

    public GalleryControl()
    {
        InitializeComponent();
        _viewModel = (GalleryControlVM?)VMLocator.Resolve<GalleryControl>();
        DataContext = _viewModel;
    }
}
