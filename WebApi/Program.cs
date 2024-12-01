using Data.Models;
using Data.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//TODO get from appsettings.json
builder.Services.AddDbContext<OnlineLibraryContext>(options =>
    options.UseSqlServer("Server=localhost,1433;Database=rwa;User=sa;Password=password123!;Encrypt=False;TrustServerCertificate=False"));

builder.Services.AddScoped<IBookService, BookServices>();

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