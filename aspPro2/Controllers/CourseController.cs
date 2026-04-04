using aspPro2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace aspPro2.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CoursePage()
        {
            var cors = await _context.Courses.Include(c => c.Enrollments).ToListAsync();
            return View(cors);
        }

        [HttpGet]
        public IActionResult CreateCourse()
        {
            ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(Course course)
        {
            bool courseExists = _context.Courses.Any(c => c.Title == course.Title);
            if (ModelState.IsValid && !courseExists)
            {

                _context.Courses.Add(course);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(CoursePage));
            }

            ModelState.AddModelError("", "Course already exists");
            return View(course);
        }

        [HttpGet]
        public async Task<IActionResult> EditCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return View("Error");
            }
            ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name", course.DepartmentId);
            return View(course);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourse(int id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest();

            }

            if (!ModelState.IsValid)
            {
                return View(course);
            }
            try
            {
                _context.Courses.Update(course);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Courses.Any(c => c.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(CoursePage));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {

                return NotFound();
            }
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CoursePage));
        }
    }
}
