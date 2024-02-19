using Autofac;
using ReactiveUI;
using Splat.Autofac;
using YaDiskBackup.Client.Views;

namespace YaDiskBackup.Client.Configurations;

/// <summary>
/// The application bootstrapper and its configuration
/// </summary>
internal static class Bootstrapper
{
    /// <summary>
    /// Build container and use autofac resolver
    /// </summary>
    internal static void BuildIoC()
    {
        ContainerBuilder builder = new();

        builder.AddCoreServices();
        builder.AddViews();

        AutofacDependencyResolver autofacResolver = builder.UseAutofacDependencyResolver();
        builder.RegisterInstance(autofacResolver);

        autofacResolver.InitializeReactiveUI();
        IContainer container = builder.Build();
        autofacResolver.SetLifetimeScope(container);

        ShowWindow(container);
    }

    /// <summary>
    /// Show main application window with registrated services
    /// </summary>
    /// <param name="container">Application container</param>
    private static void ShowWindow(IContainer container) => container.Resolve<MainWindow>().Show();
}