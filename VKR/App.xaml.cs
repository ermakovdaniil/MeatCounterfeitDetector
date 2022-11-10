using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using DataAccess.Models;
using DataAccess.Data;
using VKR.View;

namespace FlowModelDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.DefaultThreadCurrentUICulture;
            var builder = new ContainerBuilder();
            //builder.RegisterType<MainWindowVM>().AsSelf(); // TODO: LoginControlVM ИЛИ MainWindowVM?
            builder.RegisterType<CounterfeitKBContext>().AsSelf();
            builder.RegisterType<ResultDBContext>().AsSelf();
            builder.RegisterType<UserDBContext>().AsSelf();

            builder.RegisterType<IdentityFlowModelContext>().AsSelf();
            builder.RegisterType<EFMaterialRepository>().As<IRepository<Material>>();
            builder.RegisterType<EFMeasureRepository>().As<IRepository<Measure>>();
            builder.RegisterType<EFParameterRepository>().As<IRepository<Parameter>>();
            builder.RegisterType<EFParameterValueRepository>().As<IParameterValueRepository>();
            builder.RegisterType<EFTypeParameterRepository>().As<IRepository<TypeParameter>>();
            builder.RegisterType<EFUserRepository>().As<IUserRepository>();

            var container = builder.Build();
            var mainWindowVM = container.Resolve<MainWindowVM>();
            var mainWindow = new MainWindow { DataContext = mainWindowVM };
            mainWindow.Show();
        }

    }
}
