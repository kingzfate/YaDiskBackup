using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using YaDiskBackup.Domain.Abstractions;
using YaDiskBackup.Domain.Models;
using YaDiskBackup.Shared.ViewModels;

namespace YaDiskBackup.Client.ViewModels;

/// <summary>
/// View model for main view
/// </summary>
public class MainWindowViewModel : NavigationViewModelBase
{
    private readonly ReadOnlyObservableCollection<CopiedFile> list;
    public ReadOnlyObservableCollection<CopiedFile> List => list;

    [Reactive]
    public bool IsPaused { get; set; } = true;
    //private bool _paused;
    //public bool IsPaused
    //{
    //    get => _paused;
    //    set => SetAndRaise(ref _paused, value);
    //}
    [Reactive]
    public bool IsRunning { get; set; } = false;

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
        });

        DisableBackup = ReactiveCommand.Create(() =>
        {
            backup.Disable();

            IsPaused = true;
            IsRunning = false;
        });

        backup.Live.Connect()
            .ObserveOnDispatcher()
            .Bind(out list)
            .DisposeMany()
            .Subscribe();
    }
}