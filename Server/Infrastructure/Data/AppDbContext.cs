using Microsoft.EntityFrameworkCore;
using API.Domain.Entities.UserManagement;

namespace API.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {  }

        public DbSet<User> Users { get; set; }
    }
}
