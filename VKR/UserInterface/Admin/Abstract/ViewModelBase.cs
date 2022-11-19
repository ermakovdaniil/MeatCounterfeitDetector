using System.ComponentModel;
using System.Runtime.CompilerServices;

using PropertyChanged;


namespace VKR.ViewModel;

/// <summary>
///     Абстрактный класс для VM
/// </summary>
//[AddINotifyPropertyChangedInterface]
public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
