using System.Windows;

namespace VKR.Utils
{
    internal interface IWindowFactory<T> where T : Window
    {
        T CreateWindow();
    }
}
