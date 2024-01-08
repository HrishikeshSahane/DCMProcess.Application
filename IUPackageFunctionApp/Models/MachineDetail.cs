using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IUPackageFunctionApp.Models
{
    public class MachineDetail
    {
        public int Id { get; set; }

        public int? SeriesId { get; set; }

        public string ImageSeriesId { get; set; }

        public string Manufacturer { get; set; } = null!;

        public string ModelName { get; set; } = null!;

        public string OperatorName { get; set; } = null!;

        public int? XrayTubeCurrent { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
