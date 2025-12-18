using System;
using System.Collections.Generic;

namespace School_of_arts.Models;

public partial class SpecialtyDiscipline
{
    public int SpecialtyDisciplineId { get; set; }

    public int DepartmentId { get; set; }

    public int DisciplineId { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual Department Department { get; set; } = null!;

    public virtual Discipline Discipline { get; set; } = null!;
}
