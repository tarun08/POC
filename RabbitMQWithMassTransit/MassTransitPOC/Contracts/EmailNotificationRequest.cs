namespace MassTransitPOC.Contracts;

public record EmailNotificationRequest(int UserId, string EmailAddress);
