using CompanyAPI.DTO;
using CompanyAPI.Models;

namespace CompanyAPI.Interfaces
{
    public interface IEmployeeService
    {
        Task<Employee> AddEmployee(DTOEmployee employee);
        Task<IEnumerable<Employee>> GetAllEmployees(int pageNumber);
        Task<Employee> GetEmployeeById(int id);
        Task<Employee> UpdateEmployee(DTOEmployee employee, int id);
        Task<bool> DeleteEmployee(int id);
        Task<IEnumerable<Employee>> GetEmployeesBorne1980(int pageNumber);
        Task<IEnumerable<Employee>> GetFemaleEmployeesBorne1980(int pageNumber);
        Task<IEnumerable<Employee>> GetNonManagerEmployees(int pageNumber);
        Task<IEnumerable<Employee>> GetBRICSEmployees(int pageNumber);
        Task<IEnumerable<Employee>> GetITEmployees(int pageNumber);
    }
}
