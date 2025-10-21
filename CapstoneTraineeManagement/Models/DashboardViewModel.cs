namespace CapstoneTraineeManagement.Models
{
    public class DashboardViewModel
    {
        public int TotalTrainees { get; set; }
        public int TotalPrograms { get; set; }
        public Dictionary<string, int> EnrollmentsByProgram { get; set; }
    }
}