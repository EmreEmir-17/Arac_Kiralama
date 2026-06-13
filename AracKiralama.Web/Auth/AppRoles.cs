namespace AracKiralama.Web.Auth;

public static class AppRoles
{
    public const string Admin = "Admin";
    public const string Operator = "Operator";
    public const string ReadOnly = "ReadOnly";
    public const string Customer = "Customer";
}

public static class AppPolicies
{
    public const string CanWrite = "CanWrite";
}
