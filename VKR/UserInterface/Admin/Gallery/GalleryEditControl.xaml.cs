using VKR.Utils;


namespace VKR.UserInterface.Admin.Gallery;

/// <summary>
///     Логика взаимодействия для GalleryEditControl.xaml
/// </summary>
public partial class GalleryEditControl
{
    private readonly GalleryEditControlVM _viewModel;

    public GalleryEditControl()
    {
        InitializeComponent();
        _viewModel = (GalleryEditControlVM?) VmLocator.Resolve<GalleryEditControl>();
        DataContext = _viewModel;
    }
}
