using CompanyAPI.DTO;
using CompanyAPI.Interfaces;
using CompanyAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanySystemWebAPI.Services
{
    public class WorksOnService : IWorksOnService
    {
        private readonly CompanySystemContext _context;
        public WorksOnService(CompanySystemContext context)
        {
            _context = context;
        }
        public async Task<WorksOn> AddWorksOn(DTOWorksOn inputWorksOn)
        {
            if (!await _context.Projects.AnyAsync(pro => pro.ProjNo == inputWorksOn.ProjNo))
            {
                throw new ArgumentException("Invalid Project ID");
            }
            if (!await _context.Employees.AnyAsync(e => e.EmpNo == inputWorksOn.EmpNo))
            {
                throw new ArgumentException("Invalid Employee ID");
            }
            var newWorksOn = new WorksOn
            {
                EmpNo = inputWorksOn.EmpNo,
                ProjNo = inputWorksOn.ProjNo,
                DateWorked = inputWorksOn.DateWorked,
                Hoursworked = inputWorksOn.Hoursworked
            };
            await _context.WorksOns.AddAsync(newWorksOn);
            await _context.SaveChangesAsync();
            return newWorksOn;
        }
        public async Task<IEnumerable<WorksOn>> GetAllWorksOns(int pageNumber)
        {
            int pageSize = 10;
            if (pageNumber < 1) pageNumber = 1;
            var worksOns = await _context.WorksOns
                .OrderBy(w => w.WorkNo)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return worksOns;
        }
        public async Task<WorksOn> GetWorksOnById(int id)
        {
            WorksOn chosenWorksOn = await _context.WorksOns.FirstOrDefaultAsync(foundWorksOn => foundWorksOn.WorkNo == id);
            return chosenWorksOn;
        }
        public async Task<WorksOn> UpdateWorksOn(DTOWorksOn worksOn, int id)
        {
            if (!await _context.Projects.AnyAsync(pro => pro.ProjNo == worksOn.ProjNo))
            {
                throw new ArgumentException("Invalid Project ID");
            }
            if (!await _context.Employees.AnyAsync(e => e.EmpNo == worksOn.EmpNo))
            {
                throw new ArgumentException("Invalid Employee ID");
            }
            var foundWorksOn = await GetWorksOnById(id);
            if (foundWorksOn is null)
            {
                return null;
            }
            foundWorksOn.ProjNo = worksOn.ProjNo;
            foundWorksOn.EmpNo = worksOn.EmpNo;
            foundWorksOn.DateWorked = worksOn.DateWorked;
            foundWorksOn.Hoursworked = worksOn.Hoursworked;
            await _context.SaveChangesAsync();
            return foundWorksOn;
        }
        public async Task<bool> DeleteWorksOn(int id)
        {
            var foundWorksOn = await GetWorksOnById(id);
            if (foundWorksOn is null)
            {
                return false;
            }
            _context.WorksOns.Remove(foundWorksOn);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<object> GetTotalHoursWorkedEmployee(int pageNumber)
        {
            int pageSize = 10;
            if (pageNumber < 1) pageNumber = 1;
            var totalHoursWorkedPerEmployee = await _context.WorksOns
        .GroupBy(w => w.EmpNo)
        .Select(g => new
        {
            Employee = _context.Employees
                .Where(e => e.EmpNo == g.Key)
                .Select(e => new
                {
                    Name = e.Fname + " " + e.Lname,
                })
                .FirstOrDefault(),
            TotalHoursWorked = g.Sum(w => w.Hoursworked),
            Projects = _context.Projects
                .Where(p => _context.WorksOns.Any(w => w.EmpNo == g.Key && w.ProjNo == p.ProjNo))
                .Select(p => p.ProjName)
                .ToList()
        })
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
            return totalHoursWorkedPerEmployee;
        }
    }
}
