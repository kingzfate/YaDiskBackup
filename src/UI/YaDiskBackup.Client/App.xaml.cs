using System.Windows;
using YaDiskBackup.Client.Configurations;

namespace YaDiskBackup.Client;

/// <summary>
/// Starting class for application
/// </summary>
public partial class App
{
    /// <summary>
    /// Run application
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Run(object sender, StartupEventArgs e) => Bootstrapper.BuildIoC();
}