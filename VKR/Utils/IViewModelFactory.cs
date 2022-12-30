using VKR.UserInterface.Admin.Abstract;


namespace VKR.Utils;

internal interface IViewModelFactory<TViewModel> where TViewModel : ViewModelBase
{
    TViewModel CreateVM();
}
