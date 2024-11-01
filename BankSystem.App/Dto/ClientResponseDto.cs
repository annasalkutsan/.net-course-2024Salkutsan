namespace BankSystem.App.Dto;

public class ClientResponseDto
{
    public Guid Id { get; set; }
    public string Passport { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime BirthDay { get; set; }
    public List<Guid> AccountIds { get; set; } = new List<Guid>();
}