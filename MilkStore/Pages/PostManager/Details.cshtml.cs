using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects;
using BusinessLogics.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
using MilkStore.Hubs;

namespace MilkStore.Pages.PostManager
{
    public class DetailsModel : PageModel
    {
        private readonly PostService postService;
        private readonly IProductService _productService;
        private readonly IAccountService accountService;
        private readonly CommentService commentService;
        private readonly IHubContext<ChatHub> _hub;

        public DetailsModel(IServiceProvider serviceProvider, IHubContext<ChatHub> hub)
        {
            postService = new PostService();
            _productService = serviceProvider.GetRequiredService<IProductService>();
            accountService = serviceProvider.GetRequiredService<IAccountService>();
            commentService = new CommentService();
            _hub = hub;
        }

        public Post Post { get; set; } = default!;
        public IList<Comment> Comments { get; set; } = default!;
        [BindProperty]
        public Comment CommentIndex { get; set; } = new Comment();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var post = postService.GetPost(id);
            if (post == null)
            {
                return NotFound();
            }
            post.Product = _productService.GetProduct(post.ProductId);

            Post = post;
            Comments = commentService.CommentInAPost(id);
            foreach (var comment in Comments)
            {
                comment.Post = postService.GetPost(comment.PostId);
            }

            await _hub.Clients.All.SendAsync("Loading");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int postId)
        {
            if (!ModelState.IsValid)
            {
                var post = postService.GetPost(postId);
                if (post == null)
                {
                    return NotFound();
                }
                post.Product = _productService.GetProduct(post.ProductId);

                Post = post;
                Comments = commentService.CommentInAPost(postId);
                foreach (var comment in Comments)
                {
                    comment.Post = postService.GetPost(comment.PostId);
                }
                return Page();
            }

            

            CommentIndex.PostId = postId;
            CommentIndex.CreateDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

            commentService.Comment(CommentIndex);
             _hub.Clients.All.SendAsync("Loading");

            return RedirectToPage(new { id = postId });
        }
    }
}
