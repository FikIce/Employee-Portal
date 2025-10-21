using CapstoneTraineeManagement.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapstoneTraineeManagement.Controllers
{
    public class ProgramsController : BaseController // Inherit for security
    {
        private readonly ApplicationDbContext _context;

        public ProgramsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Programs
        public async Task<IActionResult> Index()
        {
            // Get all programs and include their Category and Mode for display
            var programs = await _context.Programs
                .Include(p => p.CategoryLookUp)
                .Include(p => p.ModeLookUp)
                .AsNoTracking()
                .ToListAsync();

            return View(programs);
        }

        // GET: Programs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get a single program and all its related data for the details view
            var program = await _context.Programs
                .Include(p => p.CategoryLookUp)
                .Include(p => p.ModeLookUp)
                .Include(p => p.Enrollments)
                    .ThenInclude(e => e.EnrolledTrainee) // For each enrollment, get the trainee
                        .ThenInclude(t => t.StateLookUp) // For each trainee, get their state
                .Include(p => p.Enrollments)
                    .ThenInclude(e => e.StatusLookUp) // For each enrollment, get its status
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ProgramId == id);

            if (program == null)
            {
                return NotFound();
            }

            return View(program);
        }
    }
}