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
        var resultConnectionString = config.GetConnectionString("Result");
        var countefeitConnectionString = config.GetConnectionString("Counterfeit");
        services.AddDbContext<ResultDBContext>(options =>
        {
            options.UseNpgsql(resultConnectionString);
        });

        services.AddDbContext<CounterfeitKBContext>(options =>
        {
            options.UseNpgsql(countefeitConnectionString);
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
        var userName = "user";
        var userManager = sp.GetRequiredService<UserManager<User>>();
        var roleManager = sp.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(UserRoles.Admin));
        }

        if (!await roleManager.RoleExistsAsync(UserRoles.User))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(UserRoles.User));
        }

        var adminUserExists = await userManager.FindByNameAsync(adminName) is not null;
        if (!adminUserExists)
        {
            User admin = new()
            {
                Email = "admin@kys.quick",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = adminName,
                Name = "admin" // REQUIRED!!!!!!!
            };


            var result = await userManager.CreateAsync(admin, "SuperMegaSecretPassword123!!!");

            if (!result.Succeeded)
            {
                return;
            }

            if (await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await userManager.AddToRoleAsync(admin, UserRoles.Admin);
            }

            if (await roleManager.RoleExistsAsync(UserRoles.User))
            {
                await userManager.AddToRoleAsync(admin, UserRoles.User);
            }
        }
        

        var userExists = await userManager.FindByNameAsync(userName) is not null;
        if (!userExists)
        {
            User user = new()
            {
                Email = "user@kys.quick",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = userName,
                Name = "user" // REQUIRED!!!!!!!
            };

            var userResult = await userManager.CreateAsync(user, "SuperMegaSecretPassword123!!!");

            if (!userResult.Succeeded)
            {
                return;
            }

            if (await roleManager.RoleExistsAsync(UserRoles.User))
            {
                await userManager.AddToRoleAsync(user, UserRoles.User);
            }
        }       
    }
}


