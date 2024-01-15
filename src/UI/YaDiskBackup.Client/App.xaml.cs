using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Unity;
using System.Windows;
using Unity;
using Unity.Microsoft.DependencyInjection;
using YaDiskBackup.Client.Views;

namespace YaDiskBackup.Client;

/// <inheritdoc />
public partial class App
{
    /// <inheritdoc />
    protected override IContainerExtension CreateContainerExtension()
    {
        ServiceCollection serviceCollection = [];
        serviceCollection.AddAdvancedDependencyInjection();

        var container = new UnityContainer().AddExtension(new Diagnostic());
        container.BuildServiceProvider(serviceCollection);

        
        return new UnityContainerExtension(container);
    }

    /// <inheritdoc />
    protected override void RegisterTypes(IContainerRegistry containerRegistry) {}

    /// <inheritdoc />
    protected override Window CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }
}