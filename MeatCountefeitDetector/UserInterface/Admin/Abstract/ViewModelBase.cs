using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace MeatCountefeitDetector.UserInterface.Admin.Abstract;

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

