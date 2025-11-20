using Microsoft.AspNetCore.Mvc;

namespace AcademicClaimsPortal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Role selection actions
        public IActionResult SelectRole(string role)
        {
            TempData["SelectedRole"] = role;

            return role switch
            {
                "lecturer" => RedirectToAction("Create", "Claim"),
                "coordinator" => RedirectToAction("PendingClaims", "Coordinator"),
                "hr" => RedirectToAction("Dashboard", "HR"),
                _ => RedirectToAction("Index")
            };
        }
    }
}