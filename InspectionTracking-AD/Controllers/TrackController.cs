using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InspectionTracking_AD.Models;
using Microsoft.EntityFrameworkCore;

namespace InspectionTracking_AD.Controllers
{
    [AllowAnonymous]
    public class TrackController : Controller
    {
        private ICxRepository repository;
        public TrackController(ICxRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Index(string permit)
        {
            if (string.IsNullOrEmpty(permit))
                return View();

            IxDetail? inspection = repository.IxDetails
                                    .Include(i => i.Header)
                                    .FirstOrDefault(i => i.Header.InspectionDate == DateTime.Today &&
                                                        (i.PermitNo == permit || i.IVRNo.ToString() == permit));

            ViewBag.Permit = permit;
            if (inspection == null)
            {
                ModelState.AddModelError("", "No inspection scheduled for today.");
            }
            else if (inspection.Header.IsDone)
            {
                ModelState.AddModelError("", "Inspection already completed for today.");
            }

            if (ModelState.IsValid)
            {
                ViewBag.Address = inspection?.Header.AddressLine;
                return View("Result",
                            repository.IxDetails
                                .Include(i => i.Header)
                                .Where(i => i.Header.InspectionDate == DateTime.Today &&
                                            (i.PermitNo == permit || i.IVRNo.ToString() == permit))
                                .Select(i => i.Header.UserId)
                                .Distinct());
            }
            else
                return View();
        }
    }
}
