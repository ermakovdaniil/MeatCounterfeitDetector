using System.Windows.Controls;

using VKR.Utils;


namespace VKR.UserInterface.Admin.Gallery;

/// <summary>
///     Логика взаимодействия для GalleryControl.xaml
/// </summary>
public partial class GalleryControl : UserControl
{
    private readonly GalleryControlVM _viewModel;

    public GalleryControl()
    {
        InitializeComponent();
        _viewModel = (GalleryControlVM?) VmLocator.Resolve<GalleryControl>();
        DataContext = _viewModel;
    }
}

