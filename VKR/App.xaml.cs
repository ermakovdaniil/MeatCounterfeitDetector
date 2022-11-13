using System.Globalization;
using System.Threading;
using System.Windows;
using Autofac;
using DataAccess.Data;

using VKR.View;
using VKR.ViewModel;

namespace VKR
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IContainer Container { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.DefaultThreadCurrentUICulture;
            var builder = new ContainerBuilder();
            builder.RegisterType<CounterfeitKBContext>().AsSelf();
            builder.RegisterType<ResultDBContext>().AsSelf();
            builder.RegisterType<UserDBContext>().AsSelf();
            builder.RegisterType<LoginControl>().AsSelf();
            builder.RegisterType<LoginControlVM>().AsSelf();
            builder.RegisterType<MainWindowVM>().AsSelf().SingleInstance();
            builder.RegisterType<MainWindow>().AsSelf().SingleInstance();
            builder.RegisterType<ColorPropertyControlVM>().AsSelf();
            builder.RegisterType<UserControlFactory>().As<IUserControlFactory>();
            
            builder.RegisterType<ColorPropertyControl>().AsSelf();

            // builder.RegisterAssemblyTypes(typeof(App).Assembly)
            //        .Where(t => t.Name.EndsWith("VM"))
            //        .AsSelf();
            // builder.RegisterAssemblyTypes(typeof(App).Assembly)
            //        .Where(t => t.Name.EndsWith("Control"))
            //        .AsSelf();
            Container = builder.Build();
            
            var mainWindow = Container.Resolve<MainWindow>();
            var loginControl = Container.Resolve<LoginControl>();
            mainWindow.VM.SetNewContent(loginControl);
            mainWindow.Show();
        }
    }
}
