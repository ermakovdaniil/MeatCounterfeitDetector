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
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainWindowVM>().AsSelf();
            builder.RegisterType<ColorPropertyControl>().AsSelf();
            builder.RegisterType<ColorPropertyControlVM>().AsSelf();
            builder.RegisterType<ColorPropertyEditWindow>().AsSelf();
            builder.RegisterType<ColorPropertyEditWindowVM>().AsSelf();
            builder.RegisterType<CompanyControl>().AsSelf();
            builder.RegisterType<CompanyControlVM>().AsSelf();
            builder.RegisterType<CompanyEditWindow>().AsSelf();
            builder.RegisterType<CompanyEditWindowVM>().AsSelf();
            builder.RegisterType<CounterfeitEditWindow>().AsSelf();
            builder.RegisterType<CounterfeitEditWindowVM>().AsSelf();
            builder.RegisterType<CounterfeitExplorerControl>().AsSelf();
            builder.RegisterType<CounterfeitExplorerControlVM>().AsSelf();
            builder.RegisterType<MainAdminControl>().AsSelf();
            builder.RegisterType<PropertyExplorerControl>().AsSelf();
            builder.RegisterType<ResultControl>().AsSelf();
            builder.RegisterType<ResultControlVM>().AsSelf();
            builder.RegisterType<ShapePropertyControl>().AsSelf();
            builder.RegisterType<ShapePropertyControlVM>().AsSelf();
            builder.RegisterType<ShapePropertyEditWindow>().AsSelf();
            builder.RegisterType<ShapePropertyEditWindowVM>().AsSelf();
            builder.RegisterType<TechnologistControl>().AsSelf();
            builder.RegisterType<TechnologistControlVM>().AsSelf();
            builder.RegisterType<UserEditWindow>().AsSelf();
            builder.RegisterType<UserEditWindowVM>().AsSelf();
            builder.RegisterType<UserExplorerControl>().AsSelf();
            builder.RegisterType<UserExplorerControlVM>().AsSelf();
            builder.RegisterType<UserControlFactory>().As<IUserControlFactory>();

            // builder.RegisterAssemblyTypes(typeof(App).Assembly)
            //        .Where(t => t.Name.EndsWith("VM"))
            //        .AsSelf();
            // builder.RegisterAssemblyTypes(typeof(App).Assembly)
            //        .Where(t => t.Name.EndsWith("Control"))
            //        .AsSelf();
            Container = builder.Build();
            
            var mainWindow = Container.Resolve<MainWindow>();
            var loginControl = Container.Resolve<LoginControl>();
            mainWindow._viewModel.SetNewContent(loginControl);
            mainWindow.Show();
        }
    }
}
