using CapstoneTraineeManagement.DTO;
using CapstoneTraineeManagement.Interfaces;
using CapstoneTraineeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CapstoneTraineeManagement.Services
{
    public class TraineeService : ITraineeService
    {
        private readonly ApplicationDbContext _context;

        public TraineeService(ApplicationDbContext context)
        {
            _context = context;
        }

    }
}
