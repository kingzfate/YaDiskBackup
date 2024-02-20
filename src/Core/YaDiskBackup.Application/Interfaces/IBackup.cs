using DynamicData;
using YaDiskBackup.Application.Models;

namespace YaDiskBackup.Application.Interfaces;

/// <summary>
/// Backup file to yandex disk
/// </summary>
public interface IBackup
{
    /// <summary>
    /// List of copied file to yandex disk
    /// </summary>
    SourceList<CopiedFile> Live { get; set; }

    /// <summary>
    /// Enable file watcher
    /// </summary>
    void Enable();

    /// <summary>
    /// Disable file watcher
    /// </summary>
    void Disable();
}