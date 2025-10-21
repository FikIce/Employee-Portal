using System;
using System.Collections.Generic;

namespace CapstoneTraineeManagement.DTO;

public partial class Program
{
    public int ProgramId { get; set; }

    public string Name { get; set; } = null!;

    public int CategoryLookUpId { get; set; }

    public string Duration { get; set; } = null!;

    public int ModeLookUpId { get; set; }

    public bool IsActive { get; set; }

    public virtual LookUp CategoryLookUp { get; set; } = null!;

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual LookUp ModeLookUp { get; set; } = null!;
}
