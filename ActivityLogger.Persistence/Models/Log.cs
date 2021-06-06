using System;
using ActivityLogger.Persistence.Models.Enums;

namespace ActivityLogger.Persistence.Models
{
    /// <summary>
    /// Лог
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование приложение которое записывает логи
        /// </summary>
        public string Application { get; set; }

        public string Action { get; set; }
        /// <summary>
        /// Дата создания логов
        /// </summary>
        public DateTime DateCreated { get; set; }

        public LoggingType LoggingType { get; set; }
    }
}