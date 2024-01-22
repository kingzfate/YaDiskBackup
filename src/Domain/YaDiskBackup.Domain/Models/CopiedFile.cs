using System.Diagnostics;

namespace YaDiskBackup.Domain.Models
{
    /// <summary>
    /// Копируемые файлы
    /// </summary>
    public class CopiedFile
    {
        private readonly CopiedFile _file;

        public CopiedFile(CopiedFile file) 
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
}