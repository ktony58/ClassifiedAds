using Microsoft.EntityFrameworkCore;
using Model.Model;

namespace Model
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
