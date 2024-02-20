namespace YaDiskBackup.Application.Interfaces;

/// <summary>
/// Services for settings application
/// </summary>
public interface ISettings
{
    /// <summary>
    /// Add new path to scan backup folder
    /// </summary>
    /// <param name="path">Path</param>
    void AddPath(string path);

    /// <summary>
    /// Save all settings for application
    /// </summary>
    void Save();
}