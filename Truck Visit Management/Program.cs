using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using Truck_Visit_Management.Data;
using Truck_Visit_Management.Exceptions;
using Truck_Visit_Management.Repositories;
using Truck_Visit_Management.Security;
using Truck_Visit_Management.Services;
using Truck_Visit_Management.Services.ServiceImpl;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddDbContext<TruckVisitDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TruckVisitDbContext") ?? throw new InvalidOperationException("Connection string 'TruckVisitDbContext' not found.")));

builder.Services.AddControllers();

builder.Services.AddScoped<IVisitRepository, VisitRepository>();
builder.Services.AddScoped<IVisitService, VisitService>();

// Add AutoMapper configuration
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// IdentityServer configuration
builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryApiScopes(new List<ApiScope> { new ApiScope("api1", "Access to Truck Visit Management API") })
    .AddInMemoryClients(new List<Client>
    {
        new Client
        {
            ClientId = "client",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets = { new Secret("secret".Sha256()) },
            AllowedScopes = { "api1" }
        }
    });

// Add authentication and authorization
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://localhost:5001"; // IdentityServer URL
        options.RequireHttpsMetadata = false; // Disable HTTPS only for development
        options.Audience = "api1"; // Must match the 'AllowedScopes' in IdentityServer4

        // Additional token validation parameters
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = "http://localhost:5001", // IdentityServer URL
            ValidAudience = "api1" // Must match the 'AllowedScopes' in IdentityServer4
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "api1");
    });
});

// Optionally add CORS if required
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Add Swagger/OpenAPI configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Truck Visit Management API", Version = "v1" });
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            ClientCredentials = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri("http://localhost:5001/connect/token"), // Ensure this matches the URL IdentityServer4 is running on
                Scopes = new Dictionary<string, string>
                {
                    { "api1", "Access to Truck Visit Management API" }
                }
            }
        }
    });
    c.OperationFilter<AuthorizeCheckOperationFilter>();
});

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Debug);
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("CorsPolicy");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Truck Visit Management API v1");
    c.OAuthClientId("client");
    c.OAuthClientSecret("secret");
    c.OAuthAppName("Truck Visit Management API");
    c.OAuthUsePkce();
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers().RequireAuthorization("ApiScope");
});

app.Run();
