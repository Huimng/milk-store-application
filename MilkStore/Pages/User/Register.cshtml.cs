using BusinessLogics.Services;
using DAL.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects;

namespace MilkStore.Pages.User
{
    public class RegisterModel : PageModel
    {
        private readonly IAccountService _accountService;
        public RegisterModel(IServiceProvider serviceProvider) 
        {
            _accountService = serviceProvider.GetRequiredService<IAccountService>();
        }

        [BindProperty]
        public Account Account { get; set; }


        public IActionResult OnGet()
        {
            return Page();
        }
        public IActionResult OnPost() 
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var checkaccount = _accountService.GetAccounts().Where(x=>x.Email ==  Account.Email).FirstOrDefault();
            if (checkaccount != null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email existed.");
                return Page();
            }
            var checkaccount2 = _accountService.GetAccounts().Where(x => x.Username == Account.Username).FirstOrDefault();
            if (checkaccount2 != null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username existed.");
                return Page();
            }
            Account.CreatedDate = DateTime.UtcNow;
            Account.UpdateDate = DateTime.UtcNow;
            Account.Status = true; // Active by default
            Account.Role = AccountRoles.Member; // Default role
            var account = Account;
            _accountService.AddAccount(account);
            return RedirectToPage("/User/Login"); ;
        }
    }
}
