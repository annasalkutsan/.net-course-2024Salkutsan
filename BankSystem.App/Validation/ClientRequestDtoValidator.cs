using BankSystem.App.Dto;
using FluentValidation;

namespace BankSystem.App.Validation;

public class ClientRequestDtoValidator : AbstractValidator<ClientRequestDto>
{
    public ClientRequestDtoValidator()
    {
        RuleFor(client => client.FirstName)
            .NotEmpty().WithMessage("Имя не может быть пустым.")
            .Length(2, 50).WithMessage("Имя должно содержать от 2 до 50 символов.");

        RuleFor(client => client.LastName)
            .NotEmpty().WithMessage("Фамилия не может быть пустой.")
            .Length(2, 50).WithMessage("Фамилия должна содержать от 2 до 50 символов.");

        RuleFor(client => client.PhoneNumber)
            .NotEmpty().WithMessage("Номер телефона не может быть пустым.")
            .Matches(@"^\+373\s\d{3}\s\d{2}\s\d{3}$")
            .WithMessage("Номер телефона должен быть в формате: +373 777 55 837.");

        RuleFor(client => client.Passport)
            .NotEmpty().WithMessage("Паспорт не может быть пустым.")
            .Length(6, 12).WithMessage("Номер паспорта должен содержать от 6 до 12 символов.");
        
        RuleFor(e => e.BirthDay)
            .LessThan(DateTime.Today).WithMessage("Дата рождения должна быть в прошлом");
    }
}