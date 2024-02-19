using Splat;
using System.Windows;
using YaDiskBackup.Application.Interfaces;
using YaDiskBackup.Client.ViewModels;

namespace YaDiskBackup.Client.Views;

/// <summary>
/// Interaction logic for <see cref="MainWindow"/>.
/// </summary>
public partial class MainWindow : Window
{
    private readonly ISettings Settings;

    /// <inheritdoc />
    public MainWindow(ISettings settings)
    {
        InitializeComponent();
        DataContext = Locator.Current.GetService<MainWindowViewModel>();
        Settings = settings;
    }

    /// <summary>
    /// Save all settings for application
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SaveData(object sender, RoutedEventArgs e) => Settings.Save();
}