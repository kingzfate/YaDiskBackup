using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using YaDiskBackup.Domain.Abstractions;
using YaDiskBackup.Domain.Models;
using YaDiskBackup.Shared.ViewModels;

namespace YaDiskBackup.Client.ViewModels;

/// <summary>
/// View model for main view
/// </summary>
public class MainWindowViewModel : ReactiveObject
{
    private readonly ReadOnlyObservableCollection<CopiedFile> list;
    public ReadOnlyObservableCollection<CopiedFile> List => list;

    //private ObservableAsPropertyHelper<bool> greeting;
    //public bool Greeting => greeting.Value;

    //public bool _stopState { get; set; }
    //public bool _startState { get; set; }

    //public bool StopState { get => _stopState; set => this.RaiseAndSetIfChanged(ref _stopState, value); }
    //public bool StartState { get => _startState; set => this.RaiseAndSetIfChanged(ref _startState, value); }

    //private ObservableAsPropertyHelper<bool> _isPaused;
    //public bool IsPaused => _isPaused.Value;

    

    [Reactive]
    public string Test { get; set; }
    //private bool _paused;
    //public bool IsPaused
    //{
    //    get => _paused;
    //    set => SetAndRaise(ref _paused, value);
    //}

    [Reactive]
    public bool IsPaused { get; set; } = true;
    [Reactive]
    public bool IsRunning { get; private set; } = false;

    public ICommand Browse { get; }
    public ICommand EnableBackup { get; }
    public ICommand DisableBackup { get; }

    /// ctor
    public MainWindowViewModel(
        IWindow window,
        IBackup backup)
    {

        Browse = ReactiveCommand.Create(
             window.SelectSourcePath
        );
        EnableBackup = ReactiveCommand.Create(() =>
        {
            backup.Enable();

            IsPaused = false;
            IsRunning = true;

            //Test = "PREDEV";

            //this.WhenAnyValue(x => x.IsRunning)                
            //    .ToPropertyEx(this, x => x.);
        });

        //this.WhenAnyObservable(x => x.EnableBackup).Subscribe(x => x.);


        DisableBackup = ReactiveCommand.Create(() =>
        {
            backup.Disable();

            IsPaused = true;
            IsRunning = false;
        });

        //this.WhenAnyValue(x => x.IsPaused, x => x.IsRunning);

        //this.OneWayBind(ViewModel, vm => vm.BlServers, v => v.cmbServers.Visibility).DisposeWith(disposables);

        backup.Live.Connect()
            .ObserveOnDispatcher()
            .Bind(out list)
            .DisposeMany()
            .Subscribe();
    }
}