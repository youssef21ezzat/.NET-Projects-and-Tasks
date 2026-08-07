// LAB 19 — Lab ID: 34 | MIN_GRADE_LAB = 2.0 | COURSE_COUNT = 3
// CoursesController.Index can use a plain Include(c => c.Enrollments) because it only needs the collection of Enrollments to count them; CoursesController.Details needs ThenInclude(e => e.Student) because it needs to access properties of the specific Student inside each Enrollment to display them in the table.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortalWeb.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentPortalWeb.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly StudentPortalContext _context;

        public EnrollmentsController(StudentPortalContext context)
        {
            _context = context;
        }

        // This action needs to query the database because the form requires lists of existing Students and Courses to populate its dropdown menus, whereas Session 17's Create() for Students just rendered empty text inputs.
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var students = await _context.Students.OrderBy(s => s.FullName).ToListAsync();
            var courses = await _context.Courses.OrderBy(c => c.CourseName).ToListAsync();

            ViewData["Students"] = students;
            ViewData["Courses"] = courses;

            return View();
        }

        // EnrollmentDate is set in the controller rather than bound from the form to prevent malicious users from tampering with the date/time of their enrollment by modifying the hidden field in the browser.
        [HttpPost]
        public async Task<IActionResult> Create(Enrollment enrollment)
        {
            if (!ModelState.IsValid)
            {
                // Re-populate ViewData if we are returning the view, otherwise dropdowns will be empty
                ViewData["Students"] = await _context.Students.OrderBy(s => s.FullName).ToListAsync();
                ViewData["Courses"] = await _context.Courses.OrderBy(c => c.CourseName).ToListAsync();
                return View(enrollment);
            }

            enrollment.EnrollmentDate = DateTime.Now;
            
            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();

            // Looking up the names to show in TempData (since model binding only gave us IDs)
            var student = await _context.Students.FindAsync(enrollment.StudentId);
            var course = await _context.Courses.FindAsync(enrollment.CourseId);

            TempData["Message"] = $"Successfully enrolled {student?.FullName} in {course?.CourseName}.";

            return RedirectToAction("Details", "Students", new { id = enrollment.StudentId });
        }
        
        // PART E OBSERVATION:
        // Attempting to enroll in the same course a second time resulted in an ugly server error page (an unhandled DbUpdateException) because the database rejected the insert due to the unique index on (StudentId, CourseId), exactly matching the console demo's behaviour where SaveChanges() threw an exception rather than silently failing.
    }
}
