using System;
using System.Collections.Generic;

namespace School_of_arts.Models;

public partial class Pupil
{
    public int PupilId { get; set; }

    public string FullName { get; set; } = null!;

    public int ClassId { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public DateOnly Birthdate { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();
}
