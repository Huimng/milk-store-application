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
using Microsoft.AspNetCore.SignalR;
using MilkStore.Hubs;

namespace MilkStore.Pages.PostManager
{
    public class IndexModel : PageModel
    {
        private readonly PostService postService;
        private readonly IProductService _productService;
        private readonly IAccountService _accountService;
        private readonly IHubContext<ChatHub> _hub;
        public IndexModel(IServiceProvider serviceProvider, IHubContext<ChatHub> hub)
        {
            postService = new PostService();
            _productService = serviceProvider.GetRequiredService<IProductService>();
            _accountService = serviceProvider.GetRequiredService<IAccountService>();
            _hub = hub;
        }

        public List<Post> ListPost { get;set; } = default;

        public async Task OnGetAsync()
        {
            if (postService != null)
            {
                await _hub.Clients.All.SendAsync("Loading");
                ListPost = postService.GetPosts();
                foreach (Post post in ListPost)
                {
                    post.Account = _accountService.GetAccount(post.CreateBy);
                    post.Product = _productService.GetProduct(post.ProductId);
                    
                }
            }
            await _hub.Clients.All.SendAsync("Loading");
        }
    }
}
