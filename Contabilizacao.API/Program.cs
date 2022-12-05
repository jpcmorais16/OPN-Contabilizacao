using Contabilizacao.Data;
using Contabilizacao.Data.GoogleSheets;
using Contabilizacao.Data.Interfaces;
using Contabilizacao.Domain.Interfaces;
using Contabilizacao.Services;
using Contabilizacao.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<ContabilizacaoContext>();
builder.Services.AddScoped<GoogleSheetsConnection>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
//builder.Services.AddSingleton<IDataFetcher, SpreadsheetDataFetcher>();
//builder.Services.AddSingleton<IDataCommiter, SpreadsheetDataCommiter>();
//builder.Services.AddSingleton<IProductObjectFactory, ProductObjectFactory>();
builder.Services.AddScoped<IProductAddingService, ProductAddingService>();

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
