using LibraryManagement.Infra.Context;
using LibraryManagement.Infra.Repositorios;
using LibraryManagement.Service.Interfaces;
using LibraryManagement.Service.Services;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Infra.Repositorios.Interfaces;
using LibraryManagement.Infra.Repositorios.Implementacoes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("SqlConnection");

builder.Services.AddDbContext<LibraryManagementContext>(options =>
            options.UseSqlServer(connectionString));

builder.Services.AddScoped<ILivrosRepositorio, LivrosRepositorio>();
builder.Services.AddScoped<IMembrosRepositorio, MembrosRepositorio>();
builder.Services.AddScoped<IEmprestimosRepositorio, EmprestimosRepositorio>();

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
