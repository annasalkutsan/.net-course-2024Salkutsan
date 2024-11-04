using AutoMapper;
using BankSystem.App.Dto;
using BankSystem.App.Services;
using BankSystem.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ClientService _clientService;
        private readonly IMapper _mapper;

        public ClientController(ClientService clientService, IMapper mapper)
        {
            _clientService = clientService;
            _mapper = mapper;
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(Guid id)
        {
            var client = await _clientService.GetClientAsync(id);
            var clientDto = _mapper.Map<ClientResponseDto>(client);
            return Ok(clientDto);
        }
        
        [HttpPost("add")]
        public async Task<IActionResult> AddClient([FromBody] ClientRequestDto clientRequestDto)
        {
            var client = _mapper.Map<Client>(clientRequestDto);
            await _clientService.AddClientAsync(client);
            var clientDto = _mapper.Map<ClientResponseDto>(client);
            return Ok(clientDto);
        }
        
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateClient(Guid id, [FromBody] ClientRequestDto clientRequestDto)
        {
            var updatedClient = _mapper.Map<Client>(clientRequestDto);
            await _clientService.UpdateClientAsync(id, updatedClient);
            var clientDto = _mapper.Map<ClientResponseDto>(updatedClient);
            return Ok(clientDto);
        }
        
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            await _clientService.DeleteClientAsync(id);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetClientsByFilter(
            [FromQuery] string lastName = null,
            [FromQuery] string phoneNumber = null,
            [FromQuery] string passport = null,
            [FromQuery] DateTime? birthStart = null,
            [FromQuery] DateTime? birthEnd = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var clients = await _clientService.GetClientsByFilterAsync(lastName, phoneNumber, passport, birthStart, birthEnd, pageNumber, pageSize);
            var clientDtos = _mapper.Map<IEnumerable<ClientResponseDto>>(clients);
            
            return Ok(clientDtos);
        }
    }
}