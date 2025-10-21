using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Required for [Required]

namespace CapstoneTraineeManagement.DTO;

public partial class Trainee
{
    public int TraineeId { get; set; }

    [Required(ErrorMessage = "Identity Number is required.")]
    public string? IdentityNo { get; set; }

    [Required(ErrorMessage = "Full Name is required.")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Date of Birth is required.")]
    public DateOnly Dob { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Contact Number is required.")]
    public string ContactNo { get; set; } = null!;

    [Required(ErrorMessage = "Address is required.")]
    public string Address { get; set; } = null!;

    [Required(ErrorMessage = "State is required.")]
    public int StateLookUpId { get; set; }

    [Required(ErrorMessage = "Trainee Category is required.")]
    public int CategoryLookUpId { get; set; }

    [Required(ErrorMessage = "Program Preference is required.")]
    public string Preference { get; set; } = null!;

    // The following fields are now optional
    public string? WorkingExperience { get; set; }
    public string? AdditonalNote { get; set; }
    public string? ProfilePhotoFileName { get; set; }
    public string? ResumeFileName { get; set; }


    // Navigation Properties (These are correct)
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public virtual LookUp CategoryLookUp { get; set; } = null!;
    public virtual LookUp StateLookUp { get; set; } = null!;
}