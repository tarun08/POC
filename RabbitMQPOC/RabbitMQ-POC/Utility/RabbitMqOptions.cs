namespace RabbitMQPOC.Utility
{
    public class RabbitMqOptions
    {
        public string HostName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string ExchangeName { get; set; } = default!;
        public QueueOptions Queues { get; set; } = default!;
    }


    public class QueueOptions
    {
        public string Orders { get; set; } = default!;
        public string OrdersDLQ { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string EmailDLQ { get; set; } = default!;
        public string Payments { get; set; } = default!;
        public string PaymentsDLQ { get; set; } = default!;
    }
}
