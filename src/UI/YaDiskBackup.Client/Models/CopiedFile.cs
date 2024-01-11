using System;

namespace YaDiskBackup.Client.Models
{
    /// <summary>
    /// Копируемые файлы
    /// </summary>
    internal class CopiedFile
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