namespace AdminDashboard.Contracts.Payments;

public record PaymentDto(Guid Id, decimal Amount, DateTime Date, string Description, Guid ClientId, string ClientEmail);