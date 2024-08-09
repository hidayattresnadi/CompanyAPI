using CompanyAPI.DTO;
using CompanyAPI.Interfaces;
using CompanyAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanySystemWebAPI.Services
{
    public class ProjectService : IProjectService
    {
        private readonly CompanySystemContext _context;
        public ProjectService(CompanySystemContext context)
        {
            _context = context;
        }
        public async Task<bool> ValidateDupplicateProjectName(string projectName)
        {
            bool isDupplicate = _context.Projects.Any(p => p.ProjName == projectName);
            return isDupplicate;
        }
        public async Task<Project> AddProject(DTOProject inputProject)
        {
            if (!await _context.Departments.AnyAsync(d => d.DeptNo == inputProject.DeptNo))
            {
                throw new ArgumentException("Invalid Department ID");
            }
            bool isDupplicate = await ValidateDupplicateProjectName(inputProject.ProjName);
            if (isDupplicate)
            {
                throw new ArgumentException("Project Name is Already Exist");
            }
            var newProject = new Project
            {
                DeptNo = inputProject.DeptNo,
                ProjName = inputProject.ProjName
            };
            await _context.Projects.AddAsync(newProject);
            await _context.SaveChangesAsync();
            return newProject;
        }
        public async Task<IEnumerable<Project>> GetAllProjects(int pageNumber)
        {
            int pageSize = 10;
            if (pageNumber < 1) pageNumber = 1;
            var projects = await _context.Projects.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return projects;
        }
        public async Task<Project> GetProjectById(int id)
        {
            Project chosenProject = await _context.Projects.FirstOrDefaultAsync(foundProject => foundProject.ProjNo == id);
            return chosenProject;
        }
        public async Task<Project> UpdateProject(DTOProject project, int id)
        {
            if (!await _context.Departments.AnyAsync(d => d.DeptNo == project.DeptNo))
            {
                throw new ArgumentException("Invalid Department ID");
            }
            var foundProject = await GetProjectById(id);
            if (foundProject is null)
            {
                return null;
            }
            bool isDupplicate = await ValidateDupplicateProjectName(project.ProjName);
            if (isDupplicate)
            {
                throw new ArgumentException("Project Name is Already Exist");
            }
            foundProject.ProjName = project.ProjName;
            foundProject.DeptNo = project.DeptNo;
            await _context.SaveChangesAsync();
            return foundProject;
        }
        public async Task<bool> DeleteProject(int id)
        {
            var foundProject = await GetProjectById(id);
            if (foundProject is null)
            {
                return false;
            }
            _context.Projects.Remove(foundProject);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<DTOProject>> GetITHRProjects(int pageNumber)
        {
            int pageSize = 10;
            if (pageNumber < 1) pageNumber = 1;
            var projects = await _context.Projects.Include(p => p.DeptNoNavigation)
                .Where(p => p.DeptNoNavigation.DeptName == "IT" || p.DeptNoNavigation.DeptName == "HR")
                .Select(p => new DTOProject
                {
                    ProjNo = p.ProjNo,
                    DeptNo = p.DeptNo,
                    ProjName = p.ProjName
                }).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return projects;
        }
        public async Task<IEnumerable<Project>> GetProjectsWithNoEmployees(int pageNumber)
        {
            int pageSize = 10;
            if (pageNumber < 1) pageNumber = 1;
            var projectsWithNoEmployees = await _context.Projects
                .Select(p => new
                {
                    Project = p,
                    HasEmployees = _context.WorksOns.Any(w => w.ProjNo == p.ProjNo)
                })
                .Where(p => !p.HasEmployees)
                .Select(p => p.Project)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .ToListAsync();
            return projectsWithNoEmployees;
        }
    }
}
