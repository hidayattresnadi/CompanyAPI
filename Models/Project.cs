using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CompanyAPI.Models;

public partial class Project
{
    [Key]
    public int ProjNo { get; set; }

    public string ProjName { get; set; } = null!;

    public int DeptNo { get; set; }

    [ForeignKey("DeptNo")]
    [InverseProperty("Projects")]
    public virtual Department DeptNoNavigation { get; set; } = null!;

    [InverseProperty("ProjNoNavigation")]
    public virtual ICollection<WorksOn> WorksOns { get; set; } = new List<WorksOn>();
}
