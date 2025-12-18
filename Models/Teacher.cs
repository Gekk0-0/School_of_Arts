using System;
using System.Collections.Generic;

namespace School_of_arts.Models;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public string FullName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public DateOnly Birthdate { get; set; }

    public decimal Salary { get; set; }

    public virtual ICollection<AffiliationWithDepartment> AffiliationWithDepartments { get; set; } = new List<AffiliationWithDepartment>();

    public virtual ICollection<AppointmentToPosition> AppointmentToPositions { get; set; } = new List<AppointmentToPosition>();

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Education> Educations { get; set; } = new List<Education>();
}
