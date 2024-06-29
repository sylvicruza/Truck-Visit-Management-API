using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Truck_Visit_Management.Data;
using Truck_Visit_Management.Dtos;
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
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtUtils, JwtUtils>();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

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
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        Array.Empty<string>()
    }
    });
});

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Debug);
});

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TruckVisitDbContext>();
    context.Database.Migrate();
    DataSeeder.SeedUsers(context);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseRouting();

app.UseCors("CorsPolicy");

// Custom JWT auth middleware
app.UseMiddleware<JwtMiddleware>();

// Enable Swagger middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Truck Visit Management API v1");
  
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();