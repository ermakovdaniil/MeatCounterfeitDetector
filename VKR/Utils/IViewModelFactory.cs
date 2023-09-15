using MeatCountefeitDetector.UserInterface.Admin.Abstract;


namespace MeatCountefeitDetector.Utils;

internal interface IViewModelFactory<TViewModel> where TViewModel : ViewModelBase
{
    TViewModel CreateVM();
}
