using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CompanyAPI.Models;

public partial class CompanySystemContext : DbContext
{
    public CompanySystemContext()
    {
    }

    public CompanySystemContext(DbContextOptions<CompanySystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<WorksOn> WorksOns { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasOne(d => d.MgrEmpNoNavigation).WithMany(p => p.Departments).HasConstraintName("FK_Department_Employees_MgrEmpNo");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasOne(d => d.DeptNoNavigation).WithMany(p => p.Employees).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WorksOn>(entity =>
        {
            entity.HasOne(d => d.ProjNoNavigation).WithMany(p => p.WorksOns).HasConstraintName("FK_WorksOns_Projects_ProjectNo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
