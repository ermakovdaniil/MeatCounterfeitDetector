using System;
using System.Collections.Generic;
using System.Security.Permissions;

using Autofac;


namespace VKR.Utils;

public static class VMLocator
{
    public static IComponentContext Container;

    private static Dictionary<Type, Type> VMByViews { get; set; } = new Dictionary<Type, Type>();
    
    public static void Register<View, ViewModel>()
    {
        VMByViews[typeof(View)] = typeof(ViewModel);
    }

    public static object Resolve<TView>()
    {
        return Container.Resolve(VMByViews[typeof(TView)]);
    }
}
