using DCMProcess.DataAccessLayer;

namespace DCMProcess.AppService
{
    public class ScanDetailModel
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
