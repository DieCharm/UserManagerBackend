using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UserManager.Auth.Authentication
{
    public class IdentityContext : 
        IdentityDbContext<IdentityUser>

    {
        public IdentityContext(DbContextOptions<IdentityContext> options) 
            : base(options)
        {
        }
        
        /*public IdentityContext()
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=JACKFLASHPC;Database=IdentityDatabase;Trusted_Connection=True;");
        }*/
    }
}