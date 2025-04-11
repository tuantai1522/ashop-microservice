using Carter;
using Catalog.API.Data;
using Marten;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddCarter();

builder.Services.AddMarten(opts =>
{
    var connectionString = builder.Configuration.GetConnectionString("Database")!;
    
    opts.Connection(connectionString);    
    
    // Auto create schema, indexes, and documents
    opts.AutoCreateSchemaObjects = Weasel.Core.AutoCreate.All;
    
    // Auto create documents
    opts.CreateDatabasesForTenants(c =>
    {
        c.MaintenanceDatabase(connectionString);
        c.ForTenant()
            .CheckAgainstPgDatabase()
            .WithOwner("postgres");
    });
    
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

// Configure the HTTP request pipeline.
app.MapCarter();

app.Run();

