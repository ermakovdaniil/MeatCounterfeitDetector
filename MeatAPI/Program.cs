using DataAccess.Data;
using DataAccess.Models;
using MeatAPI.Features;
using MeatAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(EFGenericRepository<>));

//builder.Services.AddTransient<CounterfeitService>();
//builder.Services.AddTransient<CounterfeitPathService>();
//builder.Services.AddTransient<OriginalPathService>();
//builder.Services.AddTransient<ResultService>();
//builder.Services.AddTransient<ResultPathService>();
//builder.Services.AddTransient<UserService>();
//builder.Services.AddTransient<UserTypeService>();

builder.Services.AddTransient<EntityAccessServiceBase<CounterfeitKBContext, Counterfeit>>();
builder.Services.AddTransient<EntityAccessServiceBase<CounterfeitKBContext, CounterfeitPath>>();
builder.Services.AddTransient<EntityAccessServiceBase<ResultDBContext, Result>>();
builder.Services.AddTransient<EntityAccessServiceBase<ResultDBContext, OriginalPath>>();
builder.Services.AddTransient<EntityAccessServiceBase<ResultDBContext, ResultPath>>();
builder.Services.AddTransient<EntityAccessServiceBase<ResultDBContext, User>>();
builder.Services.AddTransient<EntityAccessServiceBase<ResultDBContext, UserType>>();

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