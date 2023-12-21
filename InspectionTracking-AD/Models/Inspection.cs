using System.ComponentModel.DataAnnotations;

namespace InspectionTracking_AD.Models
{
    public class EGInspection
    {
        [Key] public string InspectionId { get; set; }
        public string InspectionNumber { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public string? Type { get; set; }
        public string? TypeGroup { get; set; }
        public string? AssignedInspectorUserName { get; set; }
        public string? ParentCaseNumber { get; set; }
        public int? IvrNo { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string? StreetType { get; set; }
        public string? PostDirection { get; set; }
        public string? PreDirection { get; set; }
        public string? UnitOrSuite { get; set; }

        public string AddressLine
        {
            get
            {
                string addr = "";
                if (!string.IsNullOrWhiteSpace(AddressLine1))
                    addr += AddressLine1;
                if (!string.IsNullOrWhiteSpace(PreDirection))
                    addr += " " + PreDirection;
                if (!string.IsNullOrWhiteSpace(AddressLine2))
                    addr += " " + AddressLine2.TrimEnd();
                if (!string.IsNullOrWhiteSpace(StreetType))
                    addr += " " + StreetType;
                if (!string.IsNullOrWhiteSpace(AddressLine3))
                    addr += " " + AddressLine3.TrimEnd();
                if (!string.IsNullOrWhiteSpace(PostDirection))
                    addr += " " + PostDirection;
                if (!string.IsNullOrWhiteSpace(UnitOrSuite))
                    addr += ", " + UnitOrSuite;

                return addr;
            }
        }
    }

    public class InspectionContact
    {
        [Key] public string GLOBALENTITYID { get; set; }
        public string INSPECTIONNUMBER { get; set; }
        public string? CONTACTTYPE { get; set; }
        public bool ISACTIVE { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string? EMAIL { get; set; }
        public string? BUSINESSPHONE { get; set; }
        public string? HOMEPHONE { get; set; }
        public string? MOBILEPHONE { get; set; }
        public string? OTHERPHONE { get; set; }
    }
}
