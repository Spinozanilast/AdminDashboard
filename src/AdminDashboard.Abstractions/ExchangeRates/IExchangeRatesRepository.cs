using AdminDashboard.Abstractions.Common;
using AdminDashboard.Domain;

namespace AdminDashboard.Abstractions.ExchangeRates;

public interface IExchangeRatesRepository : IRepository<ExchangeRate, Guid>
{
    Task<ExchangeRate?> GetCurrentRateAsync();
    Task UpdateRateAsync(decimal newRate);
}