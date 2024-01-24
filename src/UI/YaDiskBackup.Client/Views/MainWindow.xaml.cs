using System.Windows;
using YaDiskBackup.Domain.Properties;

namespace YaDiskBackup.Client.Views;

/// <summary>
/// Interaction logic for <see cref="MainWindow"/>.
/// </summary>
public partial class MainWindow : Window
{
    /// <inheritdoc />
    public MainWindow()
    {
        InitializeComponent();
    }

    //TODO переделать сохранение данных после того как они будут заполнены
    private void SaveData(object sender, System.EventArgs e) => ApplicationSettings.Default.Save();

   //private void Window_Closed(object sender, System.EventArgs e) 
}