using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

var builder = WebApplication.CreateBuilder(args);

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// configure dbcontext for ef core stuff
var connectionString = builder.Configuration.GetConnectionString("mySqlDb");
builder.Services.AddDbContext<MySqlDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Add Identity --> see UserModel
builder.Services.AddIdentity<UserModel, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<MySqlDbContext>()
    .AddDefaultTokenProviders();

// JWT Auth
builder.Services.Configure<JwtAuth>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton<JwtAuth>(); // unnecessary?

var jwtSecret = builder.Configuration["JwtSettings:Secret"];

if(string.IsNullOrEmpty(jwtSecret))
{
    throw new InvalidOperationException("JWT Secret is not set in configuration.");
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
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
