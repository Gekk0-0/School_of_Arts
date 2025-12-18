using System;
using System.Collections.Generic;

namespace School_of_arts.Models;

public partial class Mark
{
    public int MarkId { get; set; }

    public int Mark1 { get; set; }

    public bool IsPresent { get; set; }

    public int DisciplineId { get; set; }

    public int PupilId { get; set; }

    public DateOnly RatingDate { get; set; }

    public virtual Discipline Discipline { get; set; } = null!;

    public virtual Pupil Pupil { get; set; } = null!;
}
