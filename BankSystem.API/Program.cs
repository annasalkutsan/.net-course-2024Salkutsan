using BankSystem.App.Interfaces;
using BankSystem.App.MappingProfiles;
using BankSystem.App.Services;
using BankSystem.App.Validation;
using BankSystem.Data.EntityConfigurations;
using BankSystem.Data.Storages;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DataBase");
builder.Services.AddDbContext<BankSystemDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IClientStorage, ClientStorage>();
builder.Services.AddScoped<ClientService>();

builder.Services.AddAutoMapper(typeof(ClientProfile));

builder.Services.AddValidatorsFromAssemblyContaining<ClientRequestDtoValidator>();

builder.Services.AddScoped<IEmployeeStorage, EmployeeStorage>();
builder.Services.AddScoped<EmployeeService>();

builder.Services.AddAutoMapper(typeof(EmployeeProfile));

builder.Services.AddValidatorsFromAssemblyContaining<EmployeeRequestValidator>();

builder.Services.AddHttpClient<CurrencyService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();