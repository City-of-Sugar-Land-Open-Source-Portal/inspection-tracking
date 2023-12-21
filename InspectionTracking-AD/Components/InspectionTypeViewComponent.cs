using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InspectionTracking_AD.Models;

namespace InspectionTracking_AD.Components
{
    public class InspectionTypeViewComponent : ViewComponent
    {
        private ICxRepository repository;

        public InspectionTypeViewComponent(ICxRepository repository)
        {
            this.repository = repository;
        }

        public IViewComponentResult Invoke(string user, string permit)
        {
            IxDetail? result = repository.IxDetails
                                .Include(i => i.Header)
                                .FirstOrDefault(i => i.Header.UserId == user && i.Header.InspectionDate == DateTime.Today &&
                                                    (i.PermitNo == permit || i.IVRNo.ToString() == permit));
            return View(new InspectorTypeViewModel
            {
                InspectType = result?.InspectionGroup,
                Address = result?.Header.AddressLine,
                PermitNo = result?.PermitNo
            });
        }
    }
}
