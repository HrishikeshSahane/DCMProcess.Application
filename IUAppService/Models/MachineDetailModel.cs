using DCMProcess.DataAccessLayer;

namespace IUAppService.Models
{
    public class MachineDetailModel
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
