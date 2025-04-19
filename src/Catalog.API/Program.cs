using BuildingBlocks.Behaviour;
using BuildingBlocks.Validation;
using Carter;
using Catalog.API;
using Catalog.API.Data;
using FluentValidation;
using Marten;
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

if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<InitialCatalogData>();
}

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

// Request logging middleware
app.UseMiddleware<RequestLogContextMiddleware>();

// Global exception handling to catch all unhandled exceptions
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
app.MapCarter();

app.Run();

