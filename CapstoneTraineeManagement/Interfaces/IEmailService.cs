using CapstoneTraineeManagement.DTO;
using System.Threading.Tasks;

namespace CapstoneTraineeManagement.Interfaces
{
    public interface IEmailService
    {
        Task SendEnrollmentConfirmationAsync(Trainee trainee, DTO.Program program);
    }
}