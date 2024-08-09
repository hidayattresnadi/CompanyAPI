using CompanyAPI.Models;
using CompanyAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using CompanyAPI.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CompanyAPI.Services
{
    public class DepartmentService : IDepartmentsService
    {
        private readonly CompanySystemContext _context;
        public DepartmentService(CompanySystemContext context)
        {
            _context = context;
        }

        public async Task<bool> ValidateDupplicateDepartmentName(string departmentName)
        {
            bool isDupplicate = _context.Departments.Any(d => d.DeptName == departmentName);
            return isDupplicate;
        }
        public async Task<Department> AddDepartment(DTODepartment inputDepartment)
        {
            if (!await _context.Employees.AnyAsync(e => e.EmpNo == inputDepartment.MgrEmpNo))
            {
                throw new ArgumentException("Invalid Manager Employee ID");
            }
            if (await _context.Departments.AnyAsync(d => d.MgrEmpNo == inputDepartment.MgrEmpNo))
            {
                throw new ArgumentException("Invalid input Manager Employee ID, Manager has been asigned to another Department");
            }
            bool isDupplicate = await ValidateDupplicateDepartmentName(inputDepartment.DeptName);
            if (isDupplicate)
            {
                throw new ArgumentException("Department Name is already exist");
            }
            var department = new Department
            {
                DeptName = inputDepartment.DeptName,
                MgrEmpNo = inputDepartment.MgrEmpNo
            };
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
            return department;
        }
        public async Task<IEnumerable<Department>> GetAllDepartments(int pageNumber)
        {
            int pageSize = 10;
            if (pageNumber < 1) pageNumber = 1;
            var departments = await _context.Departments.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return departments;
        }
        public async Task<Department> GetDepartmentById(int id)
        {
            Department chosenDepartment = await _context.Departments.FirstOrDefaultAsync(foundDepartment => foundDepartment.DeptNo == id);
            return chosenDepartment;
        }
        public async Task<Department> UpdateDepartment(DTODepartment department, int id)
        {
            if (!await _context.Employees.AnyAsync(e => e.EmpNo == department.MgrEmpNo))
            {
                throw new ArgumentException("Invalid Manager Employee ID");
            }
            var foundDepartment = await GetDepartmentById(id);
            if (foundDepartment is null)
            {
                return null;
            }
            bool isDupplicate = await ValidateDupplicateDepartmentName(department.DeptName);
            if (isDupplicate)
            {
                throw new ArgumentException("Department Name is already exist");
            }
            foundDepartment.DeptName = department.DeptName;
            foundDepartment.MgrEmpNo = department.MgrEmpNo;
            await _context.SaveChangesAsync();
            return foundDepartment;
        }
        public async Task<bool> DeleteDepartment(int id)
        {
            var foundDepartment = await GetDepartmentById(id);
            if (foundDepartment is null)
            {
                return false;
            }
            _context.Departments.Remove(foundDepartment);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Employee>> GetFemaleManagers(int pageNumber)
        {
            int pageSize = 10;
            if (pageNumber < 1) pageNumber =1;
            var femaleManagers = await _context.Departments
                .Where(d => d.MgrEmpNoNavigation.Sex == "Female")
                .Select(d => d.MgrEmpNoNavigation)
                .OrderBy(e => e.Lname)
                .ThenBy(e => e.Fname)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return femaleManagers;
        }
        public async Task<int> GetCountingOfFemaleManagers()
        {
            var femaleManagers = await _context.Departments
                .Where(d => d.MgrEmpNoNavigation.Sex == "Female")
                .Select(d => d.MgrEmpNoNavigation)
                .OrderBy(e => e.Lname)
                .ThenBy(e => e.Fname)
                .ToListAsync();
            return femaleManagers.Count();
        }
        public async Task<IEnumerable<Employee>> GetManagersUnderFourty(int pageNumber)
        {
            int pageSize = 10;
            if (pageNumber < 1) pageSize = 1;
            var today = DateOnly.FromDateTime(DateTime.Today);
            var femaleManagers = await _context.Departments
                .Where(d => d.MgrEmpNoNavigation.Sex == "Female")
                .Select(d => d.MgrEmpNoNavigation)
                .OrderBy(e => e.Lname)
                .ThenBy(e => e.Fname)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return femaleManagers;
        }

        public async Task<IEnumerable<object>> GetDepartmentsWithMoreThanTenEmployees(int pageNumber)
        {
            int pageSize = 10;
            if (pageNumber < 1) pageNumber = 1;
            var departmentsWithMoreThanTenEmployees = await _context.Departments
                .Select(d => new
                {
                    Department = d,
                    EmployeeCount = d.Employees.Count()
                })
                .Where(d => d.EmployeeCount > 10)
                .Select(d => new
                {
                    d.Department.DeptNo,
                    d.Department.DeptName,
                    d.EmployeeCount
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return departmentsWithMoreThanTenEmployees;
        }
    }
}