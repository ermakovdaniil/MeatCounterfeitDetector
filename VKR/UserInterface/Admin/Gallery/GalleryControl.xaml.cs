using System.Windows.Controls;

using VKR.Utils;


namespace VKR.UserInterface.Admin.Gallery;

public partial class GalleryControl : UserControl
{
    private readonly GalleryControlVM _viewModel;

    public GalleryControl()
    {
        InitializeComponent();
        _viewModel = (GalleryControlVM?)VmLocator.Resolve<GalleryControl>();
        DataContext = _viewModel;
    }
}

