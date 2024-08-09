using System.ComponentModel.DataAnnotations;

namespace CompanyAPI.DTO
{
    public class DTOEmployee
    {
        public DateOnly Dob { get; set; }
        [Required(ErrorMessage = "Sex is required")]
        public string Sex { get; set; }
        [Required(ErrorMessage = "Position is required")]
        public string Position { get; set; } = null!;
        public int? DeptNo { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string Lname { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string Fname { get; set; } = null!;
    }
}
