using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IUPackageFunctionApp.Models
{
    public class ScanDetail
    {
        public int Id { get; set; }

        public string SeriesId { get; set; } = null!;

        public int? PatientId { get; set; }

        public string PatientMRN { get; set; } = null!;

        public string ImageModality { get; set; } = null!;

        public string ImageType { get; set; } = null!;

        public string? PatientPosition { get; set; }

        public string? PatientOrientation { get; set; }

        public DateTime ScannedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }
}
