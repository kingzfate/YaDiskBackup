using ReactiveUI;
using Splat;
using System.Reactive.Disposables;
using System.Windows;
using YaDiskBackup.Client.ViewModels;
using YaDiskBackup.Domain.Properties;

namespace YaDiskBackup.Client.Views;

/// <summary>
/// Interaction logic for <see cref="MainWindow"/>.
/// </summary>
public partial class MainWindow : Window, IViewFor<MainWindowViewModel>
{
    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(MainWindowViewModel), typeof(MainWindow), new PropertyMetadata(null));

    public MainWindowViewModel ViewModel
    {
        get { return (MainWindowViewModel)GetValue(ViewModelProperty); }
        set { SetValue(ViewModelProperty, value); }
    }

    object IViewFor.ViewModel
    {
        get { return ViewModel; }
        set { ViewModel = (MainWindowViewModel)value; }
    }


    /// <inheritdoc />
    public MainWindow()
    {
        InitializeComponent();

        MainWindowViewModel ViewModel = Locator.Current.GetService<MainWindowViewModel>();
        DataContext = ViewModel;

        /*
         * Данный метод регистрирует привязки модели к элементам представления
         * DisposeWith в необходим для очистки привязок при удалении представления
         */
        //this.WhenActivated(disposable =>
        //{
        //    /*
        //     * Привязка свойства Text элемента TextBox к свойства модели.
        //     * OneWayBind - однонаправленная привязка, Bind - двунаправленная
        //     */
        //    this.BindCommand(ViewModel, vm => vm.Browse, v => v.Browse, nameof(Browse.Click))
        //        .DisposeWith(disposable);
        //    //this.OneWayBind(ViewModel, vm => vm.Browse, v => v.Browse)
        //    //    .DisposeWith(disposable);

        //    // Двунаправленная привязка значения позиции клапана. Конверторы значений свойства в модели и в представлении: FloatToStringConverter, StringToFloatConverter
        //    //this.Bind(ViewModel, vm => vm.Position, v => v.Position.Text, FloatToStringConverter, StringToFloatConverter)
        //    //    .DisposeWith(disposable);
        //    //this.OneWayBind(ViewModel, vm => vm.Current, v => v.Current.Text)
        //    //    .DisposeWith(disposable);
        //    //this.OneWayBind(ViewModel, vm => vm.ConnectedDevice.DeviceTime, v => v.DeviceDate.SelectedDate, val => val.Date)
        //    //    .DisposeWith(disposable);
        //    //this.OneWayBind(ViewModel, vm => vm.ConnectedDevice.DeviceTime, v => v.DeviceTime.SelectedTime, val => val.DateTime)
        //    //    .DisposeWith(disposable);

        //    ///* Привязка команд к кнопкам */
        //    //this.BindCommand(ViewModel, vm => vm.ConnectToDevice, v => v.ConnectDevice, nameof(ConnectDevice.Click))
        //    //    .DisposeWith(disposable);
        //    //this.BindCommand(ViewModel, vm => vm.SetValvePosition, v => v.SetValvePosition, vm => vm.ConnectedDevice.AssignedPosition, nameof(SetValvePosition.Click))
        //    //    .DisposeWith(disposable);
        //});
    }

    //TODO переделать сохранение данных после того как они будут заполнены
    private void SaveData(object sender, System.EventArgs e) => ApplicationSettings.Default.Save();
}