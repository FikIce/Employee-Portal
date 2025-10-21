using CapstoneTraineeManagement.DTO;
using CapstoneTraineeManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CapstoneTraineeManagement.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> ValidateUserAsync(string username, string password)
        {
            // IMPORTANT: In a real-world application, passwords should ALWAYS be hashed.
            // This plain-text comparison is a simplification for this project only.
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password && u.IsActive);
        }
    }
}