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
    /// <summary>
    /// List of copied file to yandex disk
    /// </summary>
    public SourceList<CopiedFile> Live { get; set; } = new SourceList<CopiedFile>();

    /// <summary>
    /// File watcher
    /// </summary>
    FileSystemWatcher watcher = new();

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
    /// The method starts as soon as it is clear that a new file has appeared in the folder
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    private async void OnCreated(object source, FileSystemEventArgs e)
    {
        string fileName = e.Name.Split('\\').Last();

        await SendFile(fileName, e.FullPath);
        SaveInformationAboutFile(fileName);
    }

    /// <summary>
    /// Send the received file to yandex.disk
    /// </summary>
    /// <param name="fileName"></param>
    private async Task SendFile(string fileName, string filePath)
    {
        using DiskHttpApi api = new(ApplicationSettings.Default.Token);

        ResourceRequest request = new()
        {
            Path = "/"
        };
        
        Link uploadLinkAsync = await api.Files.GetUploadLinkAsync("/" + ApplicationSettings.Default.DestinationFolder + "/" + fileName, true);

        while (IsFileLocked(new FileInfo(filePath))) { }

        using FileStream fs = File.OpenRead(filePath);
        await api.Files.UploadAsync(uploadLinkAsync, fs);
    }

    /// <summary>
    /// Save the file information in the application table
    /// </summary>
    /// <param name="fileName">File name</param>
    private void SaveInformationAboutFile(string fileName)
    {
        Live.Add(new CopiedFile
        {
            Time = DateTime.Now.ToLocalTime(),
            FileName = fileName
        });
    }

    /// <summary>
    /// We check if the file is blocked in order to continue working with it
    /// </summary>
    /// <param name="file">Info about current file</param>
    /// <returns>Locked or not</returns>
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