using CapstoneTraineeManagement.DTO;
using CapstoneTraineeManagement.Interfaces;
using CapstoneTraineeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CapstoneTraineeManagement.Services
{
    public class ProgramService : IProgramService
    {
        private readonly ApplicationDbContext _context;

        public ProgramService(ApplicationDbContext context)
        {
            _context = context;
        }

    }
}
