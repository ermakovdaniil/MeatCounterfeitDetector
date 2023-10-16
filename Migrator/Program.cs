// See https://aka.ms/new-console-template for more information

using DataAccess.Data;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    public static async Task Main()
    {
        var sp = InitDI();
        MigrateDB(sp);
        await CreateAdmin(sp);
    }
    
    private static IServiceProvider InitDI()
    {
        var services = new ServiceCollection();
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var connectionString = config.GetConnectionString("Postgres");
        services.AddDbContext<ResultDBContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddDbContext<CounterfeitKBContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ResultDBContext>()
            .AddDefaultTokenProviders();
        
        services.AddLogging();
        
        var sp = services.BuildServiceProvider();
        return sp;
    }

    private static void MigrateDB(IServiceProvider sp)
    {
        var resultsContext = sp.GetRequiredService<ResultDBContext>();
        var counterfeitContext = sp.GetRequiredService<CounterfeitKBContext>();

        resultsContext.Database.Migrate();
        counterfeitContext.Database.Migrate();
    }

    private static async Task CreateAdmin(IServiceProvider sp)
    {

        var adminName = "admin";
        var userManager = sp.GetRequiredService<UserManager<User>>();
        var roleManager = sp.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        var adminUserExists = await userManager.FindByNameAsync(adminName) is not null;
        if (adminUserExists)
        {
            return;
        }
        User appUser = new()
        {
            Email = "admin@kys.quick",
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = adminName,
            Name = "admin" // REQUIRED!!!!!!!
        };
        
        
        var result = await userManager.CreateAsync(appUser, "SuperMegaSecretPassword123!!!");

        if (!result.Succeeded)
        {
            return;
        }
        
        if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(UserRoles.Admin));
        }

        if (!await roleManager.RoleExistsAsync(UserRoles.User))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(UserRoles.User));
        }

        if (await roleManager.RoleExistsAsync(UserRoles.Admin))
        {
            await userManager.AddToRoleAsync(appUser, UserRoles.Admin);
        }

        if (await roleManager.RoleExistsAsync(UserRoles.User))
        {
            await userManager.AddToRoleAsync(appUser, UserRoles.User);
        }
    }
}


