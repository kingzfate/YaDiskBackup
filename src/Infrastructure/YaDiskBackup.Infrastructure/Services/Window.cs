using System.Windows.Forms;
using Scrutor.AspNetCore;
using YaDiskBackup.Domain.Abstractions;
using YaDiskBackup.Domain.Properties;

namespace YaDiskBackup.Infrastructure.Services;

public class Window : IWindow, ISingletonLifetime
{
    public void SelectSourcePath()
    {
        using FolderBrowserDialog folderBrowserDialog = new();
        if (folderBrowserDialog.ShowDialog() != DialogResult.OK) 
            return;

        SavePath(folderBrowserDialog.SelectedPath);
    }

    private void SavePath(string path)
    {
        ApplicationSettings.Default.SourcePath = path;
        ApplicationSettings.Default.Save();
    }
}