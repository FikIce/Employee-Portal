using System;
using System.Collections.Generic;

namespace CapstoneTraineeManagement.DTO;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<EnrollmentLog> EnrollmentLogs { get; set; } = new List<EnrollmentLog>();
}
