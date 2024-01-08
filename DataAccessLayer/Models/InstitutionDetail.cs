using System;
using System.Collections.Generic;

namespace DCMProcess.DataAccessLayer;

public partial class InstitutionDetail
{
    public int Id { get; set; }

    public string InstitutionName { get; set; } = null!;

    public string DepartmentName { get; set; } = null!;

    public string InstitutionAddress { get; set; } = null!;

    public string SeriesId { get; set; } = null!;

    public DateTime CreatedDate { get; set; }
}
