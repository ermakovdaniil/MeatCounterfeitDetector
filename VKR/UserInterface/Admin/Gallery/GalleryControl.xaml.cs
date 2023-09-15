using System.Windows.Controls;

using MeatCountefeitDetector.Utils;


namespace MeatCountefeitDetector.UserInterface.Admin.Gallery;

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

