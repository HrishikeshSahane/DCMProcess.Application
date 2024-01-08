using Microsoft.AspNetCore.Mvc;

namespace DCMProcess.AppService
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {

        [HttpGet]
        [Route("Launch")]
        public IActionResult Launch()
        {
            return Json($"WELCOME TO DICOM IMAGE PROCESSOR API-{DateTime.Now}");
        }



    }
}
