using System;
using System.Windows;


namespace MeatCountefeitDetector.Utils;

public class GenericWindowFactory<T> : IWindowFactory<T> where T : Window
{
    public T CreateWindow()
    {
        throw new NotImplementedException();
    }
}

