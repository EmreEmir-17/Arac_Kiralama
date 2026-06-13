using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AracKiralama.Data;

/// <summary>
/// EF CLI migration sırasında kullanılır. Veritabanı adı sabittir: <see cref="DatabaseConstants.TargetDatabaseName"/>.
/// </summary>
public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var basePath = FindWebProjectDirectory();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var cs = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection eksik.");

        ValidateDatabaseName(cs);

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(cs);
        return new AppDbContext(optionsBuilder.Options);
    }

    private static string FindWebProjectDirectory()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var candidate = Path.Combine(dir.FullName, "AracKiralama.Web", "appsettings.json");
            if (File.Exists(candidate))
            {
                return Path.Combine(dir.FullName, "AracKiralama.Web");
            }

            dir = dir.Parent;
        }

        throw new InvalidOperationException(
            "AracKiralama.Web/appsettings.json bulunamadı. 'dotnet ef' komutunu çözüm veya Web proje klasöründen çalıştırın.");
    }

    private static void ValidateDatabaseName(string connectionString)
    {
        var builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
        var db = builder.InitialCatalog;
        if (!string.Equals(db, DatabaseConstants.TargetDatabaseName, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Yanlış veritabanı: '{db}'. Yalnızca '{DatabaseConstants.TargetDatabaseName}' kullanılmalı.");
        }
    }
}
