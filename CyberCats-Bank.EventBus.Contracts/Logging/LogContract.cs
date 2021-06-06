namespace CyberCats_Bank.EventBus.Contracts.Logging
{
    public interface LogContract
    {
        /// <summary>
        /// Приложение
        /// </summary>
        public string Application { get; set; }
        /// <summary>
        /// Действие
        /// </summary>
        public string Action { get; set; }

    }
}