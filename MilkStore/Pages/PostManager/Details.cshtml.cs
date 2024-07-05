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
using DAL.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MilkStore.Pages.PostManager
{
    public class DetailsModel : PageModel
    {
        private PostService postService;
        private IProductService _productService;
        private IAccountService accountService;
        private CommentService commentService;

        public DetailsModel(IServiceProvider serviceProvider)
        {
            postService = new PostService();
            _productService = serviceProvider.GetRequiredService<IProductService>();
            accountService = serviceProvider.GetRequiredService<IAccountService>();
            commentService = new CommentService();
        }

      public Post Post { get; set; } = default!;
        public IList<Comment> Comment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var post = postService.GetPost(id);
            if (post == null)
            {
                return NotFound();
            }
            post.Product = _productService.GetProduct(post.ProductId);
            
            Post = postService.GetPost(id);
            Comment = commentService.CommentInAPost(id);
            foreach (var comment in Comment)
            {
                comment.Post = postService.GetPost(comment.PostId);
            }
            return Page();
        }
    }
}
