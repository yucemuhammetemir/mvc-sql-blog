using blogv1.Models;
using Microsoft.EntityFrameworkCore;

namespace blogv1.Context
{
    public class BlogDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source =NB3\\SQLEXPRESS; database=blogV1; Integrated Security=True; TrustServerCertificate=True;");
        }
        
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public DbSet<Contact> Contacts { get; set; }

    }
}
