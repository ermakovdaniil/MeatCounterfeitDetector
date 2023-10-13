using System.Windows.Controls;

using MeatCounterfeitDetector.Utils;


namespace MeatCounterfeitDetector.UserInterface.Admin.Gallery;

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

