using System;
using Azure;
using EmployeeTracking.Web.Data;
using EmployeeTracking.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracking.Web.Repositories
{
    public class ProjectRepository : IProjectInterface
    {
        private readonly EmployeeTrackingDbContext employeeTrackingDbContext;

        public ProjectRepository(EmployeeTrackingDbContext employeeTrackingDbContext)
        {
            this.employeeTrackingDbContext = employeeTrackingDbContext;
        }
        public async Task<Project?> AddAsync(Project project)
        {
            await employeeTrackingDbContext.AddAsync(project);
            await employeeTrackingDbContext.SaveChangesAsync();
            return project;
        }

        public async Task<Project?> DeleteAsync(Guid id)
        {
            var existingproject = await employeeTrackingDbContext.Projects.FindAsync(id);

            if (existingproject != null)
            {
                employeeTrackingDbContext.Remove(existingproject);
                await employeeTrackingDbContext.SaveChangesAsync();

                return existingproject;
            }
            return null;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await employeeTrackingDbContext.Projects.ToListAsync();
        }

        public async Task<Project?> GetAsync(Guid id)
        {
            return await employeeTrackingDbContext.Projects.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Project?> UpdateAsync(Project project)
        {
            var existingproject = await employeeTrackingDbContext.Projects.FindAsync(project.Id);

            if (existingproject != null)
            {
                existingproject.Name = project.Name;
                existingproject.Note = project.Note;
                existingproject.ImgUrl = project.ImgUrl;
                await employeeTrackingDbContext.SaveChangesAsync();

                return existingproject;
            }
            return null;
        }
    }
}

