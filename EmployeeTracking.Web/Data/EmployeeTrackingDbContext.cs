using System;
using EmployeeTracking.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracking.Web.Data
{
    public class EmployeeTrackingDbContext : DbContext
    {
        public EmployeeTrackingDbContext(DbContextOptions<EmployeeTrackingDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Company> Companies { get; set; }
    }
}

