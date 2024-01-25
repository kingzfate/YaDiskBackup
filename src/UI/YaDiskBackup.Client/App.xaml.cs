//using System;
//using System.ComponentModel;
//using System.Windows;

//namespace YaDiskBackup.Client;

///// <inheritdoc />
//public partial class App : Application
//{
//    public App()
//    {
//        AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
//        ShutdownMode = ShutdownMode.OnLastWindowClose;
//    }

//    private void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
//    {
//        Console.WriteLine(args.LoadedAssembly);
//    }

//    /// <summary>Raises the <see cref="E:System.Windows.Application.Startup" /> event.</summary>
//    /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
//    protected override void OnStartup(StartupEventArgs e)
//    {
//        var container = new Container(x => x.AddRegistry<AppRegistry>());
//        var factory = container.GetInstance<WindowFactory>();
//        var window = factory.Create(true);
//        container.Configure(x => x.For<Dispatcher>().Add(window.Dispatcher));

//        //configure dependency resolver for RxUI / Splat
//        var resolver = new ReactiveUIDependencyResolver(container);
//        //resolver.Register(() => new LogEntryView(), typeof(IViewFor<LogEntryViewer>));
//        //Locator.Current = resolver;
//        //RxApp.SupportsRangeNotifications = false;
//        //run start up jobs
//        container.GetInstance<TradePriceUpdateJob>();
//        container.GetInstance<ILogEntryService>();

//        window.Show();
//        base.OnStartup(e);
//    }
//}
//-----------------------------------------------------------------------------------------------
//using Autofac;
//using Autofac.Core;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using ReactiveUI;
//using Splat;
//using Splat.Autofac;
//using System;
//using System.ComponentModel;
//using System.Reflection;
//using System.Windows;
//using YaDiskBackup.Client.ViewModels;
//using YaDiskBackup.Client.Views;
//using YaDiskBackup.Domain.Abstractions;
//using YaDiskBackup.Infrastructure.Services;

//namespace YaDiskBackup.Client;

///// <inheritdoc />
//public partial class App : Application
//{
//    public App()
//    {
//        Bootstrapper.BuildIoC(); // Настраиваем IoC 
//    }

//    protected override void OnStartup(StartupEventArgs e)
//    {
//        base.OnStartup(e);

//        //var container = new ContainerBuilder();
//        ////var viewModel = container.Resolve<MainViewModel>();
//        //var window = new MainWindow { DataContext = viewModel };

//        //window = new MainWindow
//        //{
//        //    //DataContext = new MainWindowViewModel().Instance
//        //};
//        //window.Show();
//    }
//}

//public static class Bootstrapper
//{
//    public static void BuildIoC()
//    {
//        /*
//         * Создаем контейнер Autofac.
//         * Регистрируем сервисы и представления
//         */
//        var builder = new ContainerBuilder();
//        RegisterServices(builder);
//        RegisterViews(builder);
//        // Регистрируем Autofac контейнер в Splat
//        //var autofacResolver = builder.UseAutofacDependencyResolver();
//        //builder.RegisterInstance(autofacResolver);

//        //// Вызываем InitializeReactiveUI(), чтобы переопределить дефолтный Service Locator
//        //autofacResolver.InitializeReactiveUI();
//        //var lifetimeScope = builder.Build();
//        //autofacResolver.SetLifetimeScope(lifetimeScope);
//        var autofacResolver = builder.UseAutofacDependencyResolver();

//        // Register the resolver in Autofac so it can be later resolved.
//        builder.RegisterInstance(autofacResolver);

//        // Initialize ReactiveUI components.
//        autofacResolver.InitializeReactiveUI();
//    }

//    private static void RegisterServices(ContainerBuilder builder)
//    {
//        //builder.RegisterModule(new ApcCoreModule());
//        //builder.RegisterType<AppLogger>().As<ILogger>();
//        // Регистрируем профили ObjectMapper путем сканирования сборки
//        //var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
//        //typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());
//    }

//    private static void RegisterViews(ContainerBuilder builder)
//    {
//        //Autofac.IContainer container = builder.Build();
//        //MainWindowViewModel mainViewModel = container.Resolve<MainWindowViewModel>();
//        builder.RegisterType<MainWindow>().As<IViewFor<MainWindowViewModel>>().SingleInstance();
//        //builder.RegisterType<MessageWindow>().As < IViewFor << MessageWindowViewModel >> ().AsSelf();
//        builder.RegisterType<MainWindowViewModel>().AsSelf().SingleInstance();
//        //builder.RegisterType<MainWindow>().SingleInstance();
//        //builder.RegisterType<IBackup>();
//        //builder.RegisterType<IWindow>();
//        builder.RegisterType<Backup>().As<IBackup>();
//        builder.RegisterType<Infrastructure.Services.Window>().As<IWindow>();

//        //builder.DataContext = new MainWindowViewModel();
//        //builder.RegisterType<MessageWindowViewModel>();
//    }
//    private static Autofac.IContainer? Container;

//    public static T Resolve<T>() where T : class
//    {
//        return Container.Resolve<T>();
//    }
//}
//-----------------------------------------------------------------------------------------------

//using Microsoft.Extensions.DependencyInjection;
//using Prism.Ioc;
//using Prism.Unity;
//using System.Windows;
//using Unity;
//using Unity.Microsoft.DependencyInjection;
//using YaDiskBackup.Client.Views;

//namespace YaDiskBackup.Client;

///// <inheritdoc />
//public partial class App
//{
//    /// <inheritdoc />
//    protected override IContainerExtension CreateContainerExtension()
//    {
//        ServiceCollection serviceCollection = [];
//        serviceCollection.AddAdvancedDependencyInjection();

//        IUnityContainer container = new UnityContainer().AddExtension(new Diagnostic());
//        container.BuildServiceProvider(serviceCollection);

//        return new UnityContainerExtension(container);
//    }

//    /// <inheritdoc />
//    protected override void RegisterTypes(IContainerRegistry containerRegistry) { }

//    /// <inheritdoc />
//    protected override Window CreateShell()
//    {
//        return Container.Resolve<MainWindow>();
//    }
//}
//--------------------------------------------------------------------------------------------------

//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using System.Windows;
//using YaDiskBackup.Client.ViewModels;
//using YaDiskBackup.Client.Views;
//using YaDiskBackup.Domain.Abstractions;
//using YaDiskBackup.Infrastructure.Services;

//namespace YaDiskBackup.Client;

//public partial class App : Application
//{
//    public static IHost? AppHost { get; private set; }

//    public App()
//    {
//        AppHost = Host.CreateDefaultBuilder()
//            .ConfigureServices((hostContext, services) =>
//            {
//                services.AddSingleton<MainWindow>();
//                services.AddScoped<MainWindowViewModel>();
//                services.AddSingleton<IBackup, Backup>();
//            })
//            .Build();
//    }

//    protected override async void OnStartup(StartupEventArgs e)
//    {
//        await AppHost!.StartAsync();

//        var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
//        startupForm.Show();

//        base.OnStartup(e);
//    }

//    protected override async void OnExit(ExitEventArgs e)
//    {
//        await AppHost!.StopAsync();
//        base.OnExit(e);
//    }
//}



using ReactiveUI;
using Splat;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using YaDiskBackup.Client.ViewModels;
using YaDiskBackup.Client.Views;
using YaDiskBackup.Domain.Abstractions;
using YaDiskBackup.Infrastructure.Services;

namespace YaDiskBackup.Client;

/// <inheritdoc />
public partial class App : System.Windows.Application
{
    public App()
    {
        Locator.CurrentMutable.Register(() => new MainWindow(), typeof(IViewFor<MainWindowViewModel>));
        //var a = 1 + 1;
        //Locator.CurrentMutable.RegisterConstant(new MainWindow(), typeof(MainWindowViewModel));
        Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
        Locator.CurrentMutable.RegisterConstant(new YaDiskBackup.Infrastructure.Services.Window(), typeof(IWindow));
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Create ShellViewModel and register as IScreen
        //var viewModel = new MainWindowViewModel(new YaDiskBackup.Infrastructure.Services.Window(), new Backup());
        //Locator.CurrentMutable.RegisterConstant(viewModel, typeof(IScreen));
        //// Resolve view for ShellViewModel
        //var view = ViewLocator.Current.ResolveView(viewModel);
        //view.ViewModel = viewModel;
        //// Run application
        //System.Windows.Forms.Application.Run((Form)view);
        var mainWindow = new MainWindow();
        mainWindow.Show();
    }
}