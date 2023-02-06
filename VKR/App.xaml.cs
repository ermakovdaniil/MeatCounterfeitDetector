using Autofac;
using DataAccess.Data;
using System.Globalization;
using System.Threading;
using System.Windows;
using VKR.UserInterface;
using VKR.UserInterface.Admin;
using VKR.UserInterface.Admin.Counterfeit;
using VKR.UserInterface.Admin.Gallery;
using VKR.UserInterface.Admin.Result;
using VKR.UserInterface.Admin.User;
using VKR.UserInterface.Technologist;
using VKR.Utils;
using VKR.Utils.Dialog;
using VKR.Utils.FrameworkFactory;
using VKR.Utils.ImageAnalyzis;
using VKR.Utils.IOService;
using VKR.Utils.MainWindowControlChanger;
using VKR.Utils.MessageBoxService;
using FrameworkElementFactory = VKR.Utils.FrameworkFactory.FrameworkElementFactory;


namespace VKR;

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
        builder.RegisterType<ResultDBContext>().AsSelf();

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

        builder.RegisterType<ImageAnalyzer>().As<IImageAnalyzer>();
        builder.RegisterType<FileDialogService>().As<IFileDialogService>();
        builder.RegisterType<HandyMessageBoxService>().As<IMessageBoxService>();


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

