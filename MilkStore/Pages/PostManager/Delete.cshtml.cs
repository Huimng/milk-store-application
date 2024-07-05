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

namespace MilkStore.Pages.PostManager
{
    public class DeleteModel : PageModel
    {
        private PostService postService;
        private readonly IHubContext<ChatHub> _hub;
        public DeleteModel(IHubContext<ChatHub> hub)
        {
            postService = new PostService();
            _hub = hub;
        }

        [BindProperty]
      public Post Post { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = postService.GetPost(id);

            if (post == null)
            {
                return NotFound();
            }
            else 
            {
                Post = post;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            

            if (Post != null)
            {
                postService.DeletePost(id);
            }
            await _hub.Clients.All.SendAsync("Loading");
            return RedirectToPage("./Index");
        }
    }
}
