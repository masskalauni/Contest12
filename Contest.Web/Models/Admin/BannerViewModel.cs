using Microsoft.AspNetCore.Http;

namespace Contest.Web.Models.Admin
{
    public class BannerViewModel
    {
        public string Name { get; set; }
        public IFormFile Banner { get; set; }
    }
}
