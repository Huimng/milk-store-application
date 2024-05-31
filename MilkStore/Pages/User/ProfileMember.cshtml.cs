using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MilkStore.Pages.User
{
    [Authorize(Roles = "Member")]
    public class ProfileMemberModel : PageModel
    {
        public string Username { get; private set; }

        public void OnGet()
        {
            if (Request.Cookies.TryGetValue("Username", out var username))
            {
                Username = username;
            }
            else
            {
                Username = "Guest";
            }
        }
    }
}
