using System.Windows.Controls;

using MeatCounterfeitDetector.Utils;


namespace MeatCounterfeitDetector.UserInterface;

public partial class UserInterfaceSelectControl : UserControl
{
    private readonly UserInterfaceSelectControlVM _viewModel;

    public UserInterfaceSelectControl()
    {
        InitializeComponent();
        _viewModel = (UserInterfaceSelectControlVM?)VmLocator.Resolve<UserInterfaceSelectControl>();
        DataContext = _viewModel;
    }
}
