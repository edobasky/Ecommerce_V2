using Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data
{
    public class AppidentityDBContext : IdentityDbContext<ApplicationUser>
    {
        public AppidentityDBContext(DbContextOptions<AppidentityDBContext> options) : base(options)
        {
        }

        protected AppidentityDBContext()
        {
        }
    }
}
