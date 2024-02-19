using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using YaDiskBackup.Application.Interfaces;
using YaDiskBackup.Application.Models;

namespace YaDiskBackup.Client.ViewModels;

/// <summary>
/// View model for main view
/// </summary>
internal class MainWindowViewModel : ReactiveObject
{
    #region Private
    private readonly ReadOnlyObservableCollection<CopiedFile> list;
    #endregion

    /// <summary>
    /// List of copied files
    /// </summary>
    public ReadOnlyObservableCollection<CopiedFile> List => list;

    /// <summary>
    /// When backup is disabled
    /// </summary>
    [Reactive]
    public bool IsPaused { get; set; } = true;

    /// <summary>
    /// When backup is running
    /// </summary>
    [Reactive]
    public bool IsRunning { get; private set; } = false;

    #region Commands
    /// <summary>
    /// Open a window to select a folder
    /// </summary>
    public ICommand Browse { get; }

    /// <summary>
    /// Enable backup and scan the folder for new files
    /// </summary>
    public ICommand EnableBackup { get; }

    /// <summary>
    /// Disable backup and scan the folder for new files
    /// </summary>
    public ICommand DisableBackup { get; }
    #endregion

    /// <inheritdoc />
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