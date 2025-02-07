using System;
using System.Collections.Generic;

namespace SalonIt.Models;

public partial class Reviewfeedback
{
    public int ReviewId { get; set; }

    public int? UserId { get; set; }

    public int? SalonId { get; set; }

    public int? Rating { get; set; }

    public string? Feedback { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Salon? Salon { get; set; }

    public virtual User? User { get; set; }
}
