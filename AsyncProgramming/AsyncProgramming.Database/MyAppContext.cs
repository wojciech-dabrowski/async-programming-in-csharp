using Microsoft.EntityFrameworkCore;

namespace AsyncProgramming.Database
{
    public class MyAppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    }
}
