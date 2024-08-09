using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CompanyAPI.Models;

public partial class Employee
{
    [Key]
    public int EmpNo { get; set; }

    [Column("FName")]
    public string Fname { get; set; } = null!;

    [Column("LName")]
    public string Lname { get; set; } = null!;

    public string Address { get; set; } = null!;

    [Column("DOB")]
    public DateOnly Dob { get; set; }

    public string Sex { get; set; } = null!;

    public string Position { get; set; } = null!;

    public int? DeptNo { get; set; }

    [InverseProperty("MgrEmpNoNavigation")]
    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    [ForeignKey("DeptNo")]
    [InverseProperty("Employees")]
    public virtual Department? DeptNoNavigation { get; set; }

    [InverseProperty("EmpNoNavigation")]
    public virtual ICollection<WorksOn> WorksOns { get; set; } = new List<WorksOn>();
}
