using Microsoft.AspNetCore.Identity;
using Pharmacy.Models;

namespace Pharmacy.Data
{
    public class SeedingData
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager,ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Roles.Any())
                {
                    IEnumerable<IdentityRole> roles = new List<IdentityRole>()
                    {
                        new IdentityRole{Name =Roles.Customer.ToString(),NormalizedName =Roles.Customer.ToString().ToUpper()},
                        new IdentityRole{Name =Roles.Pharmacy.ToString(),NormalizedName =Roles.Pharmacy.ToString().ToUpper()},
                    };
                    await context.Roles.AddRangeAsync(roles);
                    await context.SaveChangesAsync();
                }
                if (!context.Users.Any())
                {
                    var customer = new ApplicationUser()
                    {
                        Name ="Customer",
                        Email ="customer@customer.com",
                        PhoneNumber ="01153988188",
                        UserName = "customer@customer.com"

                    };
                    var pharmacy = new ApplicationUser()
                    {
                        Name = "Pharmacy",
                        Email = "pharmacy@pharmacy.com",
                        PhoneNumber = "01153988188",
                        UserName = "pharmacy@pharmacy.com"
                    };
                    var password = "Pa$$w0rd";

                    var customerResult = await userManager.CreateAsync(customer, password);
                    if (customerResult.Succeeded)
                        await userManager.AddToRoleAsync(customer, Roles.Customer.ToString());

                    var pharmacyResult = await userManager.CreateAsync(pharmacy, password);
                    if (pharmacyResult.Succeeded)
                        await userManager.AddToRoleAsync(pharmacy,Roles.Pharmacy.ToString());
                }
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<ApplicationDbContext>();
                logger.LogError(e.Message);
            }
        }
    }
}
