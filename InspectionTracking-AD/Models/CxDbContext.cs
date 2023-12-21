using Microsoft.EntityFrameworkCore;

namespace InspectionTracking_AD.Models
{
    public class CxDbContext : DbContext
    {
        public CxDbContext(DbContextOptions<CxDbContext> options) : base(options) { }

        public DbSet<IxHeader> IxHeaders => Set<IxHeader>();
        public DbSet<IxDetail> IxDetails => Set<IxDetail>();
        public DbSet<IxContact> IxContacts => Set<IxContact>();
        public DbSet<Inspector> Inspectors => Set<Inspector>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IxDetail>()
                .HasOne(d => d.Header)
                .WithMany(h => h.Details)
                .HasForeignKey(d => d.InspectionId);
        }
    }
}
