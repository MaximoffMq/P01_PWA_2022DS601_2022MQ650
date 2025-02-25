using Microsoft.EntityFrameworkCore;
using P01_DonisSanchez_MoralesQuezada.Modelos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//inyeccion por dependencia
builder.Services.AddDbContext<ParqueoDBcontext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ParqueoDbConnection")
    )
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
