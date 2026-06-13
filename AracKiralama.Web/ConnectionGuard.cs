using AracKiralama.Data;
using Microsoft.Data.SqlClient;

namespace AracKiralama.Web;

public static class ConnectionGuard
{
    public static void EnsureTargetDatabase(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        var db = builder.InitialCatalog;
        if (!string.Equals(db, DatabaseConstants.TargetDatabaseName, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Uygulama yalnızca '{DatabaseConstants.TargetDatabaseName}' veritabanına bağlanmalı. " +
                $"Şu anki Initial Catalog: '{db}'. appsettings içindeki bağlantıyı düzeltin.");
        }
    }
}
