using System.ComponentModel.DataAnnotations;

namespace CompanyAPI.DTO
{
    public class DTOProject
    {
        public int ProjNo { get; set; }
        [Required(ErrorMessage = "Project Name is required")]
        public string ProjName { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "DepartmentId is required and must be a positive integer")]
        public int DeptNo { get; set; }
    }
}