using Expensify.Context;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName: "expensify", serviceVersion: "0.0.1"))
    .WithTracing(tracing => tracing
    .AddSource("expensify")
    .AddAspNetCoreInstrumentation()
    .AddConsoleExporter());

builder.Logging.AddOpenTelemetry(options =>
{
    options
    .SetResourceBuilder(
    ResourceBuilder.CreateDefault()
    .AddService(serviceName: "expensify", serviceVersion: "0.0.1"))
    .AddConsoleExporter();
});

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
