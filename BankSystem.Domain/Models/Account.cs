using BankSystem.Domain.Models;

public class Account
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public Guid ClientId { get; set; }
    public Client Client { get; set; }
    public Guid CurrencyId { get; set; }
    public Currency Currency { get; set; }
    public DateTime CreateUtc { get; set; } = DateTime.UtcNow;
    
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
            return Currency.Equals(other.Currency);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Currency);
    }
}