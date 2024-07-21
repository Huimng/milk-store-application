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
using Microsoft.AspNetCore.SignalR;
using MilkStore.Hubs;

namespace MilkStore.Pages.MessageBoxManager
{
    public class IndexModel : PageModel
    {
        private readonly MessageGroupService messageGroupService;
        private IAccountService accountService;
        private readonly IHubContext<ChatHub> _hub;
        public IndexModel(IServiceProvider serviceProvider, IHubContext<ChatHub> hub)
        {
            messageGroupService = new MessageGroupService();
            accountService = serviceProvider.GetRequiredService<IAccountService>();
            _hub = hub;
        }

        public IList<MessageGroup> MessageGroup { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (accountService != null)
            {
                string username = Request.Cookies["Username"];
                if (string.IsNullOrEmpty(username))
                {
                    
                    return RedirectToPage("/User/Login");
                }

                Account account = accountService.GetAccountByUserName(username);
                MessageGroup = messageGroupService.GetMessageGroupByAccount(account.AccountId);
            }

            return Page();
        }

    }
}
