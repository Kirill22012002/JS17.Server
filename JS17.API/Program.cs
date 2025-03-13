using JS17.API.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=JS17.Db;Integrated Security=True;";
builder.Services.AddDbContext<WebDbContext>(x => x.UseSqlServer(connectString));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors(option =>
{
    option.AllowAnyOrigin();
    option.AllowAnyHeader();
    option.AllowAnyMethod();
});

app.UseCors(builder => builder.AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
