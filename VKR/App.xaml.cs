using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using Autofac;

using DataAccess.Data;
using DataAccess.Models;

using VKR.Utils;
using VKR.Utils.Dialog;
using VKR.Utils.FrameworkFactory;
using VKR.Utils.MainWindowControlChanger;
using VKR.View;
using VKR.ViewModel;


namespace VKR;

/// <summary>
///     Interaction logic for App.xaml
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
        builder.RegisterAssemblyTypes(typeof(App).Assembly)
               .Where(t => t.Name.EndsWith("VM"))
               .AsSelf();

        builder.RegisterAssemblyTypes(typeof(App).Assembly)
               .Where(t => t.Name.EndsWith("Control"))
               .AsSelf();

        builder.RegisterAssemblyTypes(typeof(App).Assembly)
               .Where(t => t.Name.EndsWith("Window"))
               .AsSelf();
        builder.RegisterType<MainWindow>().AsSelf().SingleInstance();
        builder.RegisterType<MainWindowVM>().AsSelf().SingleInstance();
        builder.RegisterType<FrameworkElementFactory>().As<IFrameworkElementFactory>();
        builder.RegisterType<NavigationManager>().AsSelf().SingleInstance();
        builder.RegisterType<UserControlFactory>().AsSelf();
        builder.RegisterType<DialogService>().AsSelf();
        
        Container = builder.Build();

        VMLocator.Container = Container;

        VMLocator.Register<MainWindow, MainWindowVM>();
        VMLocator.Register<LoginControl, LoginControlVM>();
        VMLocator.Register<TechnologistControl, TechnologistControlVM>();
        VMLocator.Register<UserExplorerControl, UserExplorerControlVM>();
        VMLocator.Register<ResultControl, ResultControlVM>();
        VMLocator.Register<ColorPropertyControl, ColorPropertyControlVM>(); 
        VMLocator.Register<ShapePropertyControl, ShapePropertyControlVM>();
        VMLocator.Register<CounterfeitExplorerControl, CounterfeitExplorerControlVM>();
        VMLocator.Register<CompanyControl, CompanyControlVM>();

        var mainWindow = Container.Resolve<MainWindow>();
        mainWindow.Show();

        var navigator = Container.Resolve<NavigationManager>();
        navigator.Navigate<LoginControl>();
        
        var ds = Container.Resolve<DialogService>();

        //ds.ShowDialog<ColorPropertyEditControl>(data: new Color()
        //{
        //    Name = "name",
        //    UpLine = "upline",
        //    BotLine = "botline",
        //});
    }
}
