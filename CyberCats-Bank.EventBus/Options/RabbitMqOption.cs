namespace CyberCats_Bank.EventBus.Options
{
    public class RabbitMqOption
    {
        public string IPAddress { get; set; }
        public ushort PortNumber { get; set; } = 5672;
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}