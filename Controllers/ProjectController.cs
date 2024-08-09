using CompanyAPI.DTO;
using CompanyAPI.Interfaces;
using CompanyAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanySystemWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        [HttpPost]
        public async Task<IActionResult> AddProject([FromBody] DTOProject project)
        {
            try
            {
                var inputProject = await _projectService.AddProject(project);
                return Ok(inputProject);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProjects([FromQuery]int pageNumber)
        {
            var projects = await _projectService.GetAllProjects(pageNumber);
            return Ok(projects);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            Project project = await _projectService.GetProjectById(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditProject([FromBody] DTOProject project, int id)
        {
            try
            {
                var updatedProject = await _projectService.UpdateProject(project, id);
                if (updatedProject == null)
                {
                    return NotFound();
                }
                return Ok(updatedProject);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            bool isDeletedProject = await _projectService.DeleteProject(id);
            if (isDeletedProject == false)
            {
                return NotFound();
            }
            return Ok("Project is deleted");
        }
        [HttpGet("Project-IT-HR")]
        public async Task<IEnumerable<DTOProject>> GetITHRProjects([FromQuery]int pageNumber){
            var projects = await _projectService.GetITHRProjects(pageNumber);
            return projects;
        }
        [HttpGet("Project-No-Employees-Working")]
        public async Task<IEnumerable<Project>> GetProjectsWithNoEmployees([FromQuery]int pageNumber){
            var projects = await _projectService.GetProjectsWithNoEmployees(pageNumber);
            return projects;
        }
    }
}
