using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using DAL;
using BusinessLogics.Services;
using Microsoft.AspNetCore.Authorization;

namespace MilkStore.Pages.AccountManager
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;

        public IndexModel(IServiceProvider serviceProvider)
        {
            _accountService = serviceProvider.GetRequiredService<IAccountService>();
        }

        public IList<Account> Account { get;set; } = default!;

        public async Task OnGetAsync()
        {

            Account = _accountService.GetAlllAccountAdmin();

        }
    }
}
