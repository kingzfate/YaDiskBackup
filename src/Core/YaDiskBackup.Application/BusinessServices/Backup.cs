using DynamicData;
using Egorozh.YandexDisk.Client.Http;
using Egorozh.YandexDisk.Client.Protocol;
using YaDiskBackup.Application.Interfaces;
using YaDiskBackup.Application.Models;
using YaDiskBackup.Application.Properties;

namespace YaDiskBackup.Application.BusinessServices;

/// <inheritdoc />
public class Backup : IBackup
{
    public SourceList<CopiedFile> Live { get; set; } = new SourceList<CopiedFile>();

    FileSystemWatcher watcher = new();

    /// <inheritdoc />
    public Backup()
    {
        //Live ??= new SourceList<CopiedFile>();
    }

    /// <inheritdoc />
    public void Enable()
    {
        watcher = new FileSystemWatcher(ApplicationSettings.Default.SourcePath)
        {
            IncludeSubdirectories = ApplicationSettings.Default.IsSearchSubdir,
            EnableRaisingEvents = true
        };

        watcher.NotifyFilter |= NotifyFilters.LastWrite;
        watcher.Created += new FileSystemEventHandler(OnCreated);
    }

    /// <inheritdoc />
    public void Disable()
    {
        watcher.EnableRaisingEvents = false;
        watcher.Dispose();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    private async void OnCreated(object source, FileSystemEventArgs e)
    {
        //Live ??= new SourceList<CopiedFile>();
        using (DiskHttpApi api = new(ApplicationSettings.Default.Token))
        {
            ResourceRequest request = new()
            {
                Path = "/"
            };
            CancellationToken cancellationToken = new();

            //if (!(await api.MetaInfo.GetInfoAsync(request, cancellationToken)).Embedded.Items.Any(item => item.Type == ResourceType.Dir && item.Name.Equals(ApplicationSettings.Default.DestinationFolder)))
            //{
            //    Link dictionaryAsync = await api.Commands.CreateDictionaryAsync("/" + ApplicationSettings.Default.DestinationFolder);
            //}

            Link uploadLinkAsync = await api.Files.GetUploadLinkAsync("/" + ApplicationSettings.Default.DestinationFolder + "/" + e.Name.Split('\\').Last(), true);

            while (IsFileLocked(new FileInfo(e.FullPath))) { }

            using (FileStream fs = File.OpenRead(e.FullPath))
                await api.Files.UploadAsync(uploadLinkAsync, fs);

            Live.Add(new CopiedFile
            {
                Time = DateTime.Now.ToLocalTime(),
                FileName = e.Name.Split('\\').Last()
            });
        }
    }

    private bool IsFileLocked(FileInfo file)
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