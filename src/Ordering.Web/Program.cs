using Ordering.Infrastructure;
using Ordering.UseCases;
using Ordering.Web.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapOrdersApiV1();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();

    app.UseSwaggerUI();
}

app.Run();

