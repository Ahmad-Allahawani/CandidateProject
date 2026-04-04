using aspPro2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace aspPro2.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EnrollmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult EnrollmentPage()
        {
            var enroll = _context.Enrollments.ToList();
            return View(enroll);

        }
        [HttpGet]
        public IActionResult CreateEnrollment()
        {
            ViewBag.User = new SelectList(_context.Users, "Id", "Name");
            ViewBag.Course = new SelectList(_context.Courses, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEnrollment(Enrollment enroll)
        {
            bool enrollmentExists = _context.Enrollments.Any(e => e.UserId == enroll.UserId && e.CourseId == enroll.CourseId);
            if (ModelState.IsValid && !enrollmentExists)
            {
                enroll.EnrollmentDate = DateTime.Now;
                _context.Enrollments.Add(enroll);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(EnrollmentPage));
            }
            //foreach (var kvp in ModelState)
            //{
            //    foreach (var error in kvp.Value.Errors)
            //    {
            //        Console.WriteLine($"Field: {kvp.Key}, Error: {error.ErrorMessage}");
            //    }
            //}
            ModelState.AddModelError("", "Course already exists");
            return View(enroll);
        }

        public IActionResult EditEnrollment(int id)
        {
            ViewBag.User = new SelectList(_context.Users, "Id", "Name");
            ViewBag.Course = new SelectList(_context.Courses, "Id", "Title");
            var enroll = _context.Enrollments.Find(id);
            if (enroll == null)
            {
                return View("Error");
            }

            return View(enroll);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEnrollment(int id, Enrollment enroll)
        {
            if (id != enroll.Id)
            {
                return BadRequest();

            }

            if (!ModelState.IsValid)
            {
                return View(enroll);
            }
            try
            {
                _context.Enrollments.Update(enroll);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Enrollments.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(EnrollmentPage));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var enroll = await _context.Enrollments.FindAsync(id);
            if (enroll == null)
            {

                return NotFound();
            }
            _context.Enrollments.Remove(enroll);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EnrollmentPage));
        }
    }
}
