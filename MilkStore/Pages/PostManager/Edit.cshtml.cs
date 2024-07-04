﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using DAL;
using BusinessLogics.Services;
using DAL.Repository;

namespace MilkStore.Pages.PostManager
{
    public class EditModel : PageModel
    {
        private PostService postService;
        private IProductService _productService;
        private IAccountService _accountService;

        public EditModel(IServiceProvider serviceProvider)
        {
            postService = new PostService();
            _productService = serviceProvider.GetRequiredService<IProductService>();
            _accountService = serviceProvider.GetRequiredService<IAccountService>();
        }

        [BindProperty]
        public Post Post { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Post = postService.GetPost(id);
            if (Post == null)
            {
                return NotFound();
            }

            ViewData["CreateBy"] = new SelectList(_accountService.GetAccounts(), "AccountId", "Name");
            ViewData["PostStatuses"] = new SelectList(Enum.GetValues(typeof(PostStatuses)));
            ViewData["ProductId"] = new SelectList(_productService.GetAllProduct(), "ProductId", "Brand");

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Ensure the CreateDate is set correctly
                Post.CreateDate = DateTime.SpecifyKind(Post.CreateDate, DateTimeKind.Utc);

                // Ensure CreateBy is not modified but is valid for initial creation
                var existingPost = postService.GetPost(Post.PostId);
                if (existingPost != null)
                {
                    Post.CreateBy = existingPost.CreateBy; // Preserve the original CreateBy value
                }
                else
                {
                    if (_accountService.GetAccount(Post.CreateBy) == null)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid CreateBy value. Account does not exist.");
                        ViewData["CreateBy"] = new SelectList(_accountService.GetAccounts(), "AccountId", "Name");
                        ViewData["PostStatuses"] = new SelectList(Enum.GetValues(typeof(PostStatuses)));
                        ViewData["ProductId"] = new SelectList(_productService.GetAllProduct(), "ProductId", "Brand");
                        return Page();
                    }
                }

                postService.UpdatePost(Post);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(Post.PostId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
        private bool PostExists(int id)
        {
            return postService.GetPost(id) != null;
        }

    }    
}