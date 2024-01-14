    using Microsoft.AspNetCore.Mvc;
using IUAppService.Models;
using DCMProcess.DataAccessLayer;
using Microsoft.AspNetCore.Authorization;

namespace DCMProcess.AppService
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : Controller
    {
        private readonly DbprojectContext _context;
        DataRepository repObj;


        public PatientController(DbprojectContext context)
        {
            _context = context;
            repObj = new DataRepository(_context);
        }


        [HttpPost]
        [Route("SavePatient")]
        public async Task<JsonResult> SavePatient([FromBody] PatientModel patientModel)
        {
            try
            {
                Patient patient = new Patient();
                patient.PatientId = patientModel.PatientId;
                patient.FirstName= patientModel.FirstName;
                patient.LastName= patientModel.LastName??String.Empty;
                patient.DateOfBirth = patientModel.DateOfBirth;
                patient.Gender = patientModel.Gender??String.Empty;
                patient.PatientCreatedDate = patientModel.PatientCreatedDate;

                if (patient != null)
                {

                    string result= repObj.SavePatient(patient);
                    if (result=="Success")
                    {
                        return Json("Success.Patient Saved Successfully");
                    }
                    else if(result!= "Null")
                    {
                        return Json($"Failed Saving Patient details - {result}");
                    }
                }
                return  Json("No Patient details present to Save");
            }
            catch(Exception ex)
            {
                return Json($"Some error occured.\nException Details : {ex.Message}");
            }
            }

        [HttpPost]
        [Route("SaveScanDetails")]
        public async Task<JsonResult> SaveScanDetails([FromBody] ScanDetailModel scanDetailModel)
        {
            try
            {
                if(scanDetailModel != null)
                {
                    ScanDetail scanDetail=new ScanDetail();
                    scanDetail.SeriesId= scanDetailModel.SeriesId;
                    scanDetail.ImageModality=scanDetailModel.ImageModality;
                    scanDetail.ImageType = scanDetailModel.ImageType??String.Empty;
                    scanDetail.PatientPosition = scanDetailModel.PatientPosition;
                    scanDetail.PatientOrientation = scanDetailModel.PatientOrientation;
                    scanDetail.ScannedDate= scanDetailModel.ScannedDate;
                    scanDetail.ModifiedDate = scanDetailModel.ModifiedDate;
                    bool result = repObj.SaveScanDetails(scanDetail, scanDetailModel.PatientMRN);
                    if (result)
                    {
                        return Json("Success.Image scan Details of the patient save Successfully");
                    }
                    return Json("Failed Saving Patient Scan details");
                }
                return Json("No Patient Scan details present to Save");
            }
            catch(Exception ex)
            {
                return Json($"Some error occured.\nException Details : {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetPatientDetails")]
        public async Task<JsonResult> GetPatientDetails(string patientId)
        {
            try
            {
                Patient patient = repObj.GetPatient(patientId);
                if(patient!= null)
                {
                    return Json(patient);
                }
                else
                {
                    return Json("No such Patient found");
                }
            }
            catch(Exception ex)
            {
                return Json($"Some error occured.\nException Details : {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetScanDetails")]
        public async Task<JsonResult> GetScanDetails(string patientId)
        {
            List<ScanDetail> scanDetails = new List<ScanDetail>();
            try
            {
                scanDetails=repObj.GetScanDetails(patientId);
                if (scanDetails != null)
                {
                    return Json(scanDetails.ToArray());

                }
                else
                {
                    return Json("No scan details found");
                }
            }
            catch(Exception ex)
            {
                return Json($"Some error occured.\nException Details : {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetPatientList")]
        public async Task<JsonResult> GetPatientList()
        {
            List<Patient> patients = new List<Patient>();
            try
            {
                patients = repObj.GetAllPatients();
                if(patients != null)
                {
                    return Json(patients);
                }
                else
                {
                    return Json("No patients found");
                }
            }
            catch (Exception ex)
            {
                return Json($"Some error occured.\n Exception Details : {ex.Message}");
            }
        }
    }
}
