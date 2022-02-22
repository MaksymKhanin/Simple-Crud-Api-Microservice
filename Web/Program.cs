using Api.Configuration;
using Azure.Identity;
using Business.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Diagnostics.HealthChecks;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidationService();
builder.Services.AddAuthentication()
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.Authority = builder.Configuration["Authorization:Authority"];
            options.Audience = AuthenticationConstants.Setup.Audience;

            if (builder.Environment.IsDevelopment())
            {
                options.RequireHttpsMetadata = false;
            }

            options.TokenValidationParameters.ValidTypes = new[] { AuthenticationConstants.Setup.TokenType };
            options.TokenValidationParameters.ValidateIssuerSigningKey = true;
            options.TokenValidationParameters.ValidateAudience = true;
            options.TokenValidationParameters.ValidateIssuer = true;
            options.TokenValidationParameters.ValidateLifetime = true;

            options.Events = new()
            {
                OnAuthenticationFailed = ctx =>
                {
                    return Task.CompletedTask;
                }
            };
        });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("https://localhost:44448", "DEV URL")
                                .AllowAnyHeader()
                                .AllowAnyMethod().SetIsOriginAllowed(origin => true).AllowCredentials();
        });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthenticationConstants.Policies.SimpleApiTemplate, builder =>
    {
        builder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireClaim("scope", AuthenticationConstants.Policies.SimpleApiTemplateScope);
    });
});

builder.Host.ConfigureAppConfiguration((context, builder) =>
{
    if (context.HostingEnvironment.IsProduction())
    {
        builder.AddAzureAppConfiguration(options =>
        {
            var appConfigEndpoint = new Uri(Environment.GetEnvironmentVariable("APP_CONFIG_ENDPOINT") ?? throw new InvalidOperationException("The environment variable APP_CONFIG_ENDPOINT is missing"));
            var credential = new DefaultAzureCredential();
            var label = Environment.GetEnvironmentVariable("APP_CONFIG_LABEL") ?? throw new InvalidOperationException("The environment variable APP_CONFIG_LABEL is missing");
            var sharedLabel = Environment.GetEnvironmentVariable("APP_CONFIG_LABEL_SHARED") ?? throw new InvalidOperationException("The environment variable APP_CONFIG_LABEL_SHARED is missing");

            options.Connect(appConfigEndpoint, credential)
                .Select(KeyFilter.Any, label)
                .Select(KeyFilter.Any, sharedLabel)
                .ConfigureKeyVault(kvOpts => kvOpts.SetCredential(credential));
        });
    }
});

var assembly = AppDomain.CurrentDomain.Load("Api");
builder.Services.AddAutoMapper(assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors(builder =>
{
    builder.WithOrigins("https://localhost:44448", "DEV URL")
                        .AllowAnyHeader()
                        .AllowAnyMethod().SetIsOriginAllowed(origin => true).AllowCredentials();
});
app.UseAuthorization();
app.MapControllers().RequireAuthorization(AuthenticationConstants.Policies.SimpleApiTemplate);
app.MapHealthChecks("/health", new()
{
    ResponseWriter = async (HttpContext context, HealthReport healthReport) =>
        await context.Response.WriteAsJsonAsync(healthReport)
});

app.Run();

namespace SimpleApiTemplate
{
    public partial class Program { }
}