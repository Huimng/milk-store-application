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
using Microsoft.AspNetCore.SignalR;
using MilkStore.Hubs;

namespace MilkStore.Pages.MessageBoxManager
{
    public class CreateModel : PageModel
    {
        private readonly MessageGroupService messageGroupService;
        private IAccountService accountService;
        private readonly IHubContext<ChatHub> _hub;


        public CreateModel(IServiceProvider serviceProvider, IHubContext<ChatHub> hub)
        {
            _hub = hub;
            messageGroupService = new MessageGroupService();
            accountService = serviceProvider.GetRequiredService<IAccountService>();
            
        }

        public IActionResult OnGet()
        {
        ViewData["CustomerId"] = new SelectList(accountService.GetAccounts(), "AccountId", "Name");
            return Page();
        }

        [BindProperty]
        public MessageGroup MessageGroup { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || messageGroupService == null || MessageGroup == null)
            {
                return Page();
            }

            var username = Request.Cookies["Username"];
            int id = accountService.GetAccountByUserName(username).AccountId;
            MessageGroup.ManagerId = id;

            MessageGroup.Status = MessageStatuses.Opened;
            messageGroupService.AddMessageGroup(MessageGroup);
            await _hub.Clients.All.SendAsync("Loading");
            return RedirectToPage("./Index");
        }
    }
}
