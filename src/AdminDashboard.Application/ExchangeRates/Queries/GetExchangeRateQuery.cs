using AdminDashboard.Abstractions.ExchangeRates;
using AdminDashboard.Abstractions.ExchangeRates.Queries;
using AdminDashboard.Contracts.ExchangeRates;
using AdminDashboard.Domain.Exceptions;

namespace AdminDashboard.Application.ExchangeRates.Queries;

public class GetExchangeRateQuery : IGetExchangeRateQuery
{
    private readonly IExchangeRatesRepository _exchangeRateRepository;

    public GetExchangeRateQuery(IExchangeRatesRepository exchangeRateRepository)
    {
        _exchangeRateRepository = exchangeRateRepository;
    }

    public async Task<ExchangeRateDto> GetExchangeRateAsync(GetExchangeRateRequest request,
        CancellationToken cancellationToken = default)
    {
        var rate = await _exchangeRateRepository.GetCurrentRateAsync();

        if (rate is null)
        {
            throw new NotFoundException(nameof(ExchangeRateDto));
        }

        return new ExchangeRateDto(rate.Rate, rate.LastUpdated);
    }
}