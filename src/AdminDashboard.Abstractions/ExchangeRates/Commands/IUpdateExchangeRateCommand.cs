using AdminDashboard.Contracts.ExchangeRates;

namespace AdminDashboard.Abstractions.ExchangeRates.Commands;

public interface IUpdateExchangeRateCommand
{
    Task ExecuteAsync(UpdateExchangeRateRequest request, CancellationToken cancellationToken = default);
}