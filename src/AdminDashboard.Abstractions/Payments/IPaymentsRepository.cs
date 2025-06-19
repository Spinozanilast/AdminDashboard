using AdminDashboard.Abstractions.Common;
using AdminDashboard.Domain;

namespace AdminDashboard.Abstractions.Payments;

public interface IPaymentsRepository : IRepository<Payment, Guid>
{
    Task<IEnumerable<Payment>> GetLatestPaymentsAsync(int count = 5);
    Task<IEnumerable<Payment>> GetPaymentsByClientIdAsync(Guid clientId);
}