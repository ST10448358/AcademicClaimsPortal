using Microsoft.AspNetCore.Mvc;
using AcademicClaimsPortal.Models;
using AcademicClaimsPortal.Services;

namespace AcademicClaimsPortal.Controllers
{
    public class CoordinatorController : Controller
    {
        // GET: /Coordinator/PendingClaims
        public IActionResult PendingClaims()
        {
            var pendingClaims = DataService.Claims
                .Where(c => c.Status == "Pending")
                .OrderByDescending(c => c.SubmissionDate)
                .ToList();
            return View(pendingClaims);
        }

        // POST: /Coordinator/ApproveClaim
        [HttpPost]
        public IActionResult ApproveClaim(int id)
        {
            var claim = DataService.Claims.FirstOrDefault(c => c.Id == id);
            if (claim != null)
            {
                claim.Status = "Approved by Coordinator";
                claim.ReviewedBy = "Coordinator";
                claim.ReviewDate = DateTime.Now;

                TempData["SuccessMessage"] = $"Claim {claim.ClaimReference} approved successfully!";
            }
            return RedirectToAction("PendingClaims");
        }

        // POST: /Coordinator/RejectClaim
        [HttpPost]
        public IActionResult RejectClaim(int id)
        {
            var claim = DataService.Claims.FirstOrDefault(c => c.Id == id);
            if (claim != null)
            {
                claim.Status = "Rejected by Coordinator";
                claim.ReviewedBy = "Coordinator";
                claim.ReviewDate = DateTime.Now;

                TempData["SuccessMessage"] = $"Claim {claim.ClaimReference} has been rejected.";
            }
            return RedirectToAction("PendingClaims");
        }

        // GET: /Coordinator/AllClaims
        public IActionResult AllClaims()
        {
            var allClaims = DataService.Claims
                .OrderByDescending(c => c.SubmissionDate)
                .ToList();
            return View(allClaims);
        }

        // GET: /Coordinator/ApprovedClaims
        public IActionResult ApprovedClaims()
        {
            var approvedClaims = DataService.Claims
                .Where(c => c.Status.Contains("Approved"))
                .OrderByDescending(c => c.ReviewDate)
                .ToList();
            return View(approvedClaims);
        }

        // GET: /Coordinator/CreateReport
        public IActionResult CreateReport()
        {
            // Get statistics for the report
            var pendingCount = DataService.Claims.Count(c => c.Status == "Pending");
            var approvedCount = DataService.Claims.Count(c => c.Status.Contains("Approved"));
            var totalAmount = DataService.Claims
                .Where(c => c.Status.Contains("Approved"))
                .Sum(c => (double)c.HoursWorked * (double)c.HourlyRate);

            ViewBag.PendingCount = pendingCount;
            ViewBag.ApprovedCount = approvedCount;
            ViewBag.TotalAmount = totalAmount;

            return View();
        }

        // POST: /Coordinator/CreateReport
        [HttpPost]
        public IActionResult CreateReport(string reportTitle, string reportContent, string priority)
        {
            if (string.IsNullOrWhiteSpace(reportTitle) || string.IsNullOrWhiteSpace(reportContent))
            {
                ModelState.AddModelError("", "Please fill in both title and content fields.");
                return View();
            }

            // Simulate saving report
            TempData["ReportSuccess"] = $"Report '{reportTitle}' submitted to Academic Manager successfully!";

            return RedirectToAction("CreateReport");
        }

        // GET: /Coordinator/CheckDocuments
        public IActionResult CheckDocuments(string claimReference = "", string lecturerName = "")
        {
            var documents = DataService.SupportDocuments.AsQueryable();

           

            ViewBag.ClaimReference = claimReference;
            ViewBag.LecturerName = lecturerName;
            return View(documents);
        }

        // GET: /Coordinator/ViewClaimDetails
        public IActionResult ViewClaimDetails(string claimReference)
        {
            var claim = DataService.Claims.FirstOrDefault(c => c.ClaimReference == claimReference);

            if (claim == null)
            {
                TempData["ErrorMessage"] = "Claim not found.";
                return RedirectToAction("CheckDocuments");
            }

            // Get associated documents
            var documents = DataService.SupportDocuments
                .Where(d => d.ClaimReference == claimReference)
                .ToList();

            ViewBag.Documents = documents;
            return View(claim);
        }

        // GET: /Coordinator/Dashboard
        public IActionResult Dashboard()
        {
            var claims = DataService.Claims.ToList();

            var stats = new
            {
                TotalClaims = claims.Count,
                PendingClaims = claims.Count(c => c.Status == "Pending"),
                ApprovedClaims = claims.Count(c => c.Status.Contains("Approved")),
                RejectedClaims = claims.Count(c => c.Status.Contains("Rejected")),
                TotalAmount = claims.Where(c => c.Status.Contains("Approved"))
                                   .Sum(c => (double)c.HoursWorked * (double)c.HourlyRate),
                ClaimsByDepartment = claims.GroupBy(c => c.Department)
                                          .ToDictionary(g => g.Key, g => g.Count())
            };

            return View(stats);
        }
    }
}