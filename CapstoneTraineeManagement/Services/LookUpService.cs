using CapstoneTraineeManagement.DTO;
using CapstoneTraineeManagement.Interfaces;
using CapstoneTraineeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CapstoneTraineeManagement.Services
{
    public class LookUpService : ILookUpService
    {
        private readonly ApplicationDbContext _context;
        public enum LookUpCategory
        {
            ProgramCategory = 2,
            ProgramMode = 3,
            TraineeCategory = 5,
            TraineeState = 7,
            EnrollementStatus = 8
        }

        public LookUpService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LookUpModel>> GetLookUpAsync(int lookUpCategoryId)
        {

            return await _context.LookUps
                .Include(e => e.LookUptypeCategory)
                .Where(e=>e.IsActive == true && e.LookUptypeCategoryId== lookUpCategoryId)
                .OrderBy(e=>e.SortOrder)
                .Select(e => new LookUpModel
                {
                   LookUpId = e.LookUpId,
                   ValueCode = e.ValueCode,
                })
                .ToListAsync();
        }
    }
}
