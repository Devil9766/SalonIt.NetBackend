using System;
using System.Collections.Generic;

namespace SalonIt.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Contact { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Reviewfeedback> Reviewfeedbacks { get; set; } = new List<Reviewfeedback>();
}
