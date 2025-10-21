using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WebApplication1.Application_Layer.Services.Article;
using WebApplication1.Application_Layer.Services.ConsumptionData;
using WebApplication1.Application_Layer.Services.Leaderboard;
using WebApplication1.Application_Layer.Services.Transaction;
using WebApplication1.Application_Layer.Services.UserAuth;
using WebApplication1.Application_Layer.Services.UserExistCheck;
using WebApplication1.Application_Layer.Websockets;
using WebApplication1.Domain.Models.User;
using WebApplication1.Domain.Repositories;
using WebApplication1.Domain.Services;
using WebApplication1.Infrastructure.MySqlRepositories;

namespace WebApplication1.API;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = CreateHostBuilder(args);
        var app = ConfigureApp(builder);
        app.Run();
    }

    public static WebApplicationBuilder CreateHostBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Enable CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                policy =>
                {
                    policy.WithOrigins(
                            "http://localhost:3000",
                            "http://172.25.96.152:3000",
                            "http://172.25.96.152:5137",
                            "capacitor://localhost"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });

// configure dbcontext for ef core stuff
var connectionString = builder.Configuration.GetConnectionString("mySqlDb") ?? "Server=localhost;Database=test;User=test;Password=test;";
builder.Services.AddDbContext<MySqlDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Add Identity --> see UserModel
builder.Services.AddIdentity<UserModel, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<MySqlDbContext>()
    .AddDefaultTokenProviders();

// JWT Auth
builder.Services.Configure<JwtAuth>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<JwtAuth>();
// builder.Services.AddSingleton<JwtAuth>(); // unnecessary?

var jwtSecret = builder.Configuration["JwtSettings:Secret"];

if(string.IsNullOrEmpty(jwtSecret))
{
    // Use a default secret for testing environments
    if (builder.Environment.IsEnvironment("Testing"))
    {
        jwtSecret = "TestingSecretKey12345678901234567890123456789012";
    }
    else
    {
        throw new InvalidOperationException("JWT Secret is not set in configuration.");
    }
}

var key = Encoding.UTF8.GetBytes(jwtSecret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            NameClaimType = ClaimTypes.Name
        };
    });

builder.Services.AddAuthorization();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Add the JWT Bearer definition
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your JWT token.\n\nExample: Bearer eyJhbGciOiJIUzI1NiIsInR5..."
    });

    // Add global requirement
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


// Dependency Injection stuff --> registers dependencies
builder.Services.AddScoped<IConsumptionRecordRepository, MySqlConsumptionRecordRepository>();
builder.Services.AddScoped<IRecommendRecordRepository, MySqlRecommendRecord>();
builder.Services.AddScoped<IUserRepository, MySqlUserRepository>();
builder.Services.AddScoped<IUserAuth, UserAuth>();
builder.Services.AddScoped<IUserExistCheck, UserExistCheck>();
builder.Services.AddScoped<ITransactionRepository, MySqlTransactionRepository>();
builder.Services.AddScoped<IArticleRepository, MySqlArticleRepository>();
builder.Services.AddScoped<IArticleServices, ArticleServices>();
builder.Services.AddScoped<ITransactionServices, TransactionServices>();
builder.Services.AddScoped<IConsumptionDataService, ConsumptionDataService>();
builder.Services.AddScoped<ILeaderboardServices, LeaderboardServices>();
builder.Services.AddScoped<ILeaderboardRepository, MySqlLeaderboardRepository>();


        return builder;
    }

    public static WebApplication ConfigureApp(WebApplicationBuilder builder)
    {
        var app = builder.Build();

        // Use CORS
        app.UseCors("AllowFrontend");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();
        app.UseWebSockets();
        app.UseMiddleware<RewardPointsWebSocketHandler>();

        app.MapControllers();

        return app;
    }
}
