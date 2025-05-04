using Basket.API.Data;
using BuildingBlocks.Behaviour;
using BuildingBlocks.Validation;
using Carter;
using Discount.GRPC;
using FluentValidation;
using HealthChecks.UI.Client;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.Hybrid;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddSwaggerGen();


var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    
    config.AddOpenBehavior(typeof(RequestLoggingBehaviour<,>));

    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
});

builder.Services.AddCarter();

builder.Services.AddValidatorsFromAssemblies([assembly]);

builder.Services.AddMarten(opts =>
{
    var connectionString = builder.Configuration.GetConnectionString("Database")!;

    opts.Connection(connectionString);    
    
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
// Add the caching decorator
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

// Add cache
builder.Services.AddStackExchangeRedisCache(option =>
{
    var connectionString = builder.Configuration.GetConnectionString("Redis")!;

    option.Configuration = connectionString;
});
        
builder.Services.AddHybridCache(option =>
{
    option.DefaultEntryOptions = new HybridCacheEntryOptions()
    {
        LocalCacheExpiration = TimeSpan.FromHours(2), // Set local cache expiration to 2 hours
        Expiration = TimeSpan.FromHours(1) // Set distributed cache expiration to 1 hour
    };
});

builder.Services.AddHealthChecks()
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!)
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);


//Grpc Services
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
});
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    app.UseSwagger();

    // To retrieve jwt token if user exists browser
    app.UseSwaggerUI(c =>
    {
        c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
    });

}

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Request logging middleware
app.UseMiddleware<RequestLogContextMiddleware>();

// Global exception handling to catch all unhandled exceptions
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
app.MapCarter();

app.Run();

