using System;
using System.Collections.Generic;

namespace DCMProcess.DataAccessLayer;

public partial class MachineDetail
{
    public int Id { get; set; }

    public int? SeriesId { get; set; }

    public string Manufacturer { get; set; } = null!;

    public string ModelName { get; set; } = null!;

    public string OperatorName { get; set; } = null!;

    public int? XrayTubeCurrent { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ScanDetail? Series { get; set; }
}
