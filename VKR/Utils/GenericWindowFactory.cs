using System.Windows;


namespace VKR.Utils;

public class GenericWindowFactory<T>: IWindowFactory<T> where T : Window
{
    public GenericWindowFactory()
    {
        
    }

    public T CreateWindow()
    {
        throw new System.NotImplementedException();
    }
}
