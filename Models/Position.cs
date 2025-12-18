using System;
using System.Collections.Generic;

namespace School_of_arts.Models;

public partial class Position
{
    public int PositionId { get; set; }

    public string PositionName { get; set; } = null!;

    public decimal MinSalary { get; set; }

    public string DutiesDescription { get; set; } = null!;

    public virtual ICollection<AppointmentToPosition> AppointmentToPositions { get; set; } = new List<AppointmentToPosition>();
}
