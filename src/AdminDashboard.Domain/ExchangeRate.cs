namespace AdminDashboard.Domain;

public class ExchangeRate
{
    public decimal Rate { get; private set; }
    public DateTime LastUpdated { get; private set; }

    private ExchangeRate() {}

    public ExchangeRate(decimal rate)
    {
        UpdateRate(rate);
    }

    public void UpdateRate(decimal newRate)
    {
        if (newRate <= 0)
            throw new ArgumentException("Rate must be positive");

        Rate = newRate;
        LastUpdated = DateTime.UtcNow;
    }
}