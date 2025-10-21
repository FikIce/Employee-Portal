using System;
using System.Collections.Generic;

namespace CapstoneTraineeManagement.DTO;

public partial class LookUp
{
    public int LookUpId { get; set; }

    public int LookUptypeCategoryId { get; set; }

    public string ValueCode { get; set; } = null!;

    public string? ValueDescription { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<EnrollmentLog> EnrollmentLogs { get; set; } = new List<EnrollmentLog>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual LookUpCategory LookUptypeCategory { get; set; } = null!;

    public virtual ICollection<Program> ProgramCategoryLookUps { get; set; } = new List<Program>();

    public virtual ICollection<Program> ProgramModeLookUps { get; set; } = new List<Program>();
}
