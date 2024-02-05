using Autofac;
using ReactiveUI;
using Splat;
using Splat.Autofac;
using System.Reflection;
using System.Windows;
using YaDiskBackup.Client.ViewModels;
using YaDiskBackup.Client.Views;
using YaDiskBackup.Domain.Abstractions;
using YaDiskBackup.Infrastructure.Services;

namespace YaDiskBackup.Client;

public partial class App : Application
{
    public App()
    {
        Bootstrapper.BuildIoC(); // Настраиваем IoC 
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
    }
}

public static class Bootstrapper
{
    public static void BuildIoC()
    {
        /*
         * Создаем контейнер Autofac.
         * Регистрируем сервисы и представления
         */
        var builder = new ContainerBuilder();
        RegisterServices(builder);
        RegisterViews(builder);
        // Регистрируем Autofac контейнер в Splat
        var autofacResolver = builder.UseAutofacDependencyResolver();
        builder.RegisterInstance(autofacResolver);

        // Вызываем InitializeReactiveUI(), чтобы переопределить дефолтный Service Locator
        autofacResolver.InitializeReactiveUI();
        var lifetimeScope = builder.Build();
        autofacResolver.SetLifetimeScope(lifetimeScope);
    }

    private static void RegisterServices(ContainerBuilder builder)
    {
        //builder.RegisterModule(new ApcCoreModule());
        //builder.RegisterType<AppLogger>().As<ILogger>();
        // Регистрируем профили ObjectMapper путем сканирования сборки
        //var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        //typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());
    }

    private static void RegisterViews(ContainerBuilder builder)
    {
        builder.RegisterType<MainWindow>().As<IViewFor<MainWindowViewModel>>();
        
        builder.RegisterType<MainWindowViewModel>();
       
        builder.RegisterType<Backup>().As<IBackup>();
        builder.RegisterType<Infrastructure.Services.Window>().As<IWindow>();
    }
}