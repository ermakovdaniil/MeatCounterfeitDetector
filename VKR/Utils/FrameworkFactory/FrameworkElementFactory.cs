using System.Windows;
using System.Windows.Controls;

using Autofac;

public class FrameworkElementFactory : IFrameworkElementFactory
{
    private readonly IComponentContext container;

    public FrameworkElementFactory(IComponentContext container)
    {
        this.container = container;
    }

    public FrameworkElement CreateFrameworkElement<T>(object datacontext) where T : FrameworkElement
    {
        var fe = container.Resolve<T>();

        if (datacontext is not null)
        {
            fe.DataContext = datacontext;

        }
        return fe;
    }
}