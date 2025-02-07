using System;
using System.Collections.Generic;

namespace SalonIt.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public int? SalonId { get; set; }

    public string? ServiceName { get; set; }

    public decimal? Cost { get; set; }

    public bool? Availability { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Salon? Salon { get; set; }
}
