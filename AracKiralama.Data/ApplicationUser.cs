using AracKiralama.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace AracKiralama.Data;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }

    public Customer? LinkedCustomer { get; set; }
}
