using Autofac;
using System;
using System.Collections.Generic;


namespace MeatCounterfeitDetector.Utils;

public static class VmLocator
{
    public static IComponentContext Container = null!;

    private static Dictionary<Type, Type> VmByViews { get; } = new();

    public static void Register<View, ViewModel>()
    {
        VmByViews[typeof(View)] = typeof(ViewModel);
    }

    public static object Resolve<TView>()
    {
        return Container.Resolve(VmByViews[typeof(TView)]);
    }
}

