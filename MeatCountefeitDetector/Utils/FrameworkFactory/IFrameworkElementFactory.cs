using System.Windows;


namespace MeatCountefeitDetector.Utils.FrameworkFactory;

public interface IFrameworkElementFactory
{
    FrameworkElement CreateFrameworkElement<T>(object dataСontext) where T : FrameworkElement;
}
