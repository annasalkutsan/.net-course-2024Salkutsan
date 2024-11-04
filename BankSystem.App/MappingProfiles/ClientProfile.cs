using AutoMapper;
using BankSystem.App.Dto;
using BankSystem.Domain.Models;

namespace BankSystem.App.MappingProfiles;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<Client, ClientResponseDto>()
            .ForMember(dest => dest.AccountIds, opt => opt.MapFrom(src => src.Accounts.Select(a => a.Id)));
        CreateMap<ClientRequestDto, Client>();
    }
}