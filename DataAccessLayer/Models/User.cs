using System;
using System.Collections.Generic;

namespace DCMProcess.DataAccessLayer;

public partial class User
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string EmailId { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string? Speciality { get; set; }

    public string? CurrentWorkPlace { get; set; }

    public byte? RoleId { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public virtual Role? Role { get; set; }
}
