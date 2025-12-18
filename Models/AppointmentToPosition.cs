using System;
using System.Collections.Generic;

namespace School_of_arts.Models;

public partial class AppointmentToPosition
{
    public int AppointmentToPositionId { get; set; }

    public string PositionStatus { get; set; } = null!;

    public DateOnly AppointmentDate { get; set; }

    public DateOnly? DismissalDate { get; set; }

    public int TeacherId { get; set; }

    public int PostId { get; set; }

    public virtual Position Post { get; set; } = null!;

    public virtual Teacher Teacher { get; set; } = null!;
}
