using System.Windows;


namespace MeatCounterfeitDetector.Utils;

internal interface IWindowFactory<T> where T : Window
{
    T CreateWindow();
}
