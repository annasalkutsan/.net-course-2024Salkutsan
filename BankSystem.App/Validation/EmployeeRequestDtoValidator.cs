using BankSystem.App.Dto;
using FluentValidation;

namespace BankSystem.App.Validation;

public class EmployeeRequestValidator : AbstractValidator<EmployeeRequestDto>
{
    public EmployeeRequestValidator()
    {
        RuleFor(e => e.Contract)
            .NotEmpty().WithMessage("Договор обязателен");

        RuleFor(e => e.Salary)
            .GreaterThan(0).WithMessage("Зарплата должна быть положительной");

        RuleFor(e => e.FirstName)
            .NotEmpty().WithMessage("Имя обязательно");

        RuleFor(e => e.LastName)
            .NotEmpty().WithMessage("Фамилия обязательна");

        RuleFor(e => e.PhoneNumber)
            .NotEmpty().WithMessage("Номер телефона не может быть пустым.")
            .Matches(@"^\+373\s\d{3}\s\d{2}\s\d{3}$")
            .WithMessage("Номер телефона должен быть в формате: +373 777 55 837.");

        RuleFor(e => e.BirthDay)
            .LessThan(DateTime.Today).WithMessage("Дата рождения должна быть в прошлом");
    }
}