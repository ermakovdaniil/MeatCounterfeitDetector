using System;
using System.Windows;


namespace MeatCounterfeitDetector.Utils;

public class GenericWindowFactory<T> : IWindowFactory<T> where T : Window
{
    public T CreateWindow()
    {
        throw new NotImplementedException();
    }
}

