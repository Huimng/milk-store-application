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
    public class IndexModel : PageModel
    {
        private CommentService commentService;
        private PostService postService;
        private readonly IHubContext<ChatHub> _hub;
        public IndexModel(IHubContext<ChatHub> hub)
        {
            commentService = new CommentService();
            postService = new PostService();
            _hub = hub;
        }

        public IList<Comment> Comment { get;set; } = default!;

        public async Task OnGetAsync()
        {
            await _hub.Clients.All.SendAsync("Loading");
            Comment = commentService.GetComments();
            foreach (var comment in Comment)
            {
                comment.Post = postService.GetPost(comment.PostId);
            }
            await _hub.Clients.All.SendAsync("Loading");
        }
    }
}
