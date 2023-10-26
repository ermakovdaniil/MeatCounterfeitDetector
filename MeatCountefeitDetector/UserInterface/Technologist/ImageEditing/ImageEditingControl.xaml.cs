using System.Windows.Controls;

using MeatCounterfeitDetector.Utils;


namespace MeatCounterfeitDetector.UserInterface.Technologist.Edit;

public partial class ImageEditingControl : UserControl
{
    private readonly ImageEditingControlVM _viewModel;

    public ImageEditingControl()
    {
        InitializeComponent();
        _viewModel = (ImageEditingControlVM?)VmLocator.Resolve<ImageEditingControl>();
        DataContext = _viewModel;
    }
}

