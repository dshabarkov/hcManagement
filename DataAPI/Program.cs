using DataAPI.Models;
using DataAPI.Services;
using DataAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//builder.Services.AddHttpClient<IAuthenticateService, AuthenticateService>(c =>
//        c.BaseAddress = new Uri("https://localhost:44305/"));

builder.Services.AddHttpClient<IAuthenticateService, AuthenticateService>(c =>
        c.BaseAddress = new Uri("http://humancapitalmanagement-loginapi-1:80/"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<HcmDataContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("HcmDataContext")));

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