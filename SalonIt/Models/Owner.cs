using System;
using System.Collections.Generic;

namespace SalonIt.Models;

public partial class Owner
{
    public int OwnerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Contact { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Salon> Salons { get; set; } = new List<Salon>();
}
