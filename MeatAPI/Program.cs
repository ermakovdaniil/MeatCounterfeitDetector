using System.Text;
using DataAccess.Data;
using DataAccess.Models;
using MeatAPI.Features;
using MeatAPI.Features.CounterfeitImage;
using MeatAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MeatAPI.Features.Result;
using MeatAPI.Features.User;
using MeatAPI.Features.UserRole;
using MeatAPI.Features.OriginalImage;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(EFGenericRepository<>));

builder.Services.AddTransient<EntityAccessServiceBase<CounterfeitKBContext, Counterfeit>>();
builder.Services.AddTransient<CounterfeitImageService>();
builder.Services.AddTransient<ResultService>();
builder.Services.AddTransient<OriginalImageService>();
builder.Services.AddTransient<EntityAccessServiceBase<ResultDBContext, ResultImage>>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<UserRoleService>();

//builder.Services.AddTransient<EntityAccessServiceBase<ResultDBContext, UserRole>>();

builder.Services.AddDbContext<ResultDBContext>(options =>
{
    options.UseNpgsql("Host=localhost;Port=5432;Database=ResultDB;Username=postgres;Password=sword-fish");
});

builder.Services.AddDbContext<CounterfeitKBContext>(options =>
{
    options.UseNpgsql("Host=localhost;Port=5432;Database=CounterfeitKB;Username=postgres;Password=sword-fish");
});

builder.Services.AddEntityFrameworkNpgsql();

builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ResultDBContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,

            ValidAudience = configuration["JWT:ValidAudience"],
            ValidIssuer = configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
        };
    });

builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();