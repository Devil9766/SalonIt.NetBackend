using System;
using System.Collections.Generic;

namespace SalonIt.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Contact { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }
}
