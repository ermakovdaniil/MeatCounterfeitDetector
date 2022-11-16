// using System;
// using System.Collections.Generic;
// using System.Security.Permissions;
//
// using Autofac;
//
//
// namespace VKR.Utils;
//
// public class VMLocator
// {
//     private readonly IComponentContext _container;
//     private Dictionary<Type,Type> VMByViews { get; set; }
//     
//     public VMLocator(IComponentContext container)
//     {
//         _container = container;
//     }
//
//     public void Register<View, ViewModel>()
//     {
//         VMByViews[typeof(View)] = typeof(ViewModel);
//     }
//
//     public object Resolve<TView>()
//     {
//         return _container.Resolve(VMByViews[typeof(TView)]);
//     }
// }
