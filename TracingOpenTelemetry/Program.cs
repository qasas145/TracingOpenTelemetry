using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();


var serviceName = "MuhApplication";


builder.Services.AddOpenTelemetry().WithTracing(tracerProviderBuilder =>
{
    tracerProviderBuilder
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName).AddTelemetrySdk())
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSource(serviceName)
            .AddConsoleExporter()
            .AddJaegerExporter();
}).WithMetrics(options =>
{
    options.AddHttpClientInstrumentation()
    .AddConsoleExporter()
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName).AddTelemetrySdk()).AddMeter("Meter1");
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
