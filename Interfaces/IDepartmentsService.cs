using CompanyAPI.DTO;
using CompanyAPI.Models;

namespace CompanyAPI.Interfaces
{
    public interface IDepartmentsService
    {
        Task<Department> AddDepartment(DTODepartment department);
        Task<IEnumerable<Department>> GetAllDepartments(int pageNumber);
        Task<Department> GetDepartmentById(int id);
        Task<Department> UpdateDepartment(DTODepartment department, int id);
        Task<bool> DeleteDepartment(int id);
        Task<IEnumerable<Employee>> GetFemaleManagers(int pageNumber);
        Task<int> GetCountingOfFemaleManagers();
        Task<IEnumerable<object>> GetDepartmentsWithMoreThanTenEmployees(int pageNumber);
    }
}
