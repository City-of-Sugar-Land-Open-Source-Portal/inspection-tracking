using Microsoft.AspNetCore.Mvc;
using InspectionTracking_AD.Models;

namespace InspectionTracking_AD.Components
{
    public class InspectorViewComponent : ViewComponent
    {
        private ICxRepository repository;

        public InspectorViewComponent(ICxRepository repository)
        {
            this.repository = repository;
        }

        public IViewComponentResult Invoke(string? user)
        {
            Inspector? inspector = null;
            if (user != null)
                inspector = repository.Inspectors.FirstOrDefault(i => i.UserId.ToUpper() == user.ToUpper());
            return View(inspector);
        }
    }
}
