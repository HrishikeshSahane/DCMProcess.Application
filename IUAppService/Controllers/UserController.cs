using Microsoft.AspNetCore.Mvc;
using DCMProcess.DataAccessLayer;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;

namespace DCMProcess.AppService
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly DbprojectContext _context;
        DataRepository repObj;

        public UserController(DbprojectContext context)
        {
            _context = context;
            repObj = new DataRepository(_context);
        }

        [HttpPost]
        [Authorize]
        [Route("Login")]
        public JsonResult ValidateUserCredentials([FromBody] DCMProcess.DataAccessLayer.User obj)
        {
            string role = string.Empty;
            try
            {
                role = repObj.ValidateLoginUsingLinq(obj.EmailId, obj.UserPassword);
            }
            catch (Exception ex)
            {
                role = "Invalid credentials";
            }
            return Json(role);
        }


        [HttpPost]
        [Authorize]
        [Route("Register")]
        public JsonResult RegisterNewUser([FromBody] DCMProcess.DataAccessLayer.User obj)
        {
            string retVal = string.Empty;
            try
            {
                if (obj.RoleId == 0)
                {
                    retVal = "No such role present.Please enter a valid role";
                }
                else
                {
                    string response=repObj.RegisterNewUser(obj);
                    retVal = response;
                }

                //role = repObj(obj.EmailId, obj.UserPassword);
            }
            catch (Exception ex)
            {
                retVal = "Some error occured";
            }
            return Json(retVal);
        }

        [HttpGet]
        [Authorize]
        [Route("Get")]
        public JsonResult Get()
        {
            return Json("JWT Working");
        }
    }
}
