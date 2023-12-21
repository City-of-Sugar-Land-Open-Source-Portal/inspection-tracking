using Microsoft.AspNetCore.Mvc;
using InspectionTracking_AD.Models;

namespace InspectionTracking_AD.Components
{
    public class StepperViewComponent : ViewComponent
    {
        private ICxRepository repository;

        public StepperViewComponent(ICxRepository repo)
        {
            this.repository = repo;
        }

        public IViewComponentResult Invoke(string user)
        {

            var list = repository.IxHeaders
                            .Where(i => i.UserId == user && i.InspectionDate == DateTime.Today)
                            .OrderBy(i => i.OrderNo);
            var next = list.FirstOrDefault(i => i.IsDone == false);
            string address = ViewBag.Address;
            var customer = list.FirstOrDefault(i => i.AddressLine == address);
            return View(new StepperViewModel
            {
                Inspector = repository.Inspectors.FirstOrDefault(i => i.UserId == user),
                Inspections_Left = customer.OrderNo - next.OrderNo,
                Inspections = list
            });
        }
    }
}
