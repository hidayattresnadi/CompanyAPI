using System.ComponentModel.DataAnnotations;

namespace CompanyAPI.DTO
{
    public class DTOWorksOn
    {
        [Range(1, int.MaxValue, ErrorMessage = "EmployeeId is required and must be a positive integer")]
        public int EmpNo { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "ProjectId is required and must be a positive integer")]
        public int ProjNo { get; set; }
        public DateOnly DateWorked { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "HoursWorked is required and must be a positive integer")]
        public decimal Hoursworked { get; set; }
    }
}