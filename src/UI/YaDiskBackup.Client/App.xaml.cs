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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Splat;
using System;
using System.Windows;
using YaDiskBackup.Client.ViewModels;
using YaDiskBackup.Client.Views;

namespace YaDiskBackup.Client;

/// <inheritdoc />
public partial class App : Application
{
    public App()
    {
        Bootstrapper.BuildIoC(); // Настраиваем IoC 
        _logger = Locator.Current.GetService<ILogger>();
    }

   
}


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