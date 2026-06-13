using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using AracKiralama.Data;
using AracKiralama.Data.Entities;
using AracKiralama.Web.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AracKiralama.Web.Pages;

[AllowAnonymous]
public class RegisterModel(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    AppDbContext db) : PageModel
{
    [BindProperty]
    public RegisterInput Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Input.Password != Input.ConfirmPassword)
        {
            ErrorMessage = "Şifreler eşleşmiyor.";
            return Page();
        }

        if (Input.IsCorporate && string.IsNullOrWhiteSpace(Input.TaxNumber))
        {
            ModelState.AddModelError(nameof(Input.TaxNumber), "Kurumsal kayıtta vergi numarası zorunludur.");
            return Page();
        }

        if (await userManager.FindByEmailAsync(Input.Email!) is not null)
        {
            ErrorMessage = "Bu e-posta zaten kayıtlı.";
            return Page();
        }

        var user = new ApplicationUser
        {
            UserName = Input.Email,
            Email = Input.Email,
            EmailConfirmed = true,
            DisplayName = Input.FullName
        };

        var create = await userManager.CreateAsync(user, Input.Password!);
        if (!create.Succeeded)
        {
            ErrorMessage = MapIdentityErrors(create.Errors);
            return Page();
        }

        user = await userManager.FindByEmailAsync(Input.Email!) ?? user;
        await userManager.AddToRoleAsync(user, AppRoles.Customer);

        var customer = new Customer
        {
            ApplicationUserId = user.Id,
            FullName = Input.FullName!.Trim(),
            Email = Input.Email!.Trim(),
            Phone = string.IsNullOrWhiteSpace(Input.Phone) ? null : Input.Phone.Trim(),
            IsCorporate = Input.IsCorporate,
            TaxNumber = Input.IsCorporate ? Input.TaxNumber?.Trim() : null
        };
        db.Customers.Add(customer);
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            await userManager.DeleteAsync(user);
            ErrorMessage = "Müşteri kaydı oluşturulamadı.";
            return Page();
        }

        await signInManager.SignInAsync(user, isPersistent: false);
        return LocalRedirect("/");
    }

    private static string MapIdentityErrors(IEnumerable<IdentityError> errors)
    {
        static string MapOne(IdentityError e) => e.Code switch
        {
            "DuplicateUserName" or "DuplicateEmail" => "Bu e-posta adresi zaten kayıtlı.",
            "InvalidEmail" => "E-posta adresi geçersiz.",
            "PasswordTooShort" => "Şifre çok kısa.",
            "PasswordRequiresNonAlphanumeric" => "Şifre en az bir özel karakter içermeli.",
            "PasswordRequiresDigit" => "Şifre en az bir rakam içermeli.",
            "PasswordRequiresLower" => "Şifre en az bir küçük harf içermeli.",
            "PasswordRequiresUpper" => "Şifre en az bir büyük harf içermeli.",
            "PasswordRequiresUniqueChars" => "Şifre yeterince farklı karakter içermiyor.",
            _ => e.Description
        };

        return string.Join(" ", errors.Select(MapOne));
    }

    public sealed class RegisterInput
    {
        [Required(ErrorMessage = "Ad soyad zorunlu")]
        [Display(Name = "Ad soyad")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "E-posta zorunlu")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta girin")]
        [Display(Name = "E-posta")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Geçerli bir telefon girin")]
        [Display(Name = "Telefon")]
        public string? Phone { get; set; }

        [Display(Name = "Müşteri türü")]
        public bool IsCorporate { get; set; }

        [Display(Name = "Vergi numarası")]
        public string? TaxNumber { get; set; }

        [Required(ErrorMessage = "Şifre zorunlu")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrarı zorunlu")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre tekrar")]
        public string? ConfirmPassword { get; set; }
    }
}
