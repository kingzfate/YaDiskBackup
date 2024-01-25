using ReactiveUI;
using System.Windows;
using YaDiskBackup.Client.ViewModels;
using YaDiskBackup.Domain.Properties;

namespace YaDiskBackup.Client.Views;

/// <summary>
/// Interaction logic for <see cref="MainWindow"/>.
/// </summary>
public partial class MainWindow : Window
{
    //#region ViewModel
    //public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(MainWindowViewModel), typeof(MainWindow), new PropertyMetadata(null));

    //public MainWindowViewModel ViewModel
    //{
    //    get { return (MainWindowViewModel)GetValue(ViewModelProperty); }
    //    set { SetValue(ViewModelProperty, value); }
    //}

    ////object IViewFor.ViewModel
    ////{
    ////    get { return ViewModel; }
    ////    set { ViewModel = (MainWindowViewModel)value; }
    ////}
    //#endregion ViewModel

    /// <inheritdoc />
    public MainWindow()
    {
        InitializeComponent();

    }

    //TODO переделать сохранение данных после того как они будут заполнены
    private void SaveData(object sender, System.EventArgs e) => ApplicationSettings.Default.Save();

   //private void Window_Closed(object sender, System.EventArgs e) 
}