using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kolpi.Web.Areas.Identity.Pages.AccountAdmin
{
    public class AddRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddRoleModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public void OnGet()
        {            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var role = new IdentityRole { Name = Input.Name };
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                ModelState.AddModelError("Role Create Error", result.Errors.SelectMany(x => x.Description).ToString());
            return RedirectToPage("roles");
        }

        public class InputModel
        {
            public string Id { get; set; }

            [Required]
            public string Name { get; set; }
        }

        public IList<InputModel> Roles { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
    }
}