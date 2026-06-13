using AracKiralama.Data;
using AracKiralama.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AracKiralama.Web.Auth;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration, IHostEnvironment environment)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in new[] { AppRoles.Admin, AppRoles.Operator, AppRoles.ReadOnly, AppRoles.Customer })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var email = configuration["Seed:AdminEmail"] ?? "admin@local";
        var password = configuration["Seed:AdminPassword"] ?? "Admin123!";

        var admin = await userManager.FindByEmailAsync(email);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                DisplayName = "Yönetici"
            };

            var create = await userManager.CreateAsync(admin, password);
            if (!create.Succeeded)
            {
                var msg = string.Join("; ", create.Errors.Select(e => $"{e.Code}:{e.Description}"));
                if (environment.IsDevelopment())
                {
                    throw new InvalidOperationException("Admin kullanıcı oluşturulamadı: " + msg);
                }
            }
        }

        admin = await userManager.FindByEmailAsync(email);
        if (admin is not null && !await userManager.IsInRoleAsync(admin, AppRoles.Admin))
        {
            await userManager.AddToRoleAsync(admin, AppRoles.Admin);
        }

        var roEmail = configuration["Seed:ReadOnlyEmail"];
        var roPassword = configuration["Seed:ReadOnlyPassword"];
        if (!string.IsNullOrWhiteSpace(roEmail) && !string.IsNullOrWhiteSpace(roPassword))
        {
            var ro = await userManager.FindByEmailAsync(roEmail);
            if (ro is null)
            {
                ro = new ApplicationUser
                {
                    UserName = roEmail,
                    Email = roEmail,
                    EmailConfirmed = true,
                    DisplayName = "Salt okunur"
                };
                var roCreate = await userManager.CreateAsync(ro, roPassword);
                if (!roCreate.Succeeded)
                {
                    if (environment.IsDevelopment())
                    {
                        throw new InvalidOperationException("ReadOnly kullanıcı oluşturulamadı.");
                    }

                    return;
                }
            }

            ro = await userManager.FindByEmailAsync(roEmail);
            if (ro is not null && !await userManager.IsInRoleAsync(ro, AppRoles.ReadOnly))
            {
                await userManager.AddToRoleAsync(ro, AppRoles.ReadOnly);
            }
        }

        await SeedCatalogAsync(services);
        await DemoDataSeeder.SeedIfEnabledAsync(services, configuration);
        await DemoDataSeeder.EnsureBillingAndDataSyncAsync(services, configuration);
    }

    public static async Task SeedCatalogAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<AppDbContext>();
        if (!await db.ExtraProducts.AnyAsync())
        {
            db.ExtraProducts.AddRange(
                new ExtraProduct { Code = "GPS", Name = "GPS navigasyon", UnitPrice = 150, IsActive = true },
                new ExtraProduct { Code = "CHILDSEAT", Name = "Çocuk koltuğu", UnitPrice = 120, IsActive = true },
                new ExtraProduct { Code = "ADDDRIVER", Name = "Ek sürücü", UnitPrice = 80, IsActive = true });
        }

        if (!await db.Campaigns.AnyAsync())
        {
            db.Campaigns.Add(new Campaign
            {
                Code = "WELCOME10",
                Name = "Hoş geldin %10",
                PercentDiscount = 10,
                ValidFromUtc = DateTime.UtcNow.AddYears(-1),
                ValidToUtc = DateTime.UtcNow.AddYears(5),
                IsActive = true
            });
        }

        await db.SaveChangesAsync();
    }
}
