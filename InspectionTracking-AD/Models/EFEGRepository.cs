namespace InspectionTracking_AD.Models
{
    public class EFEGRepository : IEGRepository
    {
        private EGDBContext context;

        public EFEGRepository(EGDBContext context)
        {
            this.context = context;
        }

        public IQueryable<EGInspection> EGInspections => context.EGInspections;
        public IQueryable<InspectionContact> InspectionContacts => context.InspectionContacts;
    }
}
