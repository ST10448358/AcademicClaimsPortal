using Microsoft.AspNetCore.Mvc;
using AcademicClaimsPortal.Models;
using AcademicClaimsPortal.Services;

namespace AcademicClaimsPortal.Controllers
{
    public class ClaimController : Controller
    {
        // GET: /Claim/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Claim/Create
        [HttpPost]
        public IActionResult Create(Claim claim)
        {
            if (ModelState.IsValid)
            {
                // Generate claim reference
                claim.ClaimReference = $"CLM-{DateTime.Now:yyyyMMddHHmmss}";
                claim.Status = "Pending";
                claim.SubmissionDate = DateTime.Now;
                claim.Id = DataService.Claims.Count + 1;

                // Add to our in-memory list
                DataService.Claims.Add(claim);

                TempData["SuccessMessage"] = $"Claim submitted successfully! Reference: {claim.ClaimReference}";
                TempData["ClaimReference"] = claim.ClaimReference;
                return RedirectToAction("Create");
            }
            return View(claim);
        }

        // GET: /Claim/TrackStatus
        public IActionResult TrackStatus()
        {
            return View();
        }

        // POST: /Claim/TrackStatus
        [HttpPost]
        public IActionResult TrackStatus(string claimReference)
        {
            if (string.IsNullOrWhiteSpace(claimReference))
            {
                ViewBag.ErrorMessage = "Please enter a claim reference.";
                return View();
            }

            var claim = DataService.Claims.FirstOrDefault(c => c.ClaimReference == claimReference);

            if (claim == null)
            {
                ViewBag.ErrorMessage = "Claim not found. Please check your claim reference.";
                return View();
            }

            return View("StatusDetails", claim);
        }

        // GET: /Claim/StatusDetails
        public IActionResult StatusDetails(string claimReference)
        {
            if (string.IsNullOrWhiteSpace(claimReference))
            {
                return RedirectToAction("TrackStatus");
            }

            var claim = DataService.Claims.FirstOrDefault(c => c.ClaimReference == claimReference);

            if (claim == null)
            {
                TempData["ErrorMessage"] = "Claim not found.";
                return RedirectToAction("TrackStatus");
            }

            return View(claim);
        }

        // GET: /Claim/GetAllClaimReferences
        public IActionResult GetAllClaimReferences()
        {
            var claimReferences = DataService.Claims
                .OrderByDescending(c => c.SubmissionDate)
                .Select(c => c.ClaimReference)
                .ToList();

            return Json(claimReferences);
        }

        // GET: /Claim/GetRecentClaims
        public IActionResult GetRecentClaims()
        {
            var recentClaims = DataService.Claims
                .OrderByDescending(c => c.SubmissionDate)
                .Take(10)
                .Select(c => new
                {
                    claimReference = c.ClaimReference,
                    lecturerName = c.LecturerName,
                    department = c.Department,
                    totalAmount = c.TotalAmount,
                    status = c.Status,
                    submissionDate = c.SubmissionDate
                })
                .ToList();

            return Json(recentClaims);
        }

        // GET: /Claim/SearchClaims
        public IActionResult SearchClaims(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Json(new List<object>());
            }

            var claims = DataService.Claims
                .Where(c => c.ClaimReference.Contains(searchTerm) ||
                           c.LecturerName.Contains(searchTerm))
                .OrderByDescending(c => c.SubmissionDate)
                .Take(10)
                .Select(c => new
                {
                    claimReference = c.ClaimReference,
                    lecturerName = c.LecturerName,
                    department = c.Department,
                    totalAmount = c.TotalAmount,
                    status = c.Status,
                    submissionDate = c.SubmissionDate
                })
                .ToList();

            return Json(claims);
        }

        // GET: /Claim/MyClaims
        public IActionResult MyClaims(string lecturerName = "")
        {
            if (string.IsNullOrWhiteSpace(lecturerName))
            {
                ViewBag.ShowSearchForm = true;
                return View(new List<Claim>());
            }

            var claims = DataService.Claims
                .Where(c => c.LecturerName.Contains(lecturerName))
                .OrderByDescending(c => c.SubmissionDate)
                .ToList();

            ViewBag.LecturerName = lecturerName;
            ViewBag.ShowSearchForm = true;
            return View(claims);
        }
    }
}