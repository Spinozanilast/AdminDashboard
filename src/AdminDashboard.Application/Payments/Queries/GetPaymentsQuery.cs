using AdminDashboard.Abstractions.Payments;
using AdminDashboard.Abstractions.Payments.Queries;
using AdminDashboard.Contracts.Payments;

namespace AdminDashboard.Application.Payments.Queries;

public class GetPaymentsQuery : IGetPaymentsQuery
{
    private readonly IPaymentsRepository _paymentsRepository;

    public GetPaymentsQuery(IPaymentsRepository paymentsRepository)
    {
        _paymentsRepository = paymentsRepository;
    }

    public async Task<List<PaymentDto>> GetPaymentsAsync(GetPaymentsRequest request,
        CancellationToken cancellationToken = default)
    {
        var payments = await _paymentsRepository.GetLatestPaymentsAsync(request.Take);

        return payments.Select(p => new PaymentDto
        (
            p.Id,
            p.Amount,
            p.Date,
            p.Description,
            p.Client.Id,
            p.Client.Email
        )).ToList();
    }
}