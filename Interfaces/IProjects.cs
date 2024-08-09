using CompanyAPI.DTO;
using CompanyAPI.Models;

namespace CompanyAPI.Interfaces
{
    public interface IProjectService
    {
        Task<Project> AddProject(DTOProject project);
        Task<IEnumerable<Project>> GetAllProjects(int pageNumber);
        Task<Project> GetProjectById(int id);
        Task<Project> UpdateProject(DTOProject project, int id);
        Task<bool> DeleteProject(int id);
        Task<IEnumerable<DTOProject>> GetITHRProjects(int pageNumber);
        Task<IEnumerable<Project>> GetProjectsWithNoEmployees(int pageNumber);
    }
}
