using CapstoneTraineeManagement.DTO;
using CapstoneTraineeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace CapstoneTraineeManagement.Controllers
{
    public class TraineesController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TraineesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewData["SuccessMessage"] = TempData["SuccessMessage"];
            }

            // 1. Get the queryable source of trainees
            var traineesQuery = _context.Trainees
                .Include(t => t.CategoryLookUp)
                .Include(t => t.StateLookUp)
                .AsNoTracking()
                .OrderBy(t => t.FullName);

            // 2. Get the paged list
            var pagedTrainees = traineesQuery.ToPagedList(page, pageSize);

            // 3. Pass the paged list to the view
            return View(pagedTrainees);
        }

        // GET: Trainees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var trainee = await _context.Trainees
                .Include(t => t.StateLookUp)
                .Include(t => t.CategoryLookUp)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.TraineeId == id);

            if (trainee == null) return NotFound();
            return View(trainee);
        }


        // --- CREATE ACTIONS (Using ViewModel) ---
        // GET: Trainees/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            var viewModel = new TraineeCreateViewModel();
            return View(viewModel);
        }

        // POST: Trainees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TraineeCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Trainees.AnyAsync(t => t.IdentityNo == viewModel.IdentityNo))
                {
                    ModelState.AddModelError("IdentityNo", "This Identity Number already exists.");
                }
                else if (await _context.Trainees.AnyAsync(t => t.Email == viewModel.Email))
                {
                    ModelState.AddModelError("Email", "This Email address already exists.");
                }
                else
                {
                    var trainee = new Trainee
                    {
                        IdentityNo = viewModel.IdentityNo,
                        FullName = viewModel.FullName,
                        Dob = viewModel.Dob,
                        Email = viewModel.Email,
                        ContactNo = viewModel.ContactNo,
                        Address = viewModel.Address,
                        StateLookUpId = viewModel.StateLookUpId,
                        CategoryLookUpId = viewModel.CategoryLookUpId,
                        Preference = viewModel.Preference,
                        WorkingExperience = viewModel.WorkingExperience,
                        AdditonalNote = viewModel.AdditonalNote,
                        ProfilePhotoFileName = await UploadFile(viewModel.ProfilePhoto),
                        ResumeFileName = await UploadFile(viewModel.ResumeFile)
                    };

                    _context.Add(trainee);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Trainee created successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            await PopulateDropdowns(viewModel.StateLookUpId, viewModel.CategoryLookUpId);
            return View(viewModel);
        }


        // --- EDIT ACTIONS (Using ViewModel) ---
        // GET: Trainees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var trainee = await _context.Trainees.AsNoTracking().FirstOrDefaultAsync(t => t.TraineeId == id);
            if (trainee == null) return NotFound();

            var viewModel = new TraineeEditViewModel
            {
                TraineeId = trainee.TraineeId,
                IdentityNo = trainee.IdentityNo,
                FullName = trainee.FullName,
                Dob = trainee.Dob,
                Email = trainee.Email,
                ContactNo = trainee.ContactNo,
                Address = trainee.Address,
                StateLookUpId = trainee.StateLookUpId,
                CategoryLookUpId = trainee.CategoryLookUpId,
                Preference = trainee.Preference,
                WorkingExperience = trainee.WorkingExperience,
                AdditonalNote = trainee.AdditonalNote,
                ProfilePhotoFileName = trainee.ProfilePhotoFileName,
                ResumeFileName = trainee.ResumeFileName
            };

            await PopulateDropdowns(viewModel.StateLookUpId, viewModel.CategoryLookUpId);
            return View(viewModel);
        }

        // POST: Trainees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TraineeEditViewModel viewModel, IFormFile? profilePhoto, IFormFile? resumeFile)
        {
            if (id != viewModel.TraineeId) return NotFound();

            if (await _context.Trainees.AnyAsync(t => t.IdentityNo == viewModel.IdentityNo && t.TraineeId != id))
                ModelState.AddModelError("IdentityNo", "This Identity Number is already used by another trainee.");

            if (await _context.Trainees.AnyAsync(t => t.Email == viewModel.Email && t.TraineeId != id))
                ModelState.AddModelError("Email", "This Email address is already used by another trainee.");

            if (ModelState.IsValid)
            {
                var traineeToUpdate = await _context.Trainees.FindAsync(id);
                if (traineeToUpdate == null) return NotFound();

                traineeToUpdate.FullName = viewModel.FullName;
                traineeToUpdate.IdentityNo = viewModel.IdentityNo;
                traineeToUpdate.Dob = viewModel.Dob;
                traineeToUpdate.Email = viewModel.Email;
                traineeToUpdate.ContactNo = viewModel.ContactNo;
                traineeToUpdate.Address = viewModel.Address;
                traineeToUpdate.StateLookUpId = viewModel.StateLookUpId;
                traineeToUpdate.CategoryLookUpId = viewModel.CategoryLookUpId;
                traineeToUpdate.Preference = viewModel.Preference;
                traineeToUpdate.WorkingExperience = viewModel.WorkingExperience;
                traineeToUpdate.AdditonalNote = viewModel.AdditonalNote;

                if (profilePhoto != null)
                    traineeToUpdate.ProfilePhotoFileName = await UploadFile(profilePhoto);

                if (resumeFile != null)
                    traineeToUpdate.ResumeFileName = await UploadFile(resumeFile);

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Trainee updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(viewModel.StateLookUpId, viewModel.CategoryLookUpId);
            return View(viewModel);
        }

        // --- HELPER METHODS ---
        private async Task<string> UploadFile(IFormFile? file)
        {
            string fileName = "";
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return fileName;
        }

        private async Task PopulateDropdowns(int? selectedStateId = null, int? selectedCategoryId = null)
        {
            var stateCategory = await _context.LookUpCategories.AsNoTracking().FirstOrDefaultAsync(c => c.CategoryName == "State");
            var traineeCategory = await _context.LookUpCategories.AsNoTracking().FirstOrDefaultAsync(c => c.CategoryName == "TraineeCategory");

            var statesList = new List<LookUp>();
            if (stateCategory != null)
                statesList = await _context.LookUps.Where(l => l.LookUptypeCategoryId == stateCategory.LookUpCategoryId).OrderBy(l => l.SortOrder).AsNoTracking().ToListAsync();

            var categoriesList = new List<LookUp>();
            if (traineeCategory != null)
                categoriesList = await _context.LookUps.Where(l => l.LookUptypeCategoryId == traineeCategory.LookUpCategoryId).OrderBy(l => l.SortOrder).AsNoTracking().ToListAsync();

            ViewData["States"] = new SelectList(statesList, "LookUpId", "ValueCode", selectedStateId);
            ViewData["Categories"] = new SelectList(categoriesList, "LookUpId", "ValueCode", selectedCategoryId);
        }

        private bool TraineeExists(int id) => _context.Trainees.Any(e => e.TraineeId == id);

        // Delete Actions are not needed for this controller based on the last working version,
        // but can be added here if required.
    }
}