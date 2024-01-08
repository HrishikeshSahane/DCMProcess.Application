namespace DCMProcess.AppService
{
    public class InstitutionDetailModel
    {
        public int Id { get; set; }

        public string InstitutionName { get; set; } = null!;

        public string DepartmentName { get; set; } = null!;

        public string InstitutionAddress { get; set; } = null!;

        public string ImageSeriesID { get; set; } = null!;
        public string SeriesId { get; set; } = null!;

        public DateTime CreatedDate { get; set; }
    }
}
