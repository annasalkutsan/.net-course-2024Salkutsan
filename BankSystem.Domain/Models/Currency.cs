public class Currency
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
    public DateTime CreateUtc { get; set; } = DateTime.UtcNow;
    
    public Currency(string code, string name)
    {
        Code = code;
        Name = name;
    }
    public Currency () {}

    public override bool Equals(object obj)
    {
        if (obj is Currency other)
        {
            return Code == other.Code;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Code);
    }
}