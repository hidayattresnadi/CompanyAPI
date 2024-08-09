using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CompanyAPI.Models;

public partial class WorksOn
{
    [Key]
    public int WorkNo { get; set; }

    public int EmpNo { get; set; }

    public int ProjNo { get; set; }

    public DateOnly DateWorked { get; set; }

    public decimal Hoursworked { get; set; }

    [ForeignKey("EmpNo")]
    [InverseProperty("WorksOns")]
    public virtual Employee EmpNoNavigation { get; set; } = null!;

    [ForeignKey("ProjNo")]
    [InverseProperty("WorksOns")]
    public virtual Project ProjNoNavigation { get; set; } = null!;
}
