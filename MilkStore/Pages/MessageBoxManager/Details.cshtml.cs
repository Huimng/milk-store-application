using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects;
using BusinessLogics.Services;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using MilkStore.Hubs;

namespace MilkStore.Pages.MessageGroupManager
{
    public class DetailsModel : PageModel
    {
        private readonly MessageGroupService _messageGroupService;
        private readonly MessageService _messageService;
        private readonly IHubContext<ChatHub> _hub;
        public DetailsModel(IServiceProvider serviceProvidert, IHubContext<ChatHub> hub)
        {
            _hub = hub;
            _messageGroupService = new MessageGroupService();
            _messageService = new MessageService();
        }

        public MessageGroup MessageGroup { get; set; } = new MessageGroup();
        public IList<Message> Messages { get; set; } = new List<Message>();

        [BindProperty]
        public Message NewMessage { get; set; } = new Message();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var messageGroup = _messageGroupService.GetMessageGroup(id);
            if (messageGroup == null)
            {
                return NotFound();
            }

            MessageGroup = messageGroup;
            Messages = _messageService.MessageInAGroupMessage(id);

            await _hub.Clients.All.SendAsync("Loading");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                var messgroup = _messageGroupService.GetMessageGroup(id);
                if (messgroup == null)
                {
                    return NotFound();
                }

                MessageGroup = messgroup;
                Messages = _messageService.MessageInAGroupMessage(id);
                foreach (var mess in Messages)
                {
                    mess.MessageGroup = _messageGroupService.GetMessageGroup(mess.GroupId);
                }
                return Page();
            }

            var messageGroup = _messageGroupService.GetMessageGroup(id);
            if (messageGroup == null)
            {
                return NotFound();
            }

            NewMessage.GroupId = id;
            NewMessage.ManagerId = messageGroup.ManagerId;
            NewMessage.CustomerId = messageGroup.CustomerId;
            await _hub.Clients.All.SendAsync("Loading");



            _messageService.Chat(NewMessage);
            await _hub.Clients.All.SendAsync("Loading");
            return RedirectToPage(new { id = id });
        }
    }
}