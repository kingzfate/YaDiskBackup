

using YaDiskBackup.Shared.ViewModels;

namespace YaDiskBackup.Client.ViewModels;

/// <summary>
/// View model for main view
/// </summary>
internal class MainWindowViewModel : NavigationViewModelBase
{
    /*public ObservableCollection<CopiedFile> CopiedFiles { get; set; } = new ObservableCollection<CopiedFile>();
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
    }*/
}