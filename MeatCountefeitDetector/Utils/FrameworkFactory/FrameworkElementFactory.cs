using Autofac;
using System.Windows;


namespace MeatCounterfeitDetector.Utils.FrameworkFactory;

public class FrameworkElementFactory : IFrameworkElementFactory
{
    private readonly IComponentContext container;

    public FrameworkElementFactory(IComponentContext container)
    {
        this.container = container;
    }

    public FrameworkElement CreateFrameworkElement<T>(object dataСontext) where T : FrameworkElement
    {
        var fe = container.Resolve<T>();

        if (dataСontext is not null)
        {
            fe.DataContext = dataСontext;
        }

        return fe;
    }
}