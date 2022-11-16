using System.Windows;
using System.Windows.Controls;


public interface IFrameworkElementFactory
{
    FrameworkElement CreateFrameworkElement<T>(object datacontext) where T : FrameworkElement;
    
}
