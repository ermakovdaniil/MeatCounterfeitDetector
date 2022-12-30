using System.Windows;


namespace VKR.Utils.FrameworkFactory;

public interface IFrameworkElementFactory
{
    FrameworkElement CreateFrameworkElement<T>(object dataСontext) where T : FrameworkElement;
}
