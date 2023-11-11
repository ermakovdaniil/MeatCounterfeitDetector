using Autofac;
using DataAccess.Data;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Windows;
using ClientAPI;
using MeatCounterfeitDetector.UserInterface;
using MeatCounterfeitDetector.UserInterface.Admin;
using MeatCounterfeitDetector.UserInterface.Admin.Counterfeit;
using MeatCounterfeitDetector.UserInterface.Admin.Gallery;
using MeatCounterfeitDetector.UserInterface.Admin.Result;
using MeatCounterfeitDetector.UserInterface.Admin.User;
using MeatCounterfeitDetector.UserInterface.Technologist;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.Dialog;
using MeatCounterfeitDetector.Utils.FrameworkFactory;
using MeatCounterfeitDetector.Utils.IOService;
using MeatCounterfeitDetector.Utils.MainWindowControlChanger;
using MeatCounterfeitDetector.Utils.MessageBoxService;
using MeatCounterfeitDetector.Utils.UserService;
using Microsoft.Extensions.DependencyInjection;
using FrameworkElementFactory = MeatCounterfeitDetector.Utils.FrameworkFactory.FrameworkElementFactory;
using MeatCounterfeitDetector.Utils.AuthService;
using MeatCounterfeitDetector.UserInterface.Technologist.Analysis;
using MeatCounterfeitDetector.UserInterface.Technologist.Edit;
using MeatCounterfeitDetector.Utils.EventAggregator;
using ImageWorker.ProgressReporter;
using ImageWorker.BitmapService;
using ImageWorker.ImageEditing;
using ImageWorker.ImageAnalyzis;

namespace MeatCounterfeitDetector;

public partial class App : Application
{
    private IContainer Container { get; set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var baseUrl = "https://localhost:7140/";
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

        builder.RegisterType<CounterfeitClient>()
            .As<CounterfeitClient>()
            .WithParameter("baseUrl", baseUrl);

        builder.RegisterType<CounterfeitImageClient>()
            .As<CounterfeitImageClient>()
            .WithParameter("baseUrl", baseUrl);

        builder.RegisterType<ResultClient>()
            .As<ResultClient>()
            .WithParameter("baseUrl", baseUrl);

        builder.RegisterType<OriginalImageClient>()
            .As<OriginalImageClient>()
            .WithParameter("baseUrl", baseUrl);

        builder.RegisterType<ResultImageClient>()
            .As<ResultImageClient>()
            .WithParameter("baseUrl", baseUrl);

        builder.RegisterType<UserClient>()
            .As<UserClient>()
            .WithParameter("baseUrl", baseUrl);

        builder.RegisterType<UserRoleClient>()
               .As<UserRoleClient>()
               .WithParameter("baseUrl", baseUrl);

        builder.Register<IHttpClientFactory>(_ =>
        {
            var services = new ServiceCollection();
            services.AddHttpClient();
            var provider = services.BuildServiceProvider();
            return provider.GetRequiredService<IHttpClientFactory>();
        });

        builder.Register(c =>
            {
                var client = c.Resolve<IHttpClientFactory>().CreateClient();
                return new AuthClient(baseUrl, client);
            })
            .As<AuthClient>()
            .SingleInstance();

        builder.Register(c =>
            {
                var token = c.Resolve<IAuthService>().GetToken();
                var httpClient = c.Resolve<IHttpClientFactory>().CreateClient();
                if (token is not null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                return httpClient;
            })
            .As<HttpClient>();

        builder.RegisterType<MainWindow>().AsSelf().SingleInstance();
        builder.RegisterType<MainWindowVM>().AsSelf().SingleInstance();
        builder.RegisterType<FrameworkElementFactory>().As<IFrameworkElementFactory>();
        builder.RegisterType<NavigationManager>().AsSelf().SingleInstance();
        builder.RegisterType<UserControlFactory>().AsSelf();
        builder.RegisterType<DialogService>().AsSelf();
        builder.RegisterType<ImageAnalyzer>().As<IImageAnalyzer>();
        builder.RegisterType<ImageEditor>().As<IImageEditor>();
        builder.RegisterType<FileDialogService>().As<IFileDialogService>();
        builder.RegisterType<BitmapService>().As<IBitmapService>();
        builder.RegisterType<HandyMessageBoxService>().As<IMessageBoxService>();
        builder.RegisterType<UserService>().As<IUserService>().SingleInstance();
        builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
        builder.RegisterType<ProgressReporter>().As<IProgressReporter>().SingleInstance();

        builder.RegisterType<AuthService>().As<IAuthService>().SingleInstance();

        Container = builder.Build();

        VmLocator.Container = Container;
        VmLocator.Register<MainWindow, MainWindowVM>();
        VmLocator.Register<LoginControl, LoginControlVM>();
        VmLocator.Register<UserInterfaceSelectControl, UserInterfaceSelectControlVM>();

        VmLocator.Register<MainTechnologistControl, MainTechnologistControlVM>();
        VmLocator.Register<AnalysisControl, AnalysisControlVM>();
        VmLocator.Register<ImageEditingControl, ImageEditingControlVM>();

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

