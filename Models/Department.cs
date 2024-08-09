using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CompanyAPI.Models;

public partial class Department
{
    [Key]
    public int DeptNo { get; set; }

    public string DeptName { get; set; } = null!;

    public int MgrEmpNo { get; set; }

    [InverseProperty("DeptNoNavigation")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    [ForeignKey("MgrEmpNo")]
    [InverseProperty("Departments")]
    public virtual Employee MgrEmpNoNavigation { get; set; } = null!;

    [InverseProperty("DeptNoNavigation")]
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
