using DevExpress.Mvvm;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using YaDiskBackup.Properties;
using YandexDisk.Client.Http;
using YandexDisk.Client.Protocol;

namespace YaDiskBackup.ViewModels
{
    /// <summary>
    /// View model for main view
    /// </summary>
    internal class MainViewModel : ViewModelBase
    {
        public bool IsRunning { get; set; } = false;
        public bool IsPaused { get; set; } = true;

        FileSystemWatcher watcher = new FileSystemWatcher();

        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand OpenFolderCommand => new DelegateCommand(() =>
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    Settings.Default.SourcePath = fbd.SelectedPath;
                }
            }
        });

        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand EnableWatcherCommand => new DelegateCommand(() =>
        {
            watcher = new FileSystemWatcher(Settings.Default.SourcePath);
            watcher.IncludeSubdirectories = Settings.Default.IsSearchSubdir;
            watcher.NotifyFilter = watcher.NotifyFilter | NotifyFilters.LastWrite;
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
            using (DiskHttpApi api = new DiskHttpApi(Settings.Default.Token))
            {
                Resource roodFolderData = await api.MetaInfo.GetInfoAsync(new ResourceRequest
                {
                    Path = "/"
                });

                if (!roodFolderData.Embedded.Items.Any(item => item.Type == ResourceType.Dir &&
                                                               item.Name.Equals(Settings.Default.DestinationFolder)))
                {
                    await api.Commands.CreateDictionaryAsync($"/{Settings.Default.DestinationFolder}");
                }

                Link link = await api.Files.GetUploadLinkAsync($"/{Settings.Default.DestinationFolder}/{e.Name.Split('\\').Last()}", true);
                using (FileStream fs = File.OpenRead(e.FullPath))
                {
                    await api.Files.UploadAsync(link, fs);
                }
            }
        }
    }
}