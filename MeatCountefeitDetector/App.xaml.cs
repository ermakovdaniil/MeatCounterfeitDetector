using Autofac;
using DataAccess.Data;
using ImageAnalyzis;
using System.Globalization;
using System.Threading;
using System.Windows;
using MeatCountefeitDetector.UserInterface;
using MeatCountefeitDetector.UserInterface.Admin;
using MeatCountefeitDetector.UserInterface.Admin.Counterfeit;
using MeatCountefeitDetector.UserInterface.Admin.Gallery;
using MeatCountefeitDetector.UserInterface.Admin.Result;
using MeatCountefeitDetector.UserInterface.Admin.User;
using MeatCountefeitDetector.UserInterface.Technologist;
using MeatCountefeitDetector.Utils;
using MeatCountefeitDetector.Utils.Dialog;
using MeatCountefeitDetector.Utils.FrameworkFactory;
using MeatCountefeitDetector.Utils.IOService;
using MeatCountefeitDetector.Utils.MainWindowControlChanger;
using MeatCountefeitDetector.Utils.MessageBoxService;
using MeatCountefeitDetector.Utils.UserService;
using FrameworkElementFactory = MeatCountefeitDetector.Utils.FrameworkFactory.FrameworkElementFactory;


namespace MeatCountefeitDetector;

public partial class App : Application
{
    private IContainer Container { get; set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentCulture = CultureInfo.DefaultThreadCurrentUICulture;
        var builder = new ContainerBuilder();
        builder.RegisterType<CounterfeitKBContext>().AsSelf();
        builder.RegisterType<ResultDBContext>().SingleInstance().AsSelf();

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
        builder.RegisterType<ImageAnalyzis.ImageAnalyzer>().As<IImageAnalyzer>();
        builder.RegisterType<FileDialogService>().As<IFileDialogService>();
        builder.RegisterType<HandyMessageBoxService>().As<IMessageBoxService>();
        builder.RegisterType<UserService>().As<IUserService>().SingleInstance();

        Container = builder.Build();

        VmLocator.Container = Container;
        VmLocator.Register<MainWindow, MainWindowVM>();
        VmLocator.Register<LoginControl, LoginControlVM>();
        VmLocator.Register<TechnologistControl, TechnologistControlVM>();
        VmLocator.Register<MainAdminControl, MainAdminControlVM>();
        VmLocator.Register<UserExplorerControl, UserExplorerControlVM>();
        VmLocator.Register<ResultControl, ResultControlVM>();
        VmLocator.Register<CounterfeitExplorerControl, CounterfeitExplorerControlVM>();
        VmLocator.Register<GalleryEditControl, GalleryEditControlVM>();
        VmLocator.Register<GalleryControl, GalleryControlVM>();
        VmLocator.Register<UserEditControl, UserEditControlVM>();
        VmLocator.Register<CounterfeitEditControl, CounterfeitEditControlVM>();

        var mainWindow = Container.Resolve<MainWindow>();
        mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        mainWindow.Show();

        var navigator = Container.Resolve<NavigationManager>();

        navigator.Navigate<LoginControl>(new WindowParameters
        {
            Height = 300,
            Width = 350,
            Title = "Вход в систему",
            StartupLocation = WindowStartupLocation.CenterScreen,
        });
    }
}

