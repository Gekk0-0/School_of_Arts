using System;
using System.Collections.Generic;

namespace School_of_arts.Models;

public partial class Education
{
    public int EducationId { get; set; }

    public int TeacherId { get; set; }

    public int DisciplineId { get; set; }

    public string LessonType { get; set; } = null!;

    public int Semester { get; set; }

    public int HoursPerSemester { get; set; }

    public virtual Discipline Discipline { get; set; } = null!;

    public virtual Teacher Teacher { get; set; } = null!;
}
