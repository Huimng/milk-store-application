using BusinessLogics.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using BusinessObjects;

namespace MilkStore.Pages.User
{
    public class LoginModel : PageModel
    {
        private readonly IAccountService _accountService;
        public LoginModel(IServiceProvider serviceProvider)
        {
            _accountService = serviceProvider.GetRequiredService<IAccountService>(); 
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var account =  _accountService.GetAccounts()
                    .FirstOrDefault(a => a.Username == Input.Username && a.Password == Input.Password);
                if (account != null && account.Status == true)
                {

                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.Name),
                    new Claim(ClaimTypes.Email, account.Email),
                    new Claim("AccountId", account.AccountId.ToString()),
                    new Claim(ClaimTypes.Role, account.Role.ToString())
                };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    // Set cookie with username
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(30),
                        HttpOnly = true,
                        Secure = true // Ensure this is used in production over HTTPS
                    };
                    Response.Cookies.Append("Username", account.Username, cookieOptions);
                    if (account.Role == AccountRoles.Member)
                    {
                        return RedirectToPage("/Home/Product");
                    }else if (account.Role == AccountRoles.Staff)
                    {
                        return RedirectToPage("/Orders/GetOrdersStaff");
                    }
                    else
                    {
                        return RedirectToPage("/AccountManager/Index");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
               
            }
            return Page();
        }
    }
}
