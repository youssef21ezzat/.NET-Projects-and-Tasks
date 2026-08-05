using Lab15_StudentPortalWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Lab15_StudentPortalWeb.Services;
namespace Lab15_StudentPortalWeb.Controllers
{
    public class HomeController : Controller
    {

        private readonly StudentPortalContext _context;
        private readonly IYoussefStampService _stampa;
        private readonly IYoussefStampService _stampb;

     
        public HomeController(StudentPortalContext context, IYoussefStampService stampA, IYoussefStampService stampB)
        {
            _context = context;
            _stampa = stampA;
            _stampb = stampB;
        }
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                .OrderBy(s => s.FullName)
                .ToListAsync();
            ViewBag.Owner = _stampa.Owner;
            ViewBag.StampA = _stampa.Stamp;
            ViewBag.StampB = _stampb.Stamp;
            return View(students);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
