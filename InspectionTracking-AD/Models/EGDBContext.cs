using Microsoft.EntityFrameworkCore;

namespace InspectionTracking_AD.Models
{
    public class EGDBContext : DbContext
    {
        public EGDBContext(DbContextOptions<EGDBContext> options)
            : base(options) { }

        public DbSet<EGInspection> EGInspections => Set<EGInspection>();
        public DbSet<InspectionContact> InspectionContacts => Set<InspectionContact>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EGInspection>()
                .ToView("INSPECTIONSUMMARYINFO")
                .HasKey(i => i.InspectionId);
            modelBuilder.Entity<InspectionContact>()
                .ToView("INSPECTIONCONTACTSUMMARYINFO")
                .HasKey(c => c.GLOBALENTITYID);
        }
    }
}
