using System;
using System.Collections.Generic;

namespace CapstoneTraineeManagement.DTO;

public partial class Enrollment
{
    public int EnrollmentId { get; set; }

    public int EnrolledTraineeId { get; set; }

    public int EnrolledProgramId { get; set; }

    public DateOnly EnrolledDate { get; set; }

    public int StatusLookUpId { get; set; }

    public virtual Program EnrolledProgram { get; set; } = null!;

    public virtual Trainee EnrolledTrainee { get; set; } = null!;

    public virtual ICollection<EnrollmentLog> EnrollmentLogs { get; set; } = new List<EnrollmentLog>();

    public virtual LookUp StatusLookUp { get; set; } = null!;
}
