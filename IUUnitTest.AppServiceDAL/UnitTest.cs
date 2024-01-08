using DCMProcess.DataAccessLayer;

namespace IUUnitTest.AppServiceDAL
{
    public class Tests
    {

        public DbprojectContext context = new DbprojectContext();
        public DataRepository _repository;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ValidateSavePatient()
        {
            _repository=new DataRepository(context);
            Patient patient = new Patient();
            patient.PatientId = "10000A";
            patient.FirstName = "FirstNameNTest";
            patient.LastName = "LastNameNTest";
            patient.PatientCreatedDate = new DateTime(1970,07,28);
            patient.Gender = "Male";
            patient.PatientCreatedDate = DateTime.Now;
            var actual = _repository.SavePatient(patient);
            var result = "Success";
            Assert.AreEqual(result, actual);

        }


        [Test]
        public void ValidateSaveScanDetails()
        {
            _repository = new DataRepository(context);
            ScanDetail scanDetail = new ScanDetail();
            scanDetail.ImageModality = "CT";
            scanDetail.ImageType = "ORIGINAL/PRIMARY/AXIAL/HELICAL";
            scanDetail.SeriesId = "1.2.3.4.5.66666.8888.0000";
            scanDetail.ScannedDate = DateTime.Now;
            scanDetail.ModifiedDate = null;
            scanDetail.PatientOrientation = "-267.5/242.3/90.52";
            scanDetail.PatientPosition = "1/0/0/0/-1/0";
            var actual = _repository.SaveScanDetails(scanDetail, "10000A");
            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateSaveInstitutionDetails()
        {
            _repository = new DataRepository(context);
            InstitutionDetail institutionDetail = new InstitutionDetail();
            institutionDetail.InstitutionName= "TestHospital";
            institutionDetail.DepartmentName = "OncologyDepartment";
            institutionDetail.InstitutionAddress = "Berlin,Germany";
            institutionDetail.CreatedDate = DateTime.Now;
            var actual = _repository.SaveInstitutionDetails(institutionDetail, "1.2.3.4.5.66666.8888.0000");
            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SaveMachineDetails()
        {
            _repository= new DataRepository(context);
            MachineDetail machineDetail = new MachineDetail();
            machineDetail.Manufacturer = "Siemens";
            machineDetail.OperatorName = "Tom";
            machineDetail.XrayTubeCurrent = 240;
            machineDetail.ModelName = "M240";
            machineDetail.CreatedDate=DateTime.Now;
            var actual = _repository.MachineScannerDetails(machineDetail, "1.2.3.4.5.66666.8888.0000");
            var expected = true;
            Assert.AreEqual(expected, actual);


        }

        [Test]
        public void GetPatientDetails()
        {
            _repository=new DataRepository(context);
            Patient patient = _repository.GetPatient("10000A");
            var actual = $"{patient.FirstName} {patient.LastName}";
            var expected = "FirstNameNTest LastNameNTest";
            Assert.AreEqual(expected, actual);
            
        }

        [Test]
        public void GetScanDetails()
        {
            _repository = new DataRepository(context);
            List<ScanDetail> scanDetail = _repository.GetScanDetails("10000A");
            var actual = $"{scanDetail[0].SeriesId} {scanDetail[0].ImageType}";
            var expected = "1.2.3.4.5.66666.8888.0000 ORIGINAL/PRIMARY/AXIAL/HELICAL";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateGetInstitutionDetails()
        {
            _repository = new DataRepository(context);
            InstitutionDetail institutionDetail = _repository.GetInstitutionDetail("10000A");
            var actual = $"{institutionDetail.InstitutionName} {institutionDetail.DepartmentName}";
            var expected = "TestHospital OncologyDepartment";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateGetMachineDetails()
        {
            _repository = new DataRepository(context);
            MachineDetail machineDetail = _repository.GetMachineScannerDetail("10000A");
            var actual = $"{machineDetail.Manufacturer} {machineDetail.ModelName}";
            var expected = "Siemens M240";
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void ValidateDuplicatePatientCreation()
        {
            _repository = new DataRepository(context);
            Patient patient = new Patient();
            patient.PatientId = "10000A";
            patient.FirstName = "FirstNameNTest";
            patient.LastName = "LastNameNTest";
            patient.PatientCreatedDate = new DateTime(1970, 07, 28);
            patient.Gender = "Male";
            patient.PatientCreatedDate = DateTime.Now;
            var actual = _repository.SavePatient(patient);
            var result = "Null";
            Assert.AreEqual(result, actual);
        }
    }
}