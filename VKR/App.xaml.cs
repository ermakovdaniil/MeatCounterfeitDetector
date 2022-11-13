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
            builder.RegisterType<MainWindowVM>().AsSelf();
            builder.RegisterType<ColorPropertyControlVM>().AsSelf();
            
            Container = builder.Build();
            // TODO: Сервис работы с окнами (открытие, закрытие)

            //builder.AddFormFactory<ColorPropertyEditWindow>();
            //builder.AddFormFactory<CompanyEditWindow>();
            //builder.AddFormFactory<CounterfeitEditWindow>();
            //builder.AddFormFactory<ShapePropertyEditWindow>();
            //builder.AddFormFactory<UserEditWindow>();

            //using var container = builder.Build();

            //using var scope = container.BeginLifetimeScope();
            //var createHolding = scope.Resolve<A.Factory>();
            var vm = Container.Resolve<MainWindowVM>();
            var loginControl = Container.Resolve<LoginControl>();
            var loginControlVM = Container.Resolve<LoginControlVM>();
            loginControl.DataContext = loginControlVM;
            vm.SetNewContent(loginControl);
            var mainWindow = new MainWindow() { DataContext = vm };
            mainWindow.Show();
        }
    }
}
