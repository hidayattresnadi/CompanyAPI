using CompanyAPI.DTO;
using CompanyAPI.Interfaces;
using CompanyAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CompanySystemWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentsService _departmentService;
        public DepartmentController(IDepartmentsService departmentService)
        {
            _departmentService = departmentService;
        }
        [HttpPost]
        public async Task<IActionResult> AddDepartment([FromBody] DTODepartment department)
        {
            try
            {
                var inputDepartment = await _departmentService.AddDepartment(department);
                return Ok(inputDepartment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments([FromQuery]int pageNumber)
        {
            var departments = await _departmentService.GetAllDepartments(pageNumber);
            return Ok(departments);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            Department department = await _departmentService.GetDepartmentById(id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditDepartment([FromBody] DTODepartment department, int id)
        {
            try
            {
                var updatedDepartment = await _departmentService.UpdateDepartment(department, id);
                if (updatedDepartment == null)
                {
                    return NotFound();
                }
                return Ok(updatedDepartment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            bool isDeletedDepartment = await _departmentService.DeleteDepartment(id);
            if (isDeletedDepartment == false)
            {
                return NotFound();
            }
            return Ok("Department is deleted");
        }
        [HttpGet("female-managers")]
        public async Task<IActionResult> GetFemaleManagers([FromQuery]int pageNumber){
            var employees = await _departmentService.GetFemaleManagers(pageNumber);
            return Ok(employees);
        }
        [HttpGet("count-female-managers")]
        public async Task<IActionResult> GetCountingOfFemaleManagers(){
            var count = await _departmentService.GetCountingOfFemaleManagers();
            return Ok(count);
        }
        [HttpGet("Counting-More-Than-10-Employees")]
        public async Task<IActionResult> GetDepartmentsWithMoreThanTenEmployee([FromQuery]int pageNumber){
            var departments = await _departmentService.GetDepartmentsWithMoreThanTenEmployees(pageNumber);
            return Ok(departments);
        }
    }
}
