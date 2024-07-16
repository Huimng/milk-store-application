using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects;
using DAL;
using BusinessLogics.Services;
using Microsoft.AspNetCore.Authorization;

namespace MilkStore.Pages.AccountManager
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IAccountService _accountService;

        public CreateModel(IServiceProvider serviceProvider)
        {
            _accountService = serviceProvider.GetRequiredService<IAccountService>();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Account Account { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || Account == null)
            {
                return Page();
            }
            var checkaccount = _accountService.GetAccounts().Where(x => x.Email == Account.Email).FirstOrDefault();
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
            Account.Role = AccountRoles.Staff;
            Account.Status = true;
            Account.CreatedDate = DateTime.UtcNow;
            Account.UpdateDate = DateTime.UtcNow;
            _accountService.AddAccount(Account);
            return RedirectToPage("./Index");
        }
    }
}
