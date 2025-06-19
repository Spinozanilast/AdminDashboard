using AdminDashboard.Contracts.ExchangeRates;

namespace AdminDashboard.Abstractions.ExchangeRates.Queries;

public interface IGetExchangeRateQuery
{
    Task<ExchangeRateDto> GetExchangeRateAsync(GetExchangeRateRequest request, CancellationToken cancellationToken = default);
}