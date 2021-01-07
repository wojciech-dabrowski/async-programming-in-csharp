using System.Data.Entity;

namespace AsyncProgramming.DesktopApp.Data
{
    public class DesktopAppDbContext : DbContext
    {
        public IDbSet<SomeEntity> SomeEntities { get; set; }
    }
}
