using System;
using System.Collections.Generic;

namespace SalonIt.Models;

public partial class Salon
{
    public int SalonId { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public int? OwnerId { get; set; }

    public string? Contact { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Owner? Owner { get; set; }

    public virtual ICollection<Reviewfeedback> Reviewfeedbacks { get; set; } = new List<Reviewfeedback>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
