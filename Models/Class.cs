using System;
using System.Collections.Generic;

namespace School_of_arts.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public string ClassName { get; set; } = null!;

    public int CuratorId { get; set; }

    public int StudyYear { get; set; }

    public int SpecialtyId { get; set; }

    public int DepartmentId { get; set; }

    public int TermYears { get; set; }

    public virtual Teacher Curator { get; set; } = null!;

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Pupil> Pupils { get; set; } = new List<Pupil>();

    public virtual SpecialtyDiscipline Specialty { get; set; } = null!;
}
