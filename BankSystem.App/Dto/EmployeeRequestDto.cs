namespace BankSystem.App.Dto;

public class EmployeeRequestDto
{
    public string Contract { get; set; }
    public decimal Salary { get; set; }
    public Guid? PositionId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime BirthDay { get; set; }
}