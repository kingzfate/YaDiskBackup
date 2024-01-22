using DynamicData;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Documents;
using YaDiskBackup.Domain.Abstractions;
using YaDiskBackup.Domain.Models;
using YaDiskBackup.Domain.Properties;
using YaDiskBackup.Shared.ViewModels;

namespace YaDiskBackup.Client.ViewModels;

/// <summary>
/// View model for main view
/// </summary>
public class MainWindowViewModel : NavigationViewModelBase
{
    public ReactiveProperty<IList<CopiedFile>> CopiedFiles { get; } = new();
    public ReadOnlyObservableCollection<CopiedFile> list;

    public AsyncReactiveCommand Browse { get; } = new();
    public AsyncReactiveCommand EnableBackup { get; } = new();
    public AsyncReactiveCommand DisableBackup { get; } = new();

    /// <inheritdoc />
    public MainWindowViewModel(
        IWindow window,
        IBackup backup)
    {
        Browse.Subscribe(async _ =>
        {
            window.SelectSourcePath();
        });

        EnableBackup.Subscribe(async _ =>
        {
            backup.Enable();

            backup.Live.Connect()
                 .Transform(file => new CopiedFile(file))
                 .ObserveOnDispatcher()
                 .Bind(out list)
                 .DisposeMany()
                 .Subscribe();
        });

        


        //EnableBackup.Subscribe(async _ =>
        //{
        //    FileSystemWatcher watcher = new();
        //    watcher = new FileSystemWatcher(ApplicationSettings.Default.SourcePath)
        //    {
        //        IncludeSubdirectories = ApplicationSettings.Default.IsSearchSubdir,
        //        EnableRaisingEvents = true
        //    };
        //    watcher.NotifyFilter |= NotifyFilters.LastWrite;
        //    watcher.Created += new FileSystemEventHandler(OnCreated);
        //});
    }


    //TODO не работает
    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    private async void OnCreated(object source, FileSystemEventArgs e)
    {
        if(CopiedFiles.Value == null)
        {
            CopiedFiles.Value = new List<CopiedFile>();
        }
        //using DiskHttpApi api = new(ApplicationSettings.Default.Token);

        //ResourceRequest request = new()
        //{
        //    Path = "/"
        //};
        //CancellationToken cancellationToken = new();
        //if (!(await api.MetaInfo.GetInfoAsync(request, cancellationToken)).Embedded.Items.Any(item => item.Type == ResourceType.Dir && item.Name.Equals(ApplicationSettings.Default.DestinationFolder)))
        //{
        //    Link dictionaryAsync = await api.Commands.CreateDictionaryAsync("/" + ApplicationSettings.Default.DestinationFolder);
        //}
        //Link uploadLinkAsync = await api.Files.GetUploadLinkAsync("/" + ApplicationSettings.Default.DestinationFolder + "/" + e.Name.Split('\\').Last(), true);
        //do
        //{ }
        //while (IsFileLocked(new FileInfo(e.FullPath)));

        //using (FileStream fs = File.OpenRead(e.FullPath))
        //    await api.Files.UploadAsync(uploadLinkAsync, fs);

        

        //System.Windows.Application.Current.Dispatcher.Invoke(() =>
        //{
            //CopiedFiles.Value.Add(new CopiedFile
            //{
            //    Time = DateTime.Now.ToLocalTime(),
            //    FileName = (e.Name.Split('\\')).Last()
            //});
       // });
    }

    protected bool IsFileLocked(FileInfo file)
    {
        try
        {
            using FileStream fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            fileStream.Close();
        }
        catch
        {
            return true;
        }
        return false;
    }

}
    
    
    // public DelegateCommand OpenFolderCommand => new(() =>
    // {
    //     using FolderBrowserDialog folderBrowserDialog = new();
    //     if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;
    //
    //     Settings.Default.SourcePath = folderBrowserDialog.SelectedPath;
    // });
    
    /*
     public ObservableCollection<CopiedFile> CopiedFiles { get; set; } = new ObservableCollection<CopiedFile>();
    public bool IsPaused { get; set; } = true;
    public bool IsRunning { get; set; } = false;

    FileSystemWatcher watcher = new FileSystemWatcher();

    /// <summary>
    ///
    /// </summary>
    public DelegateCommand OpenFolderCommand => new(() =>
    {
        using FolderBrowserDialog folderBrowserDialog = new();
        if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;

        Settings.Default.SourcePath = folderBrowserDialog.SelectedPath;
    });

    /// <summary>
    ///
    /// </summary>
    public DelegateCommand EnableWatcherCommand => new DelegateCommand(() =>
    {
        watcher = new FileSystemWatcher(Settings.Default.SourcePath)
        {
            IncludeSubdirectories = Settings.Default.IsSearchSubdir
        };
        watcher.NotifyFilter |= NotifyFilters.LastWrite;
        watcher.Created += new FileSystemEventHandler(OnCreated);
        watcher.EnableRaisingEvents = true;

        IsPaused = false;
        IsRunning = true;
    });

    public DelegateCommand DisableWatcherCommand => new DelegateCommand(() =>
    {
        watcher.EnableRaisingEvents = false;
        watcher.Dispose();

        IsPaused = true;
        IsRunning = false;
    });

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    private async void OnCreated(object source, FileSystemEventArgs e)
    {
        using DiskHttpApi api = new(Settings.Default.Token);

        ResourceRequest request = new()
        {
            Path = "/"
        };
        CancellationToken cancellationToken = new CancellationToken();
        if (!(await api.MetaInfo.GetInfoAsync(request, cancellationToken)).Embedded.Items.Any(item => item.Type == ResourceType.Dir && item.Name.Equals(Settings.Default.DestinationFolder)))
        {
            Link dictionaryAsync = await api.Commands.CreateDictionaryAsync("/" + Settings.Default.DestinationFolder);
        }
        Link uploadLinkAsync = await api.Files.GetUploadLinkAsync("/" + Settings.Default.DestinationFolder + "/" + e.Name.Split('\\').Last(), true);
        do
        { }
        while (IsFileLocked(new FileInfo(e.FullPath)));

        using (FileStream fs = File.OpenRead(e.FullPath))
            await api.Files.UploadAsync(uploadLinkAsync, fs);

        System.Windows.Application.Current.Dispatcher.Invoke(() =>
             CopiedFiles.Add(new CopiedFile
             {
                 Time = DateTime.Now.ToLocalTime(),
                 FileName = (e.Name.Split('\\')).Last()
             })
        );
    }

    protected virtual bool IsFileLocked(FileInfo file)
    {
        try
        {
            using FileStream fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            fileStream.Close();
        }
        catch (IOException ex)
        {
            return true;
        }
        return false;
    }
}
    */