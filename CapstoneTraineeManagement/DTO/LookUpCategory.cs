using System;
using System.Collections.Generic;

namespace CapstoneTraineeManagement.DTO;

public partial class LookUpCategory
{
    public int LookUpCategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<LookUp> LookUps { get; set; } = new List<LookUp>();
}
