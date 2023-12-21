namespace InspectionTracking_AD.Models
{
    public interface IEGRepository
    {
        IQueryable<EGInspection> EGInspections { get; } 
        IQueryable<InspectionContact> InspectionContacts { get; }
    }
}
