using DynamicData;
using YaDiskBackup.Domain.Models;

namespace YaDiskBackup.Domain.Abstractions;

/// <summary>
/// Backup file to yandex disk
/// </summary>
public interface IBackup
{
    /// <summary>
    /// Enable file watcher
    /// </summary>
    void Enable();

    /// <summary>
    /// List of copied file to yandex disk
    /// </summary>
    SourceList<CopiedFile> Live { get; set; }

    /// <summary>
    /// Disable file watcher
    /// </summary>
    void Disable();
}