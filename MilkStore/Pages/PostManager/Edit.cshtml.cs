using System;
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
        private readonly BSADBContext _context;

        public EditModel(IServiceProvider serviceProvider)
        {
            postService = new PostService();
            _productService = serviceProvider.GetRequiredService<IProductService>();
            _accountService = serviceProvider.GetRequiredService<IAccountService>();
            _context = new BSADBContext();
        }

        [BindProperty]
        public Post Post { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Post = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(m => m.PostId == id);
            ViewData["CreateBy"] = new SelectList(_accountService.GetAccounts(), "AccountId", "Name");
            ViewData["PostStatuses"] = new SelectList(Enum.GetValues(typeof(PostStatuses)));
            ViewData["ProductId"] = new SelectList(_productService.GetAllProduct(), "ProductId", "Brand");

            if (Post == null)
            {
                return NotFound();
            }

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

            var existingPost = await _context.Posts.FindAsync(Post.PostId);
            if (existingPost != null)
            {
                _context.Entry(existingPost).State = EntityState.Detached;
            }
            _context.Attach(Post).State = EntityState.Modified;
            _context.Entry(Post).Property(x => x.CreateDate).IsModified = false;
            _context.Entry(Post).Property(x => x.CreateBy).IsModified = false;
            try
            {
                await _context.SaveChangesAsync();
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
            return _context.Posts.Any(e => e.PostId == id);
        }

    }    
}