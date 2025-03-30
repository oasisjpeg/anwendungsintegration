using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApplication1.Application_Layer.Services.UserAuth;
using WebApplication1.Application_Layer.Services.UserExistCheck;
using WebApplication1.Domain.Repositories;
using WebApplication1.Infrastructure.MySqlRepositories;

var builder = WebApplication.CreateBuilder(args);

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // Change this to your frontend URL
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


builder.Services.Configure<JwtAuth>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton<JwtAuth>();

var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
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


builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("mySqlDb");
builder.Services.AddDbContext<MySqlDbContext>(options => 
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IConsumptionRecordRepository, MySqlConsumptionRecordRepository>();

builder.Services.AddScoped<IUserRepository, MySqlUserRepository>();
builder.Services.AddScoped<IUserAuth, UserAuth>();
builder.Services.AddScoped<IUserExistCheck, UserExistCheck>();



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

app.UseAuthorization();

app.MapControllers();

app.Run();
