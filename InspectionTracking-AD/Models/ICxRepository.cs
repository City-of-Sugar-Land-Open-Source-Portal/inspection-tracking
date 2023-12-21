namespace InspectionTracking_AD.Models
{
    public interface ICxRepository
    {
        IQueryable<IxHeader> IxHeaders { get; }
        IQueryable<IxDetail> IxDetails { get; }
        IQueryable<IxContact> IxContacts { get; }
        IQueryable<Inspector> Inspectors { get; }

        IxHeader AddInspection(IxHeader inspection);
        void AddDetail(IxDetail detail);
        void AddContact(IxContact contact);
        void InitialSort(string user);
        void CheckRoute(string user);
        void SaveOrder(int inspectionId, int order);
        void EnRoute(int inspectionId);
        void Done(int inspectionId);
    }
}
