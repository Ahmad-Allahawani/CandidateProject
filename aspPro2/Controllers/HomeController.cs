using aspPro2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace aspPro2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //course section start 

        public IActionResult CoursePage()
        {
            var cors  =  _context.Courses.ToList();
            return View(cors);
        }

        [HttpGet]
        public IActionResult CreateCourse()
        {
            ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(CoursePage));
            }
            
            return View(course);
        }

        [HttpGet]
        public IActionResult EditCourse(int id )
        {
            var course = _context.Courses.Find(id);
               if (course == null)
               {
                    return View("Error");
               }
            ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name", course.DepartmentId);
            return View(course);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourse(int id ,Course course)
        {
            if(id != course.Id)
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
        public async Task <IActionResult> DeleteCourse(int id)
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
        //course section end

        //enrollment section start 
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
        public async Task<IActionResult> CreateEnrollment(Enrollment enroll)
        {
            if (ModelState.IsValid)
            {
                enroll.EnrollmentDate = DateTime.Now;
                _context.Enrollments.Add(enroll);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(EnrollmentPage));
            }
            foreach (var kvp in ModelState)
            {
                foreach (var error in kvp.Value.Errors)
                {
                    Console.WriteLine($"Field: {kvp.Key}, Error: {error.ErrorMessage}");
                }
            }
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

        //enrollment section end

        //department section start 
        public IActionResult DepartmentPage()
        {
            var dept = _context.Departments.ToList();
            return View(dept);
        }
        public IActionResult CreateDepartment()
        {
            ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        [HttpPost  , ActionName("CreateDepartment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDepartment(Department dept)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(dept);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            
            return View(dept);
        }

        public IActionResult EditDepartment(int id)
        {
            var dept = _context.Departments.Find(id);
            if (dept == null)
            {
                return View("Error");
            }
            
            return View(dept);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDepartment(int id, Department dept)
        {
            if (id != dept.Id)
            {
                return BadRequest();

            }

            if (!ModelState.IsValid)
            {
                return View(dept);
            }
            try
            {
                _context.Departments.Update(dept);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Departments.Any(d => d.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(DepartmentPage));

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null)
            {

                return NotFound();
            }
            _context.Departments.Remove(dept);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DepartmentPage));
        }


        //department section end





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
