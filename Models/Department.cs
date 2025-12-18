using System;
using System.Collections.Generic;

namespace School_of_arts.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<SpecialtyDiscipline> SpecialtyDisciplines { get; set; } = new List<SpecialtyDiscipline>();
}
