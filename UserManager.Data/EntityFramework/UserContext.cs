using Microsoft.EntityFrameworkCore;

namespace UserManager.Data.EntityFramework
{
    public class UserContext : 
        DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) :
            base(options)
        { }

        /*public UserContext()
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=JACKFLASHPC;Database=UsersDatabase;Trusted_Connection=True;");
        }*/
        
        public DbSet<User> Users { get; set; }
    }
}