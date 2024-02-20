namespace YaDiskBackup.Application.Interfaces;

/// <summary>
/// Work for system window
/// </summary>
public interface IWindow
{
    /// <summary>
    /// Select the folder where the backup files will be scanned 
    /// </summary>
    void SelectSourcePath();
}