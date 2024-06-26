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

namespace MilkStore.Pages.PostManager
{
    public class IndexModel : PageModel
    {
        private readonly PostService postService;
        private readonly IProductService _productService;
        private readonly IAccountService _accountService;

        public IndexModel(IServiceProvider serviceProvider)
        {
            postService = new PostService();
            _productService = serviceProvider.GetRequiredService<IProductService>();
            _accountService = serviceProvider.GetRequiredService<IAccountService>();
        }

        public List<Post> ListPost { get;set; } = default;

        public async Task OnGetAsync()
        {
            if (postService != null)
            {
                ListPost = postService.GetPosts();
                foreach (Post post in ListPost)
                {
                    post.Account = _accountService.GetAccount(post.CreateBy);
                    post.Product = _productService.GetProduct(post.ProductId);
                    
                }
            }
        }
    }
}
