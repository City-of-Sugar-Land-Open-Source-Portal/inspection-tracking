namespace InspectionTracking_AD.Models
{
    public class EFCxRepository : ICxRepository
    {
        private CxDbContext context;
        public EFCxRepository(CxDbContext context)
        {
            this.context = context;
        }

        public IQueryable<IxHeader> IxHeaders => context.IxHeaders;
        public IQueryable<IxDetail> IxDetails => context.IxDetails;
        public IQueryable<IxContact> IxContacts => context.IxContacts;
        public IQueryable<Inspector> Inspectors => context.Inspectors;

        public IxHeader AddInspection(IxHeader inspection)
        {
            IxHeader? dbRecord = context.IxHeaders
                                    .FirstOrDefault(i => i.AddressLine == inspection.AddressLine &&
                                                        i.InspectionDate == inspection.InspectionDate &&
                                                        i.UserName == inspection.UserName);
            if (dbRecord == null)
            {
                context.IxHeaders.Add(inspection);
                context.SaveChanges();
                return inspection;
            }
            else
                return dbRecord;
        }

        public void AddDetail(IxDetail detail)
        {
            context.IxDetails.Add(detail);
            context.SaveChanges();
        }

        public void AddContact(IxContact contact)
        {
            context.IxContacts.Add(contact);
            context.SaveChanges();
        }

        public void InitialSort(string user)
        {
            var list = context.IxHeaders
                        .Where(i => i.UserId == user && i.InspectionDate == DateTime.Today)
                        .OrderBy(i => i.AddressLine);
            int index = 0;
            foreach (var item in list)
                item.OrderNo = index++;
            context.SaveChanges();
            CheckRoute(user);
        }

        public void CheckRoute(string user)
        {
            var list = context.IxHeaders
                        .Where(i => i.UserId == user && i.InspectionDate == DateTime.Today)
                        .OrderBy(i => i.OrderNo);
            foreach (var item in list)
                if (!item.IsDone)
                {
                    item.EnRoute = 1;
                    break;
                }
            context.SaveChanges();
        }

        public void SaveOrder(int inspectionId, int order)
        {
            IxHeader? dbRecord = context.IxHeaders.FirstOrDefault(i => i.InspectionId == inspectionId);
            if (dbRecord != null)
            {
                dbRecord.OrderNo = order;
                dbRecord.EnRoute = 0;
                context.SaveChanges();
            }
        }

        public void EnRoute(int inspectionId)
        {
            IxHeader? dbRecord = context.IxHeaders.FirstOrDefault(x => x.InspectionId == inspectionId);
            if (dbRecord != null)
            {
                dbRecord.EnRoute = 2;
                context.SaveChanges();
            }
        }

        public void Done(int inspectionId)
        {
            IxHeader? dbRecord = context.IxHeaders.FirstOrDefault(x => x.InspectionId == inspectionId);
            if (dbRecord != null)
            {
                dbRecord.EnRoute = 0;
                dbRecord.IsDone = true;
                context.SaveChanges();

                CheckRoute(dbRecord.UserId);
            }
        }
    }
}
