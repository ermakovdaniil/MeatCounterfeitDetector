//using Autofac;
//using System;

//namespace VKR.Utils
//{
//    public static class ServicesExtensions
//    {
//        public static void AddFormFactory<TForm>(this ContainerBuilder builder) 
//            where TForm : class
//        {
//            builder.RegisterType<TForm>().InstancePerDependency();
//            builder.Register<Func<TForm>>(x => () => x.Resolve<TForm>()!).SingleInstance();
//            builder.RegisterGeneric(typeof(AbstractFactory<>)).As(typeof(IAbstractFactory<>)).SingleInstance();

//            // builder.AddTransient<TForm>();
//            // builder.AddSingleton<Func<TForm>>(x => () => x.GetService<TForm>()!);
//            // builder.AddSingleton<IAbstractFactory<TForm>, AbstractFactory<TForm>>();
//        }
//    }
//}
