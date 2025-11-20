using Microsoft.AspNetCore.Mvc;
using AcademicClaimsPortal.Models;
using AcademicClaimsPortal.Services;

namespace AcademicClaimsPortal.Controllers
{
    public class HRController : Controller
    {
        // GET: /HR/Dashboard
        public IActionResult Dashboard()
        {
            var allClaims = DataService.Claims.ToList();

            var stats = new
            {
                TotalClaims = allClaims.Count,
                PendingClaims = allClaims.Count(c => c.Status == "Pending"),
                ApprovedClaims = allClaims.Count(c => c.Status.Contains("Approved")),
                TotalAmount = allClaims.Where(c => c.Status.Contains("Approved"))
                                     .Sum(c => (double)c.HoursWorked * (double)c.HourlyRate)
            };

            return View(stats);
        }

        // GET: /HR/AllClaims
        public IActionResult AllClaims()
        {
            var allClaims = DataService.Claims.OrderByDescending(c => c.SubmissionDate).ToList();
            return View(allClaims);
        }

        // GET: /HR/GenerateReport
        public IActionResult GenerateReport(string reportType = "Monthly")
        {
            var claims = DataService.Claims.ToList();

            // Calculate total amount manually
            var totalAmount = claims.Where(c => c.Status.Contains("Approved"))
                                   .Sum(c => (double)c.HoursWorked * (double)c.HourlyRate);

            var reportData = new
            {
                ReportType = reportType,
                GeneratedDate = DateTime.Now,
                TotalClaims = claims.Count,
                TotalAmount = totalAmount,
                ClaimsByStatus = claims.GroupBy(c => c.Status).ToDictionary(g => g.Key, g => g.Count()),
                ClaimsByDepartment = claims.GroupBy(c => c.Department).ToDictionary(g => g.Key, g => g.Count())
            };

            return View(reportData);
        }
    }
}