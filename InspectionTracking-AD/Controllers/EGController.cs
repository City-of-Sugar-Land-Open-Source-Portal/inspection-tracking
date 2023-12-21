using InspectionTracking_AD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net.WebSockets;

namespace InspectionTracking_AD.Controllers
{
    [Authorize]
    public class EGController : Controller
    {
        private IEGRepository repositoryEG;
        private ICxRepository repositoryCx;
        public EGController(IEGRepository repo1, ICxRepository repo2)
        {
            repositoryEG = repo1;
            repositoryCx = repo2;
        }

        public IActionResult Inspectors()
        {
            if (User.Identity?.Name.ToUpper() == "WNGANTIAN@SUGARLANDTX.GOV")
                return View();
            else
                return RedirectToAction("List", "EG");
        }

        public IActionResult List(string? user, DateTime? date)
        {
            if (user == null)
                user = User.Identity?.Name;
            if (date == null)
                date = DateTime.Today;
            ViewBag.UserId = user;
            ViewBag.SchedDate = date;

            var result = repositoryEG.EGInspections
                            .Where(e => e.ScheduleDate == date && e.AssignedInspectorUserName.ToUpper() == user.ToUpper());
            return View(result.OrderBy(e => e.InspectionNumber));
        }

        [HttpPost]
        public IActionResult Download(string user, DateTime date)
        {
            Inspector? inspector = repositoryCx.Inspectors.FirstOrDefault(i => i.UserId.ToUpper() == user.ToUpper());
            if (inspector == null)
                return RedirectToAction("Inspectors", "EG");

            var result = repositoryEG.EGInspections
                            .Where(e => e.ScheduleDate == date && e.AssignedInspectorUserName.ToUpper() == user.ToUpper());

            int index = 0;
            foreach (EGInspection i in result)
            {
                IxHeader record = repositoryCx.AddInspection(
                    new IxHeader
                    {
                        UserId = i.AssignedInspectorUserName,
                        UserName = inspector.FirstName + " " + inspector.LastName,
                        InspectionDate = i.ScheduleDate.Value,
                        AddressLine = i.AddressLine,
                        OrderNo = index

                    });
                repositoryCx.AddDetail(
                    new IxDetail
                    {
                        InspectionId = record.InspectionId,
                        PermitNo = i.ParentCaseNumber,
                        InspectionNo = i.InspectionNumber,
                        IVRNo = i.IvrNo,
                        InspectionGroup = i.TypeGroup,
                        InspectionType = i.Type
                    });

                var contacts = repositoryEG.InspectionContacts.Where(c => c.INSPECTIONNUMBER == i.InspectionNumber);
                foreach (InspectionContact c in contacts)
                    repositoryCx.AddContact(
                        new IxContact
                        {
                            EntityId = c.GLOBALENTITYID,
                            InspectionNo = c.INSPECTIONNUMBER,
                            ContactType = c.CONTACTTYPE,
                            IsActive = c.ISACTIVE,
                            FirstName = c.FIRSTNAME,
                            LastName = c.LASTNAME,
                            Email = c.EMAIL,
                            Phone1 = string.IsNullOrWhiteSpace(c.BUSINESSPHONE) ? c.HOMEPHONE : c.BUSINESSPHONE,
                            Phone2 = string.IsNullOrWhiteSpace(c.MOBILEPHONE) ? c.OTHERPHONE : c.MOBILEPHONE
                        });

                index++;
            }

            return RedirectToAction("Index", "Home");
        }

        public JsonResult GetInspectors()
            => Json(repositoryCx.Inspectors.ToList());
    }
}
