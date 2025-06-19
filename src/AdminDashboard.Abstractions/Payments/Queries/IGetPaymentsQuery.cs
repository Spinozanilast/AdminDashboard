using AdminDashboard.Contracts.Payments;

namespace AdminDashboard.Abstractions.Payments.Queries;

public interface IGetPaymentsQuery
{
    Task<List<PaymentDto>> GetPaymentsAsync(GetPaymentsRequest request,  CancellationToken cancellationToken = default);
}