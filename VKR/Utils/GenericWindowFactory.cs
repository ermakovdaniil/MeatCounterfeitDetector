using System;
using System.Windows;


namespace VKR.Utils;

public class GenericWindowFactory<T> : IWindowFactory<T> where T : Window
{
    public T CreateWindow()
    {
        throw new NotImplementedException();
    }
}

