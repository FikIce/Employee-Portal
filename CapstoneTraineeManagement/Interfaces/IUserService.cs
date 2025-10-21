using CapstoneTraineeManagement.DTO;

namespace CapstoneTraineeManagement.Interfaces
{
    public interface IUserService
    {
        Task<User?> ValidateUserAsync(string username, string password);
    }
}