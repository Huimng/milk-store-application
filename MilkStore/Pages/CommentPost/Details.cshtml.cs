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
    public class DetailsModel : PageModel
    {
        private CommentService commentService;

        public DetailsModel()
        {
            commentService = new CommentService();
        }

      public Comment Comment { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null || commentService == null)
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
    }
}
