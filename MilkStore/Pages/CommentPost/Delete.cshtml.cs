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

namespace MilkStore.Pages.CommentPost
{
    public class DeleteModel : PageModel
    {
        private CommentService commentService;
        private readonly IHubContext<ChatHub> _hub;
        public DeleteModel(IHubContext<ChatHub> hub)
        {
            commentService = new CommentService();
            _hub = hub;
        }

        [BindProperty]
      public Comment Comment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = commentService.GetComment(id);
            if (comment == null)
            {
                return NotFound();
            }
            else 
            {
                Comment = comment;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            commentService.DeleteComment(id);
            await _hub.Clients.All.SendAsync("Loading");
            return RedirectToPage("./Index");
        }
    }
}
