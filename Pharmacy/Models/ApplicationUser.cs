using Microsoft.AspNetCore.Identity;

namespace Pharmacy.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
