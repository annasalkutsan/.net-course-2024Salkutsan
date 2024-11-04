namespace BankSystem.App.Dto;

public class EmployeeResponseDto
{
    public Guid Id { get; set; }
    public string Contract { get; set; }
    public decimal Salary { get; set; }
    public Guid? PositionId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime BirthDay { get; set; }
}