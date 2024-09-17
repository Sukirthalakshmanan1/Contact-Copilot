
using ContactManagementCopilot.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManagementCopilot.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
           
        }
         public virtual DbSet<ContactDetails> ContactDetails { get; set; } 
    //      protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<ContactDetails>().ToTable("ContactDetails");
    //     }
    }
}
