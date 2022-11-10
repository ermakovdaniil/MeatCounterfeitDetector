using System.Globalization;
using System.Threading;
using System.Windows;
using Autofac;
using DataAccess.Data;

using VKR.View;
using VKR.ViewModel;

namespace FlowModelDesktop
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
        }

        public void AppStartUp(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow();
            var vm = Container.Resolve<MainWindowVM>();
            MainWindow.DataContext = vm;
            var loginControl = Container.Resolve<LoginControl>();
            var loginControlVM = Container.Resolve<LoginControlVM>();
            loginControl.DataContext = loginControlVM;
            vm.SetNewContent(loginControl);
            mainWindow.Show();
        }
    }
}
