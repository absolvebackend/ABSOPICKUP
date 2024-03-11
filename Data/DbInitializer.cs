using _AbsoPickUp.IServices;
using _AbsoPickUp.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace _AbsoPickUp.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext _context,
           UserManager<UserDetails> _userManager,
           RoleManager<IdentityRole> _roleManager,
           IFunctionalService _functionalService)
        {
            _context.Database.EnsureCreated();

            //check for users
            if (_context.UserDetails.Any())
            {
                return; //if user is not empty, DB has been seed
            }

            //init app with super admin user
            await _functionalService.CreateDefaultSuperAdmin();
        }
    }
}
