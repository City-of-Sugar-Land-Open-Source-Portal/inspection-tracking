using InspectionTracking_AD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.EntityFrameworkCore;

namespace InspectionTracking_AD.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ICxRepository repository;

        public HomeController(ILogger<HomeController> logger, ICxRepository repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        public IActionResult Index()
        {
            var list = repository.IxHeaders
                            .Where(i => i.InspectionDate == DateTime.Today &&
                                        i.UserId.ToUpper() == User.Identity.Name.ToUpper());
            if (list == null || list.Count() == 0)
                return RedirectToAction("List", "EG");
            else
                return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Reorder(int id, int idx)
        {
            repository.SaveOrder(id, idx);
            return Ok();
        }

        [HttpPost]
        public ActionResult Route()
        {
            repository.CheckRoute(User?.Identity.Name);
            return Ok();
        }

        [HttpPost]
        public IActionResult EnRoute(int id, string time)
        {
            repository.EnRoute(id);

            var details = repository.IxDetails
                            .Include(d => d.Header)
                            .Where(d => d.InspectionId == id);
            List<string> emails = new List<string>();
            EmailNotifier notifier = new EmailNotifier();
            foreach (IxDetail detail in details)
            {
                var contacts = repository.IxContacts.Where(c => c.InspectionNo == detail.InspectionNo &&
                                    (c.ContactType == "Owner" || c.IsActive == true));
                foreach (IxContact contact in contacts)
                    if (contact.Email is not null)
                        emails.Add(contact.Email);

                notifier.SendEmail(time, detail, emails.Distinct());
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public ActionResult Done(int id)
        {
            repository.Done(id);
            return Ok();
        }

        public ActionResult Inspection_Read([DataSourceRequest] DataSourceRequest request)
        {
            var list = repository.IxHeaders
                            .Where(i => i.InspectionDate == DateTime.Today &&
                                        i.UserId.ToUpper() == User.Identity.Name.ToUpper())
                            .OrderBy(i => i.OrderNo);
            return Json(list.ToDataSourceResult(request));
        }

        public ActionResult Detail_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            return Json(repository.IxDetails
                            .Where(d => d.InspectionId == id)
                            .OrderBy(d => d.PermitNo).ThenBy(d => d.InspectionNo)
                            .ToDataSourceResult(request));
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}