using CapstoneTraineeManagement.Models;

namespace CapstoneTraineeManagement.Interfaces
{
    public interface ILookUpService
    {
        Task<List<LookUpModel>> GetLookUpAsync(int lookUpCategoryId);
    }
}
