﻿using System;
using System.Collections.Generic;

namespace DCMProcess.DataAccessLayer;

public partial class Patient
{
    public int Id { get; set; }

    public string PatientId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public DateTime PatientCreatedDate { get; set; }

    public virtual ICollection<ScanDetail> ScanDetails { get; set; } = new List<ScanDetail>();
}
