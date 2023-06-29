using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using TestWeb.API.DataContext;
using TestWeb.API.Models;

namespace TestWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private EmployeeDbContext _dbcontext;
        public EmployeeController(EmployeeDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        [Authorize]
        [HttpGet]
        [Route("EmployeeList")]
        public IActionResult GetEmployeeList()
        {
            try
            {
                var employeelist = _dbcontext.Employee.ToList();
                return Ok(new { success = true, message = "got successfully", data = employeelist }); //return ok refers to success
            }
            
            catch (Exception ex)
            {
                return BadRequest(new {success =false, message = ex.Message});
            }
        }
        [Authorize]
        [HttpPost]
        [Route("AddEmployee")]

        public IActionResult SaveEmployee( Employee Modal)
        {
            try
            {
                _dbcontext.Employee.Add(Modal);
                _dbcontext.SaveChanges();
                var emplyeeid = Modal.Id;
                var employee = _dbcontext.Employee.Find(emplyeeid);
                return Ok(new { success = true, message = "employee  Addes successfully", data= employee });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("EmployeeById/{employeeid}")]
        public IActionResult GetEmployeeById(int employeeId)
        {
            try
            {
                var employee = _dbcontext.Employee.Find(employeeId);
                return Ok(new { success = true, message = "got successfully", singledata = employee }); //return ok refers to success
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
           
        }
        [Authorize]
        [HttpDelete]
        [Route("DeleteEmployee")]
        public IActionResult DeleteEmployee(int employeeId)
        {
            try
            {
                var employee = _dbcontext.Employee.Find(employeeId);
                if(employee != null)
                {
                    _dbcontext.Employee.Remove(employee);
                    _dbcontext.SaveChanges();
                    return Ok(new { success = true, message = "EMPLOYEE DELETED successfully" });
                }
                else
                {
                    return NotFound(new { success = false, message = "Not Found" });
                }
               
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [Authorize]
        [HttpPut]
        [Route("UpdateEmployee")]
        public IActionResult UpdateEmployee(Employee employee)
        {
            try
            {
                _dbcontext.Employee.Update(employee);
                _dbcontext.SaveChanges();
                return Ok(new { success = true, message = "EMPLOYEE Added successfully" });
            }

            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

    }
}
