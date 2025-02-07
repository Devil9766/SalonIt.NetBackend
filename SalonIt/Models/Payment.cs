using System;
using System.Collections.Generic;

namespace SalonIt.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? AppointmentId { get; set; }

    public int? UserId { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual User? User { get; set; }
}
