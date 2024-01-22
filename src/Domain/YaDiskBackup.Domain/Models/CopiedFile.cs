namespace YaDiskBackup.Domain.Models
{
    /// <summary>
    /// Копируемые файлы
    /// </summary>
    public class CopiedFile
    {
        /// <summary>
        /// Время копирования
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Название файла
        /// </summary>
        public string FileName { get; set; }
    }
}