using DataAccess.Data;
using MeatAPI.Features.Counterfeit;
using MeatAPI.Features.CounterfeitPath;
using MeatAPI.Features.OriginalPath;
using MeatAPI.Features.Result;
using MeatAPI.Features.ResultPath;
using MeatAPI.Features.User;
using MeatAPI.Features.UserType;
using MeatAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(EFGenericRepository<>));

builder.Services.AddTransient<CounterfeitService>();
builder.Services.AddTransient<CounterfeitPathService>();
builder.Services.AddTransient<OriginalPathService>();
builder.Services.AddTransient<ResultService>();
builder.Services.AddTransient<ResultPathService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<UserTypeService>();

builder.Services.AddDbContext<ResultDBContext>(options =>
{
    options.UseNpgsql("Host=localhost;Port=5432;Database=ResultDB;Username=postgres;Password=sword-fish");
});

builder.Services.AddDbContext<CounterfeitKBContext>(options =>
{
    options.UseNpgsql("Host=localhost;Port=5432;Database=CounterfeitKB;Username=postgres;Password=sword-fish");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers().AllowAnonymous();

app.Run();
