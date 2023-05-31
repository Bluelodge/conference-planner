using BackEnd;
using BackEnd.Data;
using BackEnd.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Sqlite DB
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? "Data Source=conferences.db";

var services = builder.Services;

services.AddSqlite<ApplicationDbContext>(connectionString);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapSpeakerEndpoints();
app.MapSessionEndpoints();
app.MapAttendeeEndpoints();
app.MapSearchEndpoints();

app.Run();
