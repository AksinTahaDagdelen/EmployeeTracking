using System;
using EmployeeTracking.Web.Data;
using EmployeeTracking.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracking.Web.Repositories
{
    public class DepartmentRepository : IDepartmentInterface
    {
        private readonly EmployeeTrackingDbContext employeeTrackingDbContext;

        public DepartmentRepository(EmployeeTrackingDbContext employeeTrackingDbContext)
        {
            this.employeeTrackingDbContext = employeeTrackingDbContext;
        }

        public async Task<Department?> AddAsync(Department department)
        {
            await employeeTrackingDbContext.AddAsync(department);
            await employeeTrackingDbContext.SaveChangesAsync();
            return department;
        }

        public async Task<Department?> DeleteAsync(Guid id)
        {
            var existingDepartment = await employeeTrackingDbContext.Departments.FindAsync(id);

            if (existingDepartment != null)
            {
                employeeTrackingDbContext.Departments.Remove(existingDepartment);
                await employeeTrackingDbContext.SaveChangesAsync();
                return existingDepartment;
            }
            return null;

        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await employeeTrackingDbContext.Departments.Include(x => x.Projects).ToListAsync();
        }

        public async Task<Department?> GetAsync(Guid id)
        {
            return await employeeTrackingDbContext.Departments.Include(x => x.Projects).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Department?> UpdateAsync(Department department)
        {
            var existingDepartment = await employeeTrackingDbContext.Departments.Include(x => x.Projects).FirstOrDefaultAsync(x => x.Id == department.Id);

            if (existingDepartment != null)
            {
                existingDepartment.Id = department.Id;
                existingDepartment.Name = department.Name;
                existingDepartment.Projects = department.Projects;
                existingDepartment.ImgUrl = department.ImgUrl;
                await employeeTrackingDbContext.SaveChangesAsync();
                return existingDepartment;

            }
            return null;
        }
    }
}

