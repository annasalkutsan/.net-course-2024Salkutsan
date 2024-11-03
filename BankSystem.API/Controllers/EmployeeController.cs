using AutoMapper;
using BankSystem.App.Dto;
using BankSystem.App.Services;
using BankSystem.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController:ControllerBase
{
    private readonly EmployeeService _employeeService;
    private readonly IMapper _mapper;

    public EmployeeController(EmployeeService employeeService, IMapper mapper)
    {
        _employeeService = employeeService;
        _mapper = mapper;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeById(Guid id)
    {
        var employee = await _employeeService.GetEmployeeAsync(id);
        var employeeDto = _mapper.Map<EmployeeResponseDto>(employee);
        return Ok(employeeDto);
    }
    
    [HttpPost("add")]
    public async Task<IActionResult> AddEmployee([FromBody] EmployeeRequestDto employeeRequestDto)
    {
        var employee = _mapper.Map<Employee>(employeeRequestDto);
        await _employeeService.AddEmployeeAsync(employee);
        var employeeDto = _mapper.Map<EmployeeResponseDto>(employee);
        return Ok(employeeDto);
    }
        
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] EmployeeRequestDto employeeRequestDto)
    {
        var updatedEmployee = _mapper.Map<Employee>(employeeRequestDto);
        await _employeeService.UpdateEmployeeAsync(id, updatedEmployee);
        var employeeDto = _mapper.Map<EmployeeResponseDto>(updatedEmployee);
        return Ok(employeeDto);
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteEmployee(Guid id)
    {
        await _employeeService.DeleteEmployeeAsync(id);
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> GetEmployeesByFilter(
        [FromQuery] string lastName = null,
        [FromQuery] string phoneNumber = null,
        [FromQuery] Guid? positionId = null, 
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var employees = await _employeeService.GetEmployeesByFilterAsync(lastName, phoneNumber, positionId, pageNumber, pageSize); 
        var employeeDtos = _mapper.Map<IEnumerable<EmployeeResponseDto>>(employees);
        return Ok(employeeDtos);
    }
}