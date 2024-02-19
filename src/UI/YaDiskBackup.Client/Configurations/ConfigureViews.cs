using Autofac;
using YaDiskBackup.Client.ViewModels;
using YaDiskBackup.Client.Views;

namespace YaDiskBackup.Client.Configurations;

/// <summary>
/// Configure views
/// </summary>
internal static class ConfigureViews
{
    /// <summary>
    /// Registrate views to container
    /// </summary>
    /// <param name="builder">Container</param>
    /// <returns>Registrates views into container</returns>
    internal static ContainerBuilder AddViews(this ContainerBuilder builder)
    {
        builder.RegisterType<MainWindowViewModel>();
        builder.RegisterType<MainWindow>();

        return builder;
    }
}