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
using Microsoft.Extensions.DependencyInjection;

namespace MilkStore.Pages.PostManager
{
    public class DeleteModel : PageModel
    {
        private PostService postService;
        private readonly IHubContext<ChatHub> _hub;
        private readonly IAccountService _accountService;

        public DeleteModel(IServiceProvider serviceProvider, IHubContext<ChatHub> hub)
        {
            postService = new PostService();
            _hub = hub;
            _accountService = serviceProvider.GetRequiredService<IAccountService>();
        }

        [BindProperty]
      public Post Post { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            string username = Request.Cookies["Username"];

            var user = _accountService.GetAccountByUserName(username);
            var post = postService.GetPost(id);
            if (user.AccountId != post.CreateBy)
                return RedirectToPage("/PostManager/Index");

            if (id == null)
            {
                return NotFound();
            }

            

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
            string username = Request.Cookies["Username"];

            if (string.IsNullOrEmpty(username))
            {
                RedirectToPage("/User/Login");
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
