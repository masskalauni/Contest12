using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Contest.Web.Areas.Identity.Pages.AccountAdmin
{
    public class AssignRoleModel : PageModel
    {

        private readonly UserManager<IdentityUser> _userManager;

        public AssignRoleModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task OnGet(string userId)
        {
            IdentityUser user;
            if (!string.IsNullOrWhiteSpace(userId))
            {
                user = await _userManager.FindByIdAsync(userId);
                UserId = user.Id;
                UserName = user.UserName;
                UserRoles = await _userManager.GetRolesAsync(user);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrWhiteSpace(UserId) && !string.IsNullOrWhiteSpace(UserRolesFlat))
            {
                var user = await _userManager.FindByIdAsync(UserId);
                UserRoles = UserRolesFlat.Split(",");

                foreach (var role in UserRoles)
                {
                    if (!await _userManager.IsInRoleAsync(user, role))
                    {
                        var result = await _userManager.AddToRoleAsync(user, role);
                        if (!result.Succeeded)
                            continue;
                    }
                }

                return RedirectToPage("RoleAssignment");
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public string UserName { get; set; }
        public IList<string> UserRoles { get; set; }

        [BindProperty]
        public string UserRolesFlat { get; set; }

        [BindProperty]
        public string UserId { get; set; }
    }
}