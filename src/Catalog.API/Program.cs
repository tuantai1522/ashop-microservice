using BuildingBlocks.Behaviour;
using BuildingBlocks.Validation;
using Carter;
using Catalog.API.Data;
using FluentValidation;
using Marten;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
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

// Global exception handling to catch all unhandled exceptions
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
app.MapCarter();

app.Run();

