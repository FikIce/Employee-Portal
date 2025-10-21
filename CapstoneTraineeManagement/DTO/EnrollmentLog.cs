using System;
using System.Collections.Generic;

namespace CapstoneTraineeManagement.DTO;

public partial class EnrollmentLog
{
    public int EnrollmentLogId { get; set; }

    public int EnrollmentLogEnrollmentId { get; set; }

    public string Remarks { get; set; } = null!;

    public int StatusLookUpId { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual User CreatedByUser { get; set; } = null!;

    public virtual Enrollment EnrollmentLogEnrollment { get; set; } = null!;

    public virtual LookUp StatusLookUp { get; set; } = null!;
}
