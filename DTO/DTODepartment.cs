using System.ComponentModel.DataAnnotations;

namespace CompanyAPI.DTO
{
    public class DTODepartment
    {
        [Required(ErrorMessage = "DeptName is required")]
        public string DeptName { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "ManagerId is required and must be a positive integer")]
        public int MgrEmpNo { get; set; }
    }
}
