using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CapstoneTraineeManagement.Models
{
    public class EnrollmentViewModel
    {
        public int EnrollmentId { get; set; }

        [Required(ErrorMessage = "Please select a Trainee.")]
        [Display(Name = "Trainee")]
        public int EnrolledTraineeId { get; set; }

        [Required(ErrorMessage = "Please select a Program.")]
        [Display(Name = "Program")]
        public int EnrolledProgramId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Enrollment Date")]
        public DateOnly EnrolledDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [Required(ErrorMessage = "Please select a Status.")]
        [Display(Name = "Status")]
        public int StatusLookUpId { get; set; }

        // For the Edit screen later
        public string? Remarks { get; set; }

        // Properties to hold the dropdown lists
        public SelectList? Trainees { get; set; }
        public SelectList? Programs { get; set; }
        public SelectList? Statuses { get; set; }
    }
}