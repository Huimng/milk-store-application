using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using DAL;
using BusinessLogics.Services;
using Microsoft.AspNetCore.Authorization;

namespace MilkStore.Pages.AccountManager
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IAccountService _accountService;
        public EditModel(IServiceProvider serviceProvider)
        {
            _accountService = serviceProvider.GetRequiredService<IAccountService>();
        }

        [BindProperty]
        public Account Account { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = _accountService.GetAccount(id);
            if (account == null)
            {
                return NotFound();
            }
            Account = account;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }            

            try
            {
                _accountService.UpdateAccount(Account);
            }
            catch (DbUpdateConcurrencyException)
            {
              return NotFound();
            }

            return RedirectToPage("./Index");
        }


    }
}
