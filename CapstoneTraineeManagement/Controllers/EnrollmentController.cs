using CapstoneTraineeManagement.DTO;
using CapstoneTraineeManagement.Interfaces; // This using statement is required
using CapstoneTraineeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CapstoneTraineeManagement.Controllers
{
    public class EnrollmentsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService; // The controller needs this field

        // THIS CONSTRUCTOR IS THE FIX. It MUST ask for IEmailService.
        public EnrollmentsController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index(int? programId)
        {
            if (TempData["SuccessMessage"] != null) ViewData["SuccessMessage"] = TempData["SuccessMessage"];
            if (TempData["ErrorMessage"] != null) ViewData["ErrorMessage"] = TempData["ErrorMessage"];

            var enrollmentsQuery = _context.Enrollments
                .Include(e => e.EnrolledTrainee)
                .Include(e => e.EnrolledProgram)
                .Include(e => e.StatusLookUp)
                .AsQueryable();

            if (programId.HasValue && programId > 0)
            {
                enrollmentsQuery = enrollmentsQuery.Where(e => e.EnrolledProgramId == programId.Value);
            }

            ViewData["Programs"] = new SelectList(await _context.Programs.OrderBy(p => p.Name).AsNoTracking().ToListAsync(), "ProgramId", "Name", programId);
            
            var enrollments = await enrollmentsQuery.AsNoTracking().ToListAsync();
            return View(enrollments);
        }

        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var enrollment = await _context.Enrollments
                .Include(e => e.EnrolledTrainee)
                .Include(e => e.EnrolledProgram)
                .Include(e => e.StatusLookUp)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (enrollment == null) return NotFound();
            return View(enrollment);
        }

        // GET: Enrollments/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new EnrollmentViewModel();
            await PopulateEnrollmentDropdowns(viewModel);
            return View(viewModel);
        }

        // POST: Enrollments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EnrollmentViewModel viewModel)
        {
            bool alreadyEnrolled = await _context.Enrollments.AnyAsync(e => e.EnrolledTraineeId == viewModel.EnrolledTraineeId && e.EnrolledProgramId == viewModel.EnrolledProgramId);
            if (alreadyEnrolled) ModelState.AddModelError("", "This trainee is already enrolled in the selected program.");
            
            var program = await _context.Programs.FindAsync(viewModel.EnrolledProgramId);
            if (program != null && !program.IsActive) ModelState.AddModelError("EnrolledProgramId", "Cannot enroll in an inactive program.");

            if (ModelState.IsValid)
            {
                var enrollment = new Enrollment
                {
                    EnrolledTraineeId = viewModel.EnrolledTraineeId,
                    EnrolledProgramId = viewModel.EnrolledProgramId,
                    EnrolledDate = viewModel.EnrolledDate,
                    StatusLookUpId = viewModel.StatusLookUpId
                };
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Enrollment created successfully!";
                return RedirectToAction(nameof(Index));
            }
            await PopulateEnrollmentDropdowns(viewModel);
            return View(viewModel);
        }

        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var enrollment = await _context.Enrollments.Include(e => e.EnrolledTrainee).Include(e => e.EnrolledProgram).FirstOrDefaultAsync(e => e.EnrollmentId == id);
            if (enrollment == null) return NotFound();
            var viewModel = new EnrollmentViewModel
            {
                EnrollmentId = enrollment.EnrollmentId,
                EnrolledTraineeId = enrollment.EnrolledTraineeId,
                EnrolledProgramId = enrollment.EnrolledProgramId,
                EnrolledDate = enrollment.EnrolledDate,
                StatusLookUpId = enrollment.StatusLookUpId
            };
            await PopulateEnrollmentDropdowns(viewModel);
            ViewData["EnrollmentDetails"] = enrollment;
            return View(viewModel);
        }

        // POST: Enrollments/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, EnrollmentViewModel viewModel)
        //{
        //    if (id != viewModel.EnrollmentId) return NotFound();
        //    if (string.IsNullOrEmpty(viewModel.Remarks)) ModelState.AddModelError("Remarks", "Remarks are required when changing the status.");

        //    if (ModelState.IsValid)
        //    {
        //        var enrollmentToUpdate = await _context.Enrollments
        //            .Include(e => e.EnrolledTrainee)
        //            .Include(e => e.EnrolledProgram).ThenInclude(p => p.CategoryLookUp)
        //            .Include(e => e.EnrolledProgram).ThenInclude(p => p.ModeLookUp)
        //            .FirstOrDefaultAsync(e => e.EnrollmentId == id);
        //        if (enrollmentToUpdate == null) return NotFound();

        //        var originalStatusId = enrollmentToUpdate.StatusLookUpId;
        //        if (originalStatusId != viewModel.StatusLookUpId)
        //        {
        //            var log = new EnrollmentLog
        //            {
        //                EnrollmentLogEnrollmentId = enrollmentToUpdate.EnrollmentId,
        //                Remarks = viewModel.Remarks,
        //                StatusLookUpId = viewModel.StatusLookUpId,
        //                CreatedByUserId = HttpContext.Session.GetInt32("UserId") ?? 1,
        //                CreatedDate = DateTime.Now
        //            };
        //            _context.Add(log);
        //            enrollmentToUpdate.StatusLookUpId = viewModel.StatusLookUpId;
        //            _context.Update(enrollmentToUpdate);
        //            await _context.SaveChangesAsync();

        //            var enrolledStatus = await _context.LookUps.FirstOrDefaultAsync(l => l.ValueCode == "Enrolled");
        //            if (enrolledStatus != null && viewModel.StatusLookUpId == enrolledStatus.LookUpId)
        //            {
        //                try
        //                {
        //                    await _emailService.SendEnrollmentConfirmationAsync(enrollmentToUpdate.EnrolledTrainee, enrollmentToUpdate.EnrolledProgram);
        //                }
        //                catch (Exception ex)
        //                {
        //                    System.Diagnostics.Debug.WriteLine($"--> Email failed to send: {ex.Message} <--");
        //                    TempData["ErrorMessage"] = "Enrollment was updated, but the confirmation email could not be sent.";
        //                }
        //            }
        //        }
        //        TempData["SuccessMessage"] = "Enrollment status updated successfully!";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    var enrollmentForDisplay = await _context.Enrollments.Include(e => e.EnrolledTrainee).Include(e => e.EnrolledProgram).AsNoTracking().FirstOrDefaultAsync(e => e.EnrollmentId == id);
        //    ViewData["EnrollmentDetails"] = enrollmentForDisplay;
        //    await PopulateEnrollmentDropdowns(viewModel);
        //    return View(viewModel);
        //}

        // POST: Enrollments/Edit/5
        // POST: Enrollments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EnrollmentViewModel viewModel)
        {
            if (id != viewModel.EnrollmentId) return NotFound();

            if (string.IsNullOrEmpty(viewModel.Remarks))
            {
                ModelState.AddModelError("Remarks", "Remarks are required when changing the status.");
            }

            if (ModelState.IsValid)
            {
                var enrollmentToUpdate = await _context.Enrollments
                    .Include(e => e.EnrolledTrainee)
                    .Include(e => e.EnrolledProgram).ThenInclude(p => p.CategoryLookUp)
                    .Include(e => e.EnrolledProgram).ThenInclude(p => p.ModeLookUp)
                    .FirstOrDefaultAsync(e => e.EnrollmentId == id);

                if (enrollmentToUpdate == null) return NotFound();

                var originalStatusId = enrollmentToUpdate.StatusLookUpId;

                if (originalStatusId != viewModel.StatusLookUpId)
                {
                    var log = new EnrollmentLog
                    {
                        EnrollmentLogEnrollmentId = enrollmentToUpdate.EnrollmentId,
                        Remarks = viewModel.Remarks,
                        StatusLookUpId = viewModel.StatusLookUpId,
                        CreatedByUserId = HttpContext.Session.GetInt32("UserId") ?? 1,
                        CreatedDate = DateTime.Now
                    };
                    _context.Add(log);

                    enrollmentToUpdate.StatusLookUpId = viewModel.StatusLookUpId;
                    _context.Update(enrollmentToUpdate);

                    await _context.SaveChangesAsync();

                    // 1. Get the LookUp item for the status the user just selected from the form.
                    var newlySelectedStatus = await _context.LookUps.FindAsync(viewModel.StatusLookUpId);

                    // 2. Check if the text of that status is "Enrolled". This is safe and data-agnostic.
                    if (newlySelectedStatus != null && newlySelectedStatus.ValueCode.Equals("Enrolled", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            // If the code reaches here, the breakpoint will be hit.
                            await _emailService.SendEnrollmentConfirmationAsync(
                                enrollmentToUpdate.EnrolledTrainee,
                                enrollmentToUpdate.EnrolledProgram);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"--> Email failed to send: {ex.Message} <--");
                            TempData["ErrorMessage"] = "Enrollment was updated, but the confirmation email could not be sent.";
                        }
                    }
                }

                TempData["SuccessMessage"] = "Enrollment status updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateEnrollmentDropdowns(viewModel);
            return View(viewModel);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var enrollment = await _context.Enrollments.Include(e => e.EnrolledTrainee).Include(e => e.EnrolledProgram).Include(e => e.StatusLookUp).AsNoTracking().FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (enrollment == null) return NotFound();
            ViewData["CanDelete"] = (enrollment.StatusLookUp.ValueCode == "New");
            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollments.Include(e => e.StatusLookUp).FirstOrDefaultAsync(e => e.EnrollmentId == id);
            if (enrollment == null) return NotFound();
            if (enrollment.StatusLookUp.ValueCode != "New")
            {
                TempData["ErrorMessage"] = "Cannot delete an enrollment that is not in 'New' status.";
                return RedirectToAction(nameof(Index));
            }
            var relatedLogs = await _context.EnrollmentLogs.Where(log => log.EnrollmentLogEnrollmentId == id).ToListAsync();
            if (relatedLogs.Any()) _context.EnrollmentLogs.RemoveRange(relatedLogs);
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Enrollment has been deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // Helper method to load all dropdowns for the enrollment forms
        private async Task PopulateEnrollmentDropdowns(EnrollmentViewModel viewModel)
        {
            viewModel.Trainees = new SelectList(await _context.Trainees.OrderBy(t => t.FullName).AsNoTracking().ToListAsync(), "TraineeId", "FullName", viewModel.EnrolledTraineeId);
            viewModel.Programs = new SelectList(await _context.Programs.OrderBy(p => p.Name).AsNoTracking().ToListAsync(), "ProgramId", "Name", viewModel.EnrolledProgramId);
            var enrollmentStatusCategory = await _context.LookUpCategories.FirstOrDefaultAsync(c => c.CategoryName == "EnrollmentStatus");
            if(enrollmentStatusCategory != null)
            {
                var statuses = await _context.LookUps.Where(l => l.LookUptypeCategoryId == enrollmentStatusCategory.LookUpCategoryId).OrderBy(l => l.SortOrder).ToListAsync();
                viewModel.Statuses = new SelectList(statuses, "LookUpId", "ValueCode", viewModel.StatusLookUpId);
            }
        }
    }
}