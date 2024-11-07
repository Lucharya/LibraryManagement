using LibraryManagement.Infra.Context;
using LibraryManagement.Infra.Repositorios;
using LibraryManagement.Service.Interfaces;
using LibraryManagement.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<LibraryManagementContext>(options =>
            options.UseSqlServer(connectionString));

builder.Services.AddScoped<ILibraryManagementRepositorio, LibraryManagementRepositorio>();

builder.Services.AddScoped<ILivroService, LivroService>();
builder.Services.AddScoped<IMembroService, MembroService>();
builder.Services.AddScoped<IEmprestimoService, EmprestimoService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
