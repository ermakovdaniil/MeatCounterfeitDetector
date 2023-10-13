using MeatCounterfeitDetector.Utils;


namespace MeatCounterfeitDetector.UserInterface.Admin.Gallery;

public partial class GalleryEditControl
{
    private readonly GalleryEditControlVM _viewModel;

    public GalleryEditControl()
    {
        InitializeComponent();
        _viewModel = (GalleryEditControlVM?)VmLocator.Resolve<GalleryEditControl>();
        DataContext = _viewModel;
    }
}
