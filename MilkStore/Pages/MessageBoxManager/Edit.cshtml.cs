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
using Microsoft.AspNetCore.SignalR;
using MilkStore.Hubs;

namespace MilkStore.Pages.MessageBoxManager
{
    public class EditModel : PageModel
    {
        private readonly MessageGroupService messageGroupService;
        private IAccountService accountService;
        private readonly IHubContext<ChatHub> _hub;

        public EditModel(IServiceProvider serviceProvider, IHubContext<ChatHub> hub)
        {
            _hub = hub;
            messageGroupService = new MessageGroupService();
            accountService = serviceProvider.GetRequiredService<IAccountService>();         
        }

    [BindProperty]
        public MessageGroup MessageGroup { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var messageGroup = messageGroupService.GetMessageGroup(id);
            if (messageGroup == null)
            {
                return NotFound();
            }

            MessageGroup = messageGroup;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existing = messageGroupService.GetMessageGroup(MessageGroup.MessageGroupId);
            if (existing == null)
            {
                return NotFound();
            }

            try
            {
                MessageGroup.CustomerId = existing.CustomerId;
                MessageGroup.ManagerId = existing.ManagerId;
                MessageGroup.MessageGroupId = existing.MessageGroupId;

                messageGroupService.UpdateMessageGroup(MessageGroup);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
            await _hub.Clients.All.SendAsync("Loading");
            return RedirectToPage("./Index");
        }
    }
}