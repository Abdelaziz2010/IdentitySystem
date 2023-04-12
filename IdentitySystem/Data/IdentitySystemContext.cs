using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using IdentitySystem.Models;
namespace IdentitySystem.Data
{
    public class IdentitySystemContext : IdentityDbContext
    {
        public IdentitySystemContext (DbContextOptions<IdentitySystemContext> options)
            : base(options)
        {
        }

        public DbSet<IdentitySystem.Models.Course> Course { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
