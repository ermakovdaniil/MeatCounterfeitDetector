using System.Windows;


namespace MeatCounterfeitDetector.Utils.FrameworkFactory;

public interface IFrameworkElementFactory
{
    FrameworkElement CreateFrameworkElement<T>(object dataСontext) where T : FrameworkElement;
}
