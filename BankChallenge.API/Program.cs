using BankChallenge.API.Configurations;
using BankChallenge.Infrasctructure.Helpers;

var builder = WebApplication.CreateBuilder(args);

DotEnvLoader.Load();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddServices();
builder.Services.AddSwagger();
builder.Services.AddJwtAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();