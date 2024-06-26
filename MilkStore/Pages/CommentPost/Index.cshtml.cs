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

namespace MilkStore.Pages.CommentPost
{
    public class IndexModel : PageModel
    {
        private CommentService commentService;
        private PostService postService;

        public IndexModel()
        {
            commentService = new CommentService();
            postService = new PostService();
        }

        public IList<Comment> Comment { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Comment = commentService.GetComments();
            foreach (var comment in Comment)
            {
                comment.Post = postService.GetPost(comment.PostId);
            }
        }
    }
}
