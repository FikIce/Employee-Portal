using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CapstoneTraineeManagement.DTO;
using CapstoneTraineeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapstoneTraineeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            // Security Check: Is the user logged in?
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Login", "User");
            }

            var viewModel = new DashboardViewModel
            {
                TotalTrainees = await _context.Trainees.CountAsync(),
                TotalPrograms = await _context.Programs.CountAsync(),
                EnrollmentsByProgram = await _context.Enrollments
                                        .Include(e => e.EnrolledProgram)
                                        .GroupBy(e => e.EnrolledProgram.Name)
                                        .ToDictionaryAsync(g => g.Key, g => g.Count())
            };

            return View(viewModel);
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