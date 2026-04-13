namespace MassTransitPOC.Contracts;

public record OrderCreated(int OrderId, string ItemName, DateTime Timestamp);
