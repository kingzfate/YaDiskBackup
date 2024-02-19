using Autofac;
using YaDiskBackup.Application.BusinessServices;
using YaDiskBackup.Application.Interfaces;
using YaDiskBackup.Infrastructure.ApplicationServices;

namespace YaDiskBackup.Client.Configurations;

/// <summary>
/// Configure for services core application
/// </summary>
internal static class ConfigureCoreServices
{
    /// <summary>
    /// Registrate services to container
    /// </summary>
    /// <param name="builder">Container</param>
    /// <returns>Registrates services into container</returns>
    internal static ContainerBuilder AddCoreServices(this ContainerBuilder builder)
    {
        builder.RegisterType<Backup>().As<IBackup>();
        builder.RegisterType<Window>().As<IWindow>();
        builder.RegisterType<Settings>().As<ISettings>();

        return builder;
    }
}