using System.Windows;
using YaDiskBackup.ViewModels;

namespace YaDiskBackup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Window_Closed(object sender, System.EventArgs e) => Properties.Settings.Default.Save();
    }
}