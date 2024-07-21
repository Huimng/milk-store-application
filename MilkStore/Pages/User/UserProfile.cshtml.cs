using BusinessLogics.Services;
using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
#pragma warning  disable CS8603,CS8618    
namespace MilkStore.Pages.User;

public class UserProfile : PageModel
{
    private readonly IAccountService _accountService;

    public UserProfile(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [BindProperty]
    public Account Account { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var accountIdClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
        if (accountIdClaim == null)
        {
            return RedirectToPage("/User/Login");
        }

        int accountId = int.Parse(accountIdClaim.Value);
        Account = _accountService.GetAccounts().SingleOrDefault(a => a.AccountId == accountId);

        if (Account == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {

        var checkaccount = _accountService.GetAccounts().Where(x => x.Email == Account.Email).FirstOrDefault();
        if (checkaccount != null)
        {
            ModelState.AddModelError(string.Empty, "Invalid email existed.");
            return Page();
        }

        var accountIdClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
        if (accountIdClaim == null)
        {
            return RedirectToPage("/User/Login");
        }

        int accountId = int.Parse(accountIdClaim.Value);
        var accountToUpdate = _accountService.GetAccounts().SingleOrDefault(a => a.AccountId == accountId);

        if (accountToUpdate == null)
        {
            return NotFound();
        }

        accountToUpdate.Name = Account.Name;
        accountToUpdate.Email = Account.Email;
        accountToUpdate.Password = Account.Password;
        accountToUpdate.UpdateDate = DateTime.UtcNow;
        
        _accountService.UpdateAccount(accountToUpdate);

        return RedirectToPage();
    }
}