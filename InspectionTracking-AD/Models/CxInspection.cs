using System.ComponentModel.DataAnnotations;

namespace InspectionTracking_AD.Models
{
    public class IxHeader
    {
        [Key] public int InspectionId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime InspectionDate { get; set; }
        public string AddressLine { get; set; }
        public int OrderNo { get; set; }
        public bool IsDone { get; set; }
        public int EnRoute { get; set; }

        public List<IxDetail> Details { get; set; }
    }

    public class IxDetail
    {
        [Key] public int DetailId { get; set; }
        public int InspectionId { get; set; }
        public string InspectionNo { get; set; }
        public string PermitNo { get; set; }
        public int? IVRNo { get; set; }
        public string? InspectionGroup { get; set; }
        public string? InspectionType { get; set; }

        public IxHeader Header { get; set; }
    }

    public class IxContact
    {
        [Key] public int ContactId { get; set; }
        public string EntityId { get; set; }
        public string InspectionNo { get; set; }
        public string ContactType { get; set; }
        public bool IsActive { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Email { get; set; }
    }

    public class Inspector
    {
        [Key] public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoUrl { get; set; }
        public string PhoneNo { get; set; }
    }
}
