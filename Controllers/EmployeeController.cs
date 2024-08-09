using CompanyAPI.DTO;
using CompanyAPI.Interfaces;
using CompanyAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CompanySystemWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController :ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] DTOEmployee employee)
        {
            try
            {
                var inputEmployee = await _employeeService.AddEmployee(employee);
                return Ok(inputEmployee);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message); 
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees([FromQuery] int pageNumber)
        {
            var employees = await _employeeService.GetAllEmployees(pageNumber);
            return Ok(employees);
        }
        [HttpGet("born-in-1980-1990")]
        public async Task<IActionResult> GetEmployeesBorne1980([FromQuery] int pageNumber){
            var employees = await _employeeService.GetEmployeesBorne1980(pageNumber);
            return Ok(employees);
        }
        [HttpGet("female-born-in-More-Than-1990")]
        public async Task<IActionResult> GetFemaleEmployeesBorne1980([FromQuery] int pageNumber){
            var employees = await _employeeService.GetFemaleEmployeesBorne1980(pageNumber);
            return Ok(employees);
        }
        [HttpGet("not-manager")]
        public async Task<IActionResult> GetNonManagerEmployees([FromQuery] int pageNumber){
            var employees = await _employeeService.GetNonManagerEmployees(pageNumber);
            return Ok(employees);
        }
        [HttpGet("BRICS")]
        public async Task<IActionResult> GetBRICSEmployees([FromQuery] int pageNumber){
            var employees = await _employeeService.GetBRICSEmployees(pageNumber);
            return Ok(employees);
        }
        [HttpGet("ITDepartment")]
        public async Task<IEnumerable<Employee>> GetITEmployees([FromQuery] int pageNumber){
            var ITEmployees = await _employeeService.GetITEmployees(pageNumber);
            return ITEmployees;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            Employee employee = await _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditEmployee([FromBody] DTOEmployee employee, int id)
        {
            try
            {
                var updatedEmployee = await _employeeService.UpdateEmployee(employee, id);
                if (updatedEmployee == null)
                {
                    return NotFound();
                }
                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            bool isDeletedEmployee = await _employeeService.DeleteEmployee(id);
            if (isDeletedEmployee == false)
            {
                return NotFound();
            }
            return Ok("Employee is deleted");
        }
    }
}
