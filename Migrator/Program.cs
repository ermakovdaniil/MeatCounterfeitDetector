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

        var adminName = "Admin";
        var technologistName = "Technologist";
        var userManager = sp.GetRequiredService<UserManager<User>>();
        var roleManager = sp.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        if (!await roleManager.RoleExistsAsync(UserRolesConstants.Admin))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(UserRolesConstants.Admin));
        }

        if (!await roleManager.RoleExistsAsync(UserRolesConstants.Technologist))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(UserRolesConstants.Technologist));
        }

        var adminUserExists = await userManager.FindByNameAsync(adminName) is not null;
        if (!adminUserExists)
        {
            User admin = new()
            {
                Email = "admin@mail.ru",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = adminName,
                Name = "Иванов А. А."
            };

            var adminResult = await userManager.CreateAsync(admin, "SuperMegaSecretPassword123!!!");

            if (!adminResult.Succeeded)
            {
                return;
            }

            if (await roleManager.RoleExistsAsync(UserRolesConstants.Admin))
            {
                await userManager.AddToRoleAsync(admin, UserRolesConstants.Admin);
            }

            if (await roleManager.RoleExistsAsync(UserRolesConstants.Technologist))
            {
                await userManager.AddToRoleAsync(admin, UserRolesConstants.Technologist);
            }
        }
        

        var technologistExists = await userManager.FindByNameAsync(technologistName) is not null;
        if (!technologistExists)
        {
            User technologist = new()
            {
                Email = "technologist@mail.ru",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = technologistName,
                Name = "Александров Б. Ю."
            };

            var technologistResult = await userManager.CreateAsync(technologist, "SuperMegaSecretPassword123!!!");

            if (!technologistResult.Succeeded)
            {
                return;
            }

            if (await roleManager.RoleExistsAsync(UserRolesConstants.Technologist))
            {
                await userManager.AddToRoleAsync(technologist, UserRolesConstants.Technologist);
            }
        }       
    }
}


