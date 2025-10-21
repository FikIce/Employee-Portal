using System.ComponentModel.DataAnnotations;

namespace CapstoneTraineeManagement.Models
{
    public class TraineeEditViewModel
    {
        public int TraineeId { get; set; }

        [Required(ErrorMessage = "Identity Number is required.")]
        public string? IdentityNo { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateOnly Dob { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact Number is required.")]
        public string ContactNo { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid State.")]
        public int StateLookUpId { get; set; }

        [Required(ErrorMessage = "Trainee Category is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Category.")]
        public int CategoryLookUpId { get; set; }

        [Required(ErrorMessage = "Program Preference is required.")]
        public string Preference { get; set; }

        public string? WorkingExperience { get; set; }
        public string? AdditonalNote { get; set; }

        // To hold the existing filenames
        public string? ProfilePhotoFileName { get; set; }
        public string? ResumeFileName { get; set; }
    }
}