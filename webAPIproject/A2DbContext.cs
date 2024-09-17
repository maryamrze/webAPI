using Microsoft.EntityFrameworkCore;
using A2.Models;

namespace A2.Data
{
    public class A2DbContext : DbContext
    {
        public A2DbContext(DbContextOptions<A2DbContext> options) : base(options) { }
        public DbSet<Events> Events { get; set; }
        public DbSet<Organizers> Organizers { get; set; }
        public DbSet<Signs> Signs { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Progress> Progresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=A2Database.sqlite");
        }
    }
}