using CapstoneTraineeManagement.DTO;
using CapstoneTraineeManagement.Interfaces;
using CapstoneTraineeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CapstoneTraineeManagement.Services
{
    public class EnrollmentLogService : IEnrollmentLogService
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentLogService(ApplicationDbContext context)
        {
            _context = context;
        }

    }
}
