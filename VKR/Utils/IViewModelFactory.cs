using VKR.ViewModel;

namespace VKR.Utils
{
    internal interface IViewModelFactory<TViewModel> where TViewModel : ViewModelBase
    {
        TViewModel CreateVM();
    }
}
