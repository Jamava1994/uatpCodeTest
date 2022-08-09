using RapidPay.Domain.Interfaces;
using RapidPay.Infrastructure.Services;
using UniversalFeesExchange.Sdk;
using System.Reflection;
using MediatR;
using FluentValidation;
using RapidPay.Infrastructure.Database;
using RapidPay.Application.Features.Card.Create;
using RapidPay.Application.Features.Card.Pay;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddUFE();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<RapidPayDbContext>();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


builder.Services.AddScoped<IValidator<CreateCardCommand>, CreateCardCommandValidator>();
builder.Services.AddScoped<IValidator<MakePaymentCommand>, MakePaymentCommandValidator>();

/* Add Infrastructure services */
builder.Services.AddSingleton<IUniversalFeesExchangeService, UniversalFeesExchangeService>();

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
