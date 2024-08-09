using CompanyAPI.DTO;
using CompanyAPI.Models;

namespace CompanyAPI.Interfaces
{
    public interface IWorksOnService
    {
        Task<WorksOn> AddWorksOn(DTOWorksOn worksOn);
        Task<IEnumerable<WorksOn>> GetAllWorksOns(int pageNumber);
        Task<WorksOn> GetWorksOnById(int id);
        Task<WorksOn> UpdateWorksOn(DTOWorksOn workson, int id);
        Task<bool> DeleteWorksOn(int id);
        Task<object> GetTotalHoursWorkedEmployee(int pageNumber);
    }
}
