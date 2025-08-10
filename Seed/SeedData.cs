using Microsoft.EntityFrameworkCore;
using VirocGanpati.Data;
using VirocGanpati.Models;

namespace VirocGanpati.Seed;

public static class SeedData
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        await SeedRoles(context);
        await SeedDefaultUsers(context, config);
        await SeedMandalAreas(context);
        await SeedArtiMorningTimes(context);
        await SeedArtiEveningTimes(context);
        await SeedDateOfVisarjan(context);
    }

    private static async Task SeedRoles(ApplicationDbContext context)
    {
        var roles = new List<Role>
        {
            new Role { RoleName = "Manager", RoleIdentifier = "Manager", RoleDescription = "Role for managers with full access" },
            new Role { RoleName = "Manager View", RoleIdentifier = "ManagerView", RoleDescription = "Role for managers with view-only access" },
            new Role { RoleName = "User", RoleIdentifier = "User", RoleDescription = "Standard user role" }
        };

        foreach (var role in roles)
        {
            if (!await context.Roles.AnyAsync(r => r.RoleIdentifier == role.RoleIdentifier))
                context.Roles.Add(role);
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedDefaultUsers(ApplicationDbContext context, IConfiguration config)
    {
        var managerRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleIdentifier == "Manager");
        var managerViewRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleIdentifier == "ManagerView");

        if (managerRole == null || managerViewRole == null)
        {
            throw new Exception("Roles not found. Please ensure roles are seeded first.");
        }

        if (!await context.Users.AnyAsync(user => user.RoleId == managerRole.RoleId))
        {
            var managerUser = new User
            {
                FirstName = "Manager",
                LastName = "User",
                Email = "manager@viroc.ganpati.in",
                Mobile = "9999999999",
                Password = BCrypt.Net.BCrypt.HashPassword(config["DefaultPasswords:Manager"] ?? throw new Exception("Manager password missing")),
                RoleId = managerRole.RoleId,
                CreatedAt = DateTime.UtcNow,
                IsMobileVerified = true
            };
            context.Users.Add(managerUser);
        }

        // Check and seed Manager-View user if not exists
        //if (!await context.Users.AnyAsync(user => user.Role.RoleIdentifier == "ManagerView"))
        //{
        //    var managerViewUser = new User
        //    {
        //        FirstName = "Viewer",
        //        LastName = "User",
        //        Email = "viewer@swasurvey.in",
        //        Password = BCrypt.Net.BCrypt.HashPassword(config["DefaultPasswords:ManagerView"] ?? throw new InvalidOperationException("Default ManagerView password is not configured.")),
        //        RoleId = managerViewRole.RoleId,
        //        CreatedAt = DateTime.UtcNow
        //    };

        //    context.Users.Add(managerViewUser);
        //    Console.WriteLine("Default Manager-View user created.");
        //}

        if (context.ChangeTracker.HasChanges())
            await context.SaveChangesAsync();
    }

    public static async Task SeedMandalAreas(ApplicationDbContext context)
    {
        if (!await context.MandalAreas.AnyAsync())
        {
            var areas = new List<MandalArea>
                {
                    new() { AreaName = "Ajwa Road- Waghodia road- Khodiyarnagar" },
                    new() { AreaName = "Vadsar- Kalali- Atladara" },
                    new() { AreaName = "Karelibaug – Harni-Warsiya" },
                    new() { AreaName = "City- Raopura- Dandiya bazar- vadi" },
                    new() { AreaName = "Bill- Bhayli- Vasna- Sunpharma" },
                    new() { AreaName = "Tarsali- Danteshwar- Manjalpur" },
                    new() { AreaName = "Subhapura- Gorwa- Laxmipura" },
                    new() { AreaName = "Fatehgunj – Sama,savli road" },
                    new() { AreaName = "Others" }
                };

            context.MandalAreas.AddRange(areas);
            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedArtiMorningTimes(ApplicationDbContext context)
    {
        if (!context.ArtiMorningTimes.Any())
        {
            var times = new List<ArtiMorningTime>
        {
            new ArtiMorningTime { Value = "7:00 a.m" },
            new ArtiMorningTime { Value = "7:30 a.m" },
            new ArtiMorningTime { Value = "8:00 a.m" },
            new ArtiMorningTime { Value = "8:30 a.m" },
            new ArtiMorningTime { Value = "9:00 a.m" },
            new ArtiMorningTime { Value = "9:30 a.m" },
            new ArtiMorningTime { Value = "10:00 a.m" },
        };

            context.ArtiMorningTimes.AddRange(times);
            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedArtiEveningTimes(ApplicationDbContext context)
    {
        if (!context.ArtiEveningTimes.Any())
        {
            var times = new List<ArtiEveningTime>
        {
            new ArtiEveningTime { Value = "7:00 p.m" },
            new ArtiEveningTime { Value = "7:30 p.m" },
            new ArtiEveningTime { Value = "8:00 p.m" },
            new ArtiEveningTime { Value = "8:30 p.m" },
            new ArtiEveningTime { Value = "9:00 p.m" },
        };

            context.ArtiEveningTimes.AddRange(times);
            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedDateOfVisarjan(ApplicationDbContext context)
    {
        if (!context.DateOfVisarjans.Any())
        {
            var times = new List<DateOfVisarjan>
        {
            new DateOfVisarjan { Value = "06/08/2025" },
            new DateOfVisarjan { Value = "06/09/2025" },
        };

            context.DateOfVisarjans.AddRange(times);
            await context.SaveChangesAsync();
        }
    }
}
