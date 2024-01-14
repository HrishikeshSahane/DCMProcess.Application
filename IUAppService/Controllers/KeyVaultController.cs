using DCMProcess.AppService.Models;
using DCMProcess.DataAccessLayer;
using Microsoft.AspNetCore.Mvc;

namespace DCMProcess.AppService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeyVaultController : Controller
    {
        private KeyVaultRespository _vaultRespository;

        public KeyVaultController()
        {
            _vaultRespository = new KeyVaultRespository();
        }

        [HttpGet]
        [Route("GetSecret")]
        public IActionResult GetSecret(string key)
        {
            string value = String.Empty;
            KeyVaultDetails keyVaultDetails = new KeyVaultDetails();
            try
            {

                keyVaultDetails.Value=_vaultRespository.GetKey(key);
                return Json(keyVaultDetails);
            }
            catch
            {
                value = "Some error occured";
                return Json(keyVaultDetails);
            }
        }
    }
}
