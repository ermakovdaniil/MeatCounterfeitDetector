using VKR.Utils;
using VKR.ViewModel;


namespace VKR.View;

/// <summary>
///     Логика взаимодействия для GalleryEditControl.xaml
/// </summary>
public partial class GalleryEditControl
{
    private GalleryEditControlVM _viewModel;

    public GalleryEditControl()
    {
        InitializeComponent();
        _viewModel = (GalleryEditControlVM?)VMLocator.Resolve<GalleryEditControl>();
        DataContext = _viewModel;
    }
}