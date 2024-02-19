namespace YaDiskBackup.Application.Models;

/// <summary>
/// Копируемые файлы
/// </summary>
public class CopiedFile
{
    private readonly CopiedFile _file;

    public CopiedFile(CopiedFile file = null)
    {
        _file = file;
    }

    /// <summary>
    /// Время копирования
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// Название файла
    /// </summary>
    public string FileName { get; set; }
}