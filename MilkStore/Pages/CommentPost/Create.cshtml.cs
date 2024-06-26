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

namespace MilkStore.Pages.CommentPost
{
    public class CreateModel : PageModel
    {
        private CommentService commentService;
        private PostService postService;

        public CreateModel()
        {
            commentService = new CommentService();
            postService = new PostService();
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
            Comment.CreateDate = DateTime.UtcNow;
            commentService.Comment(Comment);
            

            return RedirectToPage("./Index");
        }
    }
}
