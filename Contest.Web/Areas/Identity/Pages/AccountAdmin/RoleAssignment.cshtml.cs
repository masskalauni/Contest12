using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Contest.Web.Areas.Identity.Pages.AccountAdmin
{
    public class RoleAssignmentModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RoleAssignmentModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public Dictionary<string, string> Users { get; set; }

        public void OnGet()
        {
            Users = _userManager.Users.ToDictionary(x => x.Id, y => y.UserName);
        }
    }
}