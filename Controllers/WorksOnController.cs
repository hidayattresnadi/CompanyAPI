using CompanyAPI.DTO;
using CompanyAPI.Interfaces;
using CompanyAPI.Models;
using CompanySystemWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CompanySystemWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorksOnController : ControllerBase
    {
        private readonly IWorksOnService _worksOnService;
        public WorksOnController(IWorksOnService worksOnService)
        {
            _worksOnService = worksOnService;
        }
        [HttpPost]
        public async Task<IActionResult> AddWorksOn([FromBody] DTOWorksOn worksOn)
        {
            try
            {
                var inputWorksOn = await _worksOnService.AddWorksOn(worksOn);
                return Ok(inputWorksOn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWorksOns([FromQuery] int pageNumber)
        {
            var worksOns = await _worksOnService.GetAllWorksOns(pageNumber);
            return Ok(worksOns);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorksOnById(int id)
        {
            WorksOn worksOn = await _worksOnService.GetWorksOnById(id);
            if (worksOn == null)
            {
                return NotFound();
            }
            return Ok(worksOn);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditWorksOn([FromBody] DTOWorksOn worksOn, int id)
        {
            try
            {
                var updatedWorksOn = await _worksOnService.UpdateWorksOn(worksOn, id);
                if (updatedWorksOn == null)
                {
                    return NotFound();
                }
                return Ok(updatedWorksOn);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorksOn(int id)
        {
            bool isDeletedWorksOn = await _worksOnService.DeleteWorksOn(id);
            if (isDeletedWorksOn == false)
            {
                return NotFound();
            }
            return Ok("WorksOn is deleted");
        }
        [HttpGet("Total-Hours-Worked")]
        public async Task <IActionResult> GetTotalHoursWorkedEmployee([FromQuery] int pageNumber){
            var employeesTotalWorked = await _worksOnService.GetTotalHoursWorkedEmployee(pageNumber);
            return Ok(employeesTotalWorked);
        }
    }
}
