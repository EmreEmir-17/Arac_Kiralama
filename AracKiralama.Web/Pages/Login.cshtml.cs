using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using AracKiralama.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AracKiralama.Web.Pages;

[AllowAnonymous]
public class LoginModel(SignInManager<ApplicationUser> signInManager) : PageModel
{
    [BindProperty]
    public LoginInput Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public void OnGet([FromQuery] string? returnUrl)
    {
        Input.ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await signInManager.PasswordSignInAsync(
            Input.Email!,
            Input.Password!,
            Input.RememberMe,
            lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            ErrorMessage = "E-posta veya şifre hatalı.";
            return Page();
        }

        if (!string.IsNullOrEmpty(Input.ReturnUrl) && Url.IsLocalUrl(Input.ReturnUrl))
        {
            return LocalRedirect(Input.ReturnUrl);
        }

        return LocalRedirect("/");
    }

    public sealed class LoginInput
    {
        public string? ReturnUrl { get; set; }

        [Required(ErrorMessage = "E-posta zorunlu")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta girin")]
        [Display(Name = "E-posta")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunlu")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string? Password { get; set; }

        [Display(Name = "Beni hatırla")]
        public bool RememberMe { get; set; }
    }
}
