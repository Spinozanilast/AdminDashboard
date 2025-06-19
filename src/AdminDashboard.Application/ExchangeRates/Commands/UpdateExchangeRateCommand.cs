using AdminDashboard.Abstractions;
using AdminDashboard.Abstractions.ExchangeRates;
using AdminDashboard.Abstractions.ExchangeRates.Commands;
using AdminDashboard.Contracts.ExchangeRates;
using AdminDashboard.Domain;

namespace AdminDashboard.Application.ExchangeRates.Commands;

public class UpdateExchangeRateCommand : IUpdateExchangeRateCommand
{
    private readonly IExchangeRatesRepository _exchangeRateRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateExchangeRateCommand(IExchangeRatesRepository exchangeRateRepository, IUnitOfWork unitOfWork)
    {
        _exchangeRateRepository = exchangeRateRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(UpdateExchangeRateRequest request, CancellationToken cancellationToken = default)
    {
        var currentRate = await _exchangeRateRepository.GetCurrentRateAsync();

        await _exchangeRateRepository.UpdateRateAsync(request.NewRate);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}