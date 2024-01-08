using Microsoft.AspNetCore.Mvc;
using IUAppService.Models;
using DCMProcess.DataAccessLayer;
using Microsoft.AspNetCore.Authorization;

namespace DCMProcess.AppService
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstituteController : Controller
    {
        private readonly DbprojectContext _context;
        DataRepository repObj;
        public InstituteController(DbprojectContext context)
        {

            _context = context;
            repObj = new DataRepository(_context);
        }

        [HttpPost]
        [Route("SaveInstitutionDetails")]
        public async Task<JsonResult> SaveInstitutionDetails([FromBody] InstitutionDetailModel institutionDetailModel)
        {
            try
            {
                if(institutionDetailModel != null)
                {
                    InstitutionDetail institutionDetail=new InstitutionDetail();
                    institutionDetail.InstitutionName = institutionDetailModel.InstitutionName;
                    institutionDetail.InstitutionAddress = institutionDetailModel.InstitutionAddress;
                    institutionDetail.DepartmentName=institutionDetailModel.DepartmentName;
                    institutionDetail.CreatedDate = institutionDetailModel.CreatedDate;
                    bool result=repObj.SaveInstitutionDetails(institutionDetail, institutionDetailModel.ImageSeriesID);
                    if (result)
                    {
                        return Json("Success!\nInstitutional Details  Saved Successfully");
                    }
                    else
                    {
                        return Json("Failed to Save Institutional Details");
                    }
                }
                return Json("No Institutional details present to Save");
            }
            catch(Exception ex)
            {
                return Json($"Some error occured.\nException Details : {ex.Message}");
            }
        }

        [HttpPost]
        [Route("SaveMachineScannerDetails")]
        public async Task<JsonResult> SaveMachineScannerDetails([FromBody] MachineDetailModel machineDetailModel)
        {
            try
            {
                if (machineDetailModel != null)
                {
                    MachineDetail machineDetail=new MachineDetail();
                    machineDetail.Manufacturer = machineDetailModel.Manufacturer;
                    machineDetail.ModelName=machineDetailModel.ModelName;
                    machineDetail.OperatorName = machineDetailModel.OperatorName;
                    machineDetail.XrayTubeCurrent = machineDetailModel.XrayTubeCurrent;
                    machineDetail.CreatedDate=machineDetailModel.CreatedDate;
                    bool result=repObj.MachineScannerDetails(machineDetail, machineDetailModel.ImageSeriesId);
                    if (result)
                    {
                        return Json("Success!\nMachine Scanner Details  Saved Successfully");
                    }
                    else
                    {
                        return Json("Failed to Save Machine Scanner Details");
                    }

                }
                return Json("No Machine Scanner details present to Save");
            }
            catch(Exception ex) 
            {
                return Json($"Some error occured.\nException Details : {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetInstitutionDetails")]
        public async Task<JsonResult> GetInstitutionDetails(string patientId)
        {
            try
            {
                InstitutionDetail institutionDetail=repObj.GetInstitutionDetail(patientId);
                if(institutionDetail != null)
                {
                    return Json(institutionDetail);
                }
                else
                {
                    return Json("No institution details found");
                }
            }
            catch(Exception ex)
            {
                return Json($"Some error occured.\nException Details : {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetMachineDetails")]
        public async Task<JsonResult> GetMachineDetails(string patientId)
        {
            try
            {
                MachineDetail machineDetail = repObj.GetMachineScannerDetail(patientId);
                if (machineDetail != null)
                {
                    return Json(machineDetail);
                }
                else
                {
                    return Json("No institution details found");
                }
            }
            catch (Exception ex)
            {
                return Json($"Some error occured.\nException Details : {ex.Message}");
            }
        }
    }
}
