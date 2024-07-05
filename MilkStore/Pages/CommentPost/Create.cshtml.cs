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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
using MilkStore.Hubs;

namespace MilkStore.Pages.CommentPost
{
    public class CreateModel : PageModel
    {
        private CommentService commentService;
        private PostService postService;
        private IAccountService _accountService;
        private readonly IHubContext<ChatHub> _hub;

        public CreateModel(IServiceProvider serviceProvider, IHubContext<ChatHub> hub)
        {
            commentService = new CommentService();
            postService = new PostService();
            _accountService = serviceProvider.GetRequiredService<IAccountService>();
            _hub = hub;
        }

        public IActionResult OnGet()
        {
            ViewData["PostId"] = new SelectList(postService.GetPosts(), "PostId", "Title");

            return Page();
        }

        [BindProperty]
        public Comment Comment { get; set; } = default!;
        

        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return Page();
            }
            var username = Request.Cookies["Username"];
            int id = _accountService.GetAccountByUserName(username).AccountId;
            Comment.CreateDate = DateTime.UtcNow;
            commentService.Comment(Comment);
            await _hub.Clients.All.SendAsync("Loading");

            return RedirectToPage("./Index");
        }
    }
}
