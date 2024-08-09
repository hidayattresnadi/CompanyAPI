using CompanyAPI.DTO;
using CompanyAPI.Interfaces;
using CompanyAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly CompanySystemContext _context;
        public EmployeeService(CompanySystemContext context)
        {
            _context = context;
        }
        public async Task<bool> ValidateDupplicateEmployee(string firstName, string LastName){
            bool isDupplicate = _context.Employees.Any(e => e.Fname == firstName && e.Lname == LastName);
            return isDupplicate;
        }
        public async Task<Employee> AddEmployee(DTOEmployee inputEmployee)
        {
            if (inputEmployee.DeptNo != null)
            {
                if (!await _context.Departments.AnyAsync(d => d.DeptNo == inputEmployee.DeptNo))
                {
                    throw new ArgumentException("Invalid Department ID");
                }
            }
            bool isDupplicate = await ValidateDupplicateEmployee(inputEmployee.Fname,inputEmployee.Lname);
            if (isDupplicate){
                throw new ArgumentException("First Name and Last Name are already exist");
            }
            var newEmployee = new Employee
            {
                Fname = inputEmployee.Fname,
                Lname = inputEmployee.Lname,
                Address = inputEmployee.Address,
                Dob = inputEmployee.Dob,
                Sex = inputEmployee.Sex,
                Position = inputEmployee.Position,
                DeptNo = inputEmployee.DeptNo
            };
            await _context.Employees.AddAsync(newEmployee);
            await _context.SaveChangesAsync();
            return newEmployee;
        }
        public async Task<IEnumerable<Employee>> GetAllEmployees(int pageNumber)
        {
            int pageSize = 10;
            if (pageNumber <1) pageNumber=1;
            var employees = await _context.Employees.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return employees;
        }
        public async Task<Employee> GetEmployeeById(int id)
        {
            Employee chosenEmployee = await _context.Employees.FirstOrDefaultAsync(foundEmployee => foundEmployee.EmpNo == id);
            return chosenEmployee;
        }
        public async Task<Employee> UpdateEmployee(DTOEmployee employee, int id)
        {
            if (!await _context.Departments.AnyAsync(d => d.DeptNo == employee.DeptNo))
            {
                throw new ArgumentException("Invalid Department ID");
            }
            if (employee.Position == "Manager")
            {
                if (await _context.Departments.AnyAsync(d => d.MgrEmpNo == id))
                {
                    throw new ArgumentException("Manager Id has been asigned, cannot update employee position");
                }
            }
            var foundEmployee = await GetEmployeeById(id);
            if (foundEmployee is null)
            {
                return null;
            }
            bool isDupplicate = await ValidateDupplicateEmployee(employee.Fname,employee.Lname);
            if (isDupplicate){
                throw new ArgumentException("First Name and Last Name are already exist");
            }
            foundEmployee.DeptNo = employee.DeptNo;
            foundEmployee.Position = employee.Position;
            foundEmployee.Fname = employee.Fname;
            foundEmployee.Lname = employee.Lname;
            foundEmployee.Sex = employee.Sex;
            foundEmployee.Address = employee.Address;
            foundEmployee.Dob = employee.Dob;
            await _context.SaveChangesAsync();
            return foundEmployee;
        }
        public async Task<bool> DeleteEmployee(int id)
        {
            var foundEmployee = await GetEmployeeById(id);
            if (foundEmployee is null)
            {
                return false;
            }
            _context.Employees.Remove(foundEmployee);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Employee>> GetEmployeesBorne1980(int pageNumber)
        {
            var startDate = new DateOnly(1980, 1, 1);
            var endDate = new DateOnly(1990,1,1);
            int pageSize = 10;
            if (pageNumber < 1) pageNumber =1;
            var employees = await _context.Employees.Where(x => x.Dob >= startDate && x.Dob <= endDate)
                            .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return employees; 
        }

        public async Task<IEnumerable<Employee>> GetFemaleEmployeesBorne1980(int pageNumber)
        {
            var startDate = new DateOnly(1990, 1, 1);
            int pageSize = 10;
            if (pageNumber < 1) pageNumber =1;
            var employees = await _context.Employees.Where(x => x.Dob > startDate && x.Sex == "Female")
                            .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return employees;
        }
        public async Task<IEnumerable<Employee>> GetNonManagerEmployees(int pageNumber)
        {
            int pageSize = 10;
            if (pageNumber < 1) pageNumber =1;
            var nonManagers = await _context.Employees
                .GroupJoin(_context.Departments, employee => employee.EmpNo, department => department.MgrEmpNo,
                (employee, departments) => new { Employee = employee, Departments = departments })
                .Where(ed => !ed.Departments.Any())
                .Select(ed => ed.Employee)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return nonManagers;
        }
        public async Task<IEnumerable<Employee>> GetBRICSEmployees(int pageNumber)
        {
            int pageSize = 10;
            if (pageNumber < 1) pageNumber =1;
            var countries = new[] { "Brazil", "China", "Russia", "India", "South Africa" }.Select(c => c.ToLower()).ToArray();
            var employees = await _context.Employees.Where(x => countries.Any(country => x.Address.ToLower()
                .Contains(country))).OrderBy(x => x.Lname).Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();
            return employees;
        }
        public async Task<IEnumerable<Employee>> GetITEmployees(int pageNumber)
        {
            int pageSize = 10;
            if (pageNumber < 1) pageNumber =1;
            var itEmployees = await _context.Employees.Join(_context.Departments, employee => employee.DeptNo,
                department => department.DeptNo, (employee, department) => new { Employee = employee, Department = department })
                .Where(ed => ed.Department.DeptName == "IT").Select(ed => ed.Employee)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return itEmployees;
        }
    }
}