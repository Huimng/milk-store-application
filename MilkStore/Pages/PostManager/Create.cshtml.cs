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
using DAL.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace MilkStore.Pages.PostManager
{
    public class CreateModel : PageModel
    {
        private PostService postService;
        private AccountRepository accountRepository;
        private ProductRepository productRepository;
        private IProductService _productService;
        private IAccountService _accountService;


        public CreateModel(IServiceProvider serviceProvider)
        {
            postService = new PostService();
            accountRepository = new AccountRepository();
            productRepository = new ProductRepository();
            _productService = serviceProvider.GetRequiredService<IProductService>();
            _accountService = serviceProvider.GetRequiredService<IAccountService>();
        }

        public IActionResult OnGet()
        {
            ViewData["CreateBy"] = new SelectList(_accountService.GetAccounts(), "AccountId", "Name");
            ViewData["ProductId"] = new SelectList(_productService.GetAllProduct(), "ProductId", "Brand");
            return Page();
        }

        [BindProperty]
        public Post Post { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Post == null)
            {
                return Page();
            }
            var username = Request.Cookies["Username"];
            int id = _accountService.GetAccountByUserName(username).AccountId;
            Post.CreateBy = id;
            Post.Status = PostStatuses.Pending;
            Post.CreateDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            postService.AddPost(Post);

            return RedirectToPage("./Index");
        }
    }
}
