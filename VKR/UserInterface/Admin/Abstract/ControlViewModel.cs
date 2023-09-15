// using System.Windows.Controls;
//
//
// namespace MeatCountefeitDetector.ViewModel;
//
// public class ControlViewModel : ViewModelBase
// {
//     private readonly IUserControlFactory _factory;
//     protected MainWindowVM _mainWindowVm;
//
//     public ControlViewModel(IUserControlFactory factory, MainWindowVM mainWindowVm)
//     {
//         _factory = factory;
//         _mainWindowVm = mainWindowVm;
//     }
//
//     protected void changeControl<T>(object param) where T : UserControl
//     {
//         var control = _factory.CreateFrameworkElement<T>(null);
//         _mainWindowVm.SetNewContent(control);
//     }
// }


