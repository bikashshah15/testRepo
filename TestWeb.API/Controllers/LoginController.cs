using Microsoft.AspNetCore.Mvc;
using TestWeb.API.DataContext;
using TestWeb.API.Helper;
using TestWeb.API.Models;

namespace TestWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private EmployeeDbContext _employeeDbContext;
        private TokenHelper _helper;
        public LoginController(EmployeeDbContext employeeDbContext, TokenHelper tokenhelper) {
            _employeeDbContext = employeeDbContext;
            _helper = tokenhelper;
        }

        [HttpPost]
        public IActionResult Login( Login model)
        {
            try
            {
                var user = _employeeDbContext.Login.Where(x => x.Username == model.Username && x.Password == model.Password).SingleOrDefault();
                if (user == null)
                {
                    return BadRequest(new { success = false, message = "Invalid username or password!" });
                }
                var accessToken = _helper.GenerateAccessToken(model.Username);
                var refreshToken = _helper.GenerateRefreshToken();

                model.AccessToken = accessToken;
                model.RefreshToken = refreshToken;

                _employeeDbContext.Login.Update(model);
                _employeeDbContext.SaveChanges();
                return Ok(new{ success = true, AccessToken= accessToken, RefreshToken=refreshToken });
            }
            catch (Exception ex)
            {
                return  BadRequest(new {Success= false, message=ex.Message});
            }
           
        }
    }
}
