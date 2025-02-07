using System;
using System.Collections.Generic;

namespace SalonIt.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int? UserId { get; set; }

    public int? SalonId { get; set; }

    public int? ServiceId { get; set; }

    public DateTime? AppointmentDate { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Salon? Salon { get; set; }

    public virtual Service? Service { get; set; }

    public virtual User? User { get; set; }
}
