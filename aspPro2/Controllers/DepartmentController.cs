using aspPro2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace aspPro2.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

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

        [HttpPost, ActionName("CreateDepartment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDepartment(Department dept)
        {
            bool departmentExists = _context.Departments.Any(d => d.Name == dept.Name);
            if (ModelState.IsValid && !departmentExists)
            {
                _context.Departments.Add(dept);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Course already exists");
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
    }
}
