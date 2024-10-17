namespace BankSystem.Domain.Models;

public class Position
{
    public Guid Id { get; set; } 
    public string Title { get; set; } 
    public DateTime CreateUtc { get; set; } = DateTime.UtcNow;
    
    public ICollection<Employee> Employees { get; set; }= new List<Employee>();

    public Position(string title)
    {
        Title = title;
    }
    public Position() {}
}
