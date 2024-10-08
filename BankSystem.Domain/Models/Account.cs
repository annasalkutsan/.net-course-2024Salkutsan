public class Account
{
    public Currency Currency { get; set; }
    public decimal Amount { get; set; }

    public Account(Currency currency, decimal amount)
    {
        Currency = currency;
        Amount = amount;
    }

    public Account() { }

    public override bool Equals(object obj)
    {
        if (obj is Account other)
        {
            return Currency.Equals(other.Currency) && Amount == other.Amount;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Currency, Amount);
    }
}