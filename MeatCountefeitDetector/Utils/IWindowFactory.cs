using System.Windows;


namespace MeatCountefeitDetector.Utils;

internal interface IWindowFactory<T> where T : Window
{
    T CreateWindow();
}
