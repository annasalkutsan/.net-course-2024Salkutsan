using AutoMapper;
using BankSystem.App.Dto;
using BankSystem.Domain.Models;

namespace BankSystem.App.MappingProfiles;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeResponseDto>();
        CreateMap<EmployeeRequestDto, Employee>();
    }
}