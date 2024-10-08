public struct Currency
{
    public string Code { get; set; }
    public string Name { get; set; }

    public Currency(string code, string name)
    {
        Code = code;
        Name = name;
    }

    public static bool operator ==(Currency left, Currency right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Currency left, Currency right)
    {
        return !(left == right);
    }

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