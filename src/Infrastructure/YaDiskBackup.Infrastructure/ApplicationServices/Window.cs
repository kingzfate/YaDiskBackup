using YaDiskBackup.Application.Interfaces;

namespace YaDiskBackup.Infrastructure.ApplicationServices;

/// <inheritdoc />
public class Window(ISettings settings) : IWindow
{
    /// <inheritdoc />
    private readonly ISettings Settings = settings;

    /// <inheritdoc />
    public void SelectSourcePath()
    {
        using FolderBrowserDialog folderBrowserDialog = new();
        if (folderBrowserDialog.ShowDialog() != DialogResult.OK) 
            return;

        Settings.AddPath(folderBrowserDialog.SelectedPath);
    }
}