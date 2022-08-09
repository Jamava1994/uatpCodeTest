using RapidPay.Domain.Interfaces;
using RapidPay.Infrastructure.Services;
using UniversalFeesExchange.Sdk;
using System.Reflection;
using MediatR;
using FluentValidation;
using RapidPay.Infrastructure.Database;
using RapidPay.Application.Features.Card.Create;
using RapidPay.Application.Features.Card.Pay;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RapidPay.Application.Features.User.Authenticate;
using RapidPay.Application.Common.Services;
using RapidPay.Application.Features.User.Create;
using Microsoft.OpenApi.Models;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    var securityScheme = new OpenApiSecurityScheme()
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
    };

    var securityRequirement = new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer Authentication"
            }
        },
        new string[] {}
    }
};

    c.AddSecurityDefinition("Bearer Authentication", securityScheme);
    c.AddSecurityRequirement(securityRequirement);
});

builder.Services.AddEndpointsApiExplorer()
    .AddUFE()
    .AddDbContext<RapidPayDbContext>()
    .AddMediatR(Assembly.GetExecutingAssembly())
    .AddAutoMapper(Assembly.GetExecutingAssembly())
    .AddAuthentication(x =>
        {
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            var key = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = false,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
            };
        });

/* Validators */
/* FluentValidation no longer recommend using auto-validation for new projects for the reasons mentioned at the start of this page.
 * https://docs.fluentvalidation.net/en/latest/aspnet.html#automatic-validation */
builder.Services.AddScoped<IValidator<CreateCardCommand>, CreateCardCommandValidator>();
builder.Services.AddScoped<IValidator<MakePaymentCommand>, MakePaymentCommandValidator>();
builder.Services.AddScoped<IValidator<SignInCommand>, SignInCommandValidator>();
builder.Services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();

/* Add Infrastructure services */
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddSingleton<IUniversalFeesExchangeService, UniversalFeesExchangeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
