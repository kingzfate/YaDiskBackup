using YaDiskBackup.Application.Interfaces;
using YaDiskBackup.Application.Properties;

namespace YaDiskBackup.Infrastructure.ApplicationServices;

/// <inheritdoc />
public class Settings : ISettings
{
    /// <inheritdoc />
    public void AddPath(string path)
    {
        ApplicationSettings.Default.SourcePath = path;
        Save();
    }

    /// <inheritdoc />
    public void Save() => ApplicationSettings.Default.Save();
}