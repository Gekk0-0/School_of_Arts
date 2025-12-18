using System;
using System.Collections.Generic;

namespace School_of_arts.Models;

public partial class Discipline
{
    public int DisciplineId { get; set; }

    public string SubjectName { get; set; } = null!;

    public int TermOfStudy { get; set; }

    public virtual ICollection<AffiliationWithDepartment> AffiliationWithDepartments { get; set; } = new List<AffiliationWithDepartment>();

    public virtual ICollection<Education> Educations { get; set; } = new List<Education>();

    public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();

    public virtual ICollection<SpecialtyDiscipline> SpecialtyDisciplines { get; set; } = new List<SpecialtyDiscipline>();
}
