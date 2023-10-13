using MeatCounterfeitDetector.UserInterface.Admin.Abstract;


namespace MeatCounterfeitDetector.Utils;

internal interface IViewModelFactory<TViewModel> where TViewModel : ViewModelBase
{
    TViewModel CreateVM();
}
