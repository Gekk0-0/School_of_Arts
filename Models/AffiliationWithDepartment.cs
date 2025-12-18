using System;
using System.Collections.Generic;

namespace School_of_arts.Models;

public partial class AffiliationWithDepartment
{
    public int AffiliationWithDepartmentId { get; set; }

    public decimal WageRate { get; set; }

    public string OccupationType { get; set; } = null!;

    public int TeacherId { get; set; }

    public int DisciplineId { get; set; }

    public virtual Discipline Discipline { get; set; } = null!;

    public virtual Teacher Teacher { get; set; } = null!;
}
