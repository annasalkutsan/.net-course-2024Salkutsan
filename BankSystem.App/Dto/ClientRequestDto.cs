namespace BankSystem.App.Dto;

public class ClientRequestDto
{
    public string Passport { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime BirthDay { get; set; }
}