using DynamicData;
using Scrutor.AspNetCore;
using System.IO;
using System.Reactive.Disposables;
using YaDiskBackup.Domain.Abstractions;
using YaDiskBackup.Domain.Models;
using YaDiskBackup.Domain.Properties;

namespace YaDiskBackup.Infrastructure.Services;

public class Backup : IBackup, ISingletonLifetime
{
    public IObservableCache<CopiedFile, long> Live { get; }

    FileSystemWatcher watcher = new();

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

    public void Disable()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    private async void OnCreated(object source, FileSystemEventArgs e)
    {
        //if (CopiedFiles.Value == null)
        //{
        //    CopiedFiles.Value = new List<CopiedFile>();
        //}
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
        //    CopiedFiles.Value.Add(new CopiedFile
        //    {
        //        Time = DateTime.Now.ToLocalTime(),
        //        FileName = (e.Name.Split('\\')).Last()
        //    });
        //});
       // Live = new Source
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