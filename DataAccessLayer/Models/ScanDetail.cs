using System;
using System.Collections.Generic;

namespace DCMProcess.DataAccessLayer;

public partial class ScanDetail
{
    public int Id { get; set; }

    public string SeriesId { get; set; } = null!;

    public int? PatientId { get; set; }

    public string ImageModality { get; set; } = null!;

    public string ImageType { get; set; } = null!;

    public string? PatientPosition { get; set; }

    public string? PatientOrientation { get; set; }

    public DateTime ScannedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<MachineDetail> MachineDetails { get; set; } = new List<MachineDetail>();

    public virtual Patient? Patient { get; set; }
}
