using Arquetipo.API.Configurations;
using Arquetipo.API.Filters;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// agregar Healthcheck
builder.Services.AddHealthChecks();

// configuracion de serilog
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

// Add services to the container.
builder.Services.AddScoped<ModelValidationFilter>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.IgnoreNullValues = true;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.WriteIndented = true;
    }).
    ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true).
    AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.AddApplicationHealthCheck();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

//registro el middleware
app.AddGlobalErrorHandler();

app.MapControllers();

app.Run();
