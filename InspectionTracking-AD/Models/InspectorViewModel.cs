namespace InspectionTracking_AD.Models
{
    public class InspectorViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
    }

    public class InspectorTypeViewModel
    {
        public string InspectType { get; set; }
        public string Address { get; set; }
        public string PermitNo { get; set; }
    }

    public class StepperViewModel
    {
        public Inspector Inspector { get; set; }
        public int Inspections_Left { get; set; }
        public IEnumerable<IxHeader> Inspections { get; set; }
    }
}
