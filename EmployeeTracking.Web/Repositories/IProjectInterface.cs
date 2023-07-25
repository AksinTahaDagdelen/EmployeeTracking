using System;
using Azure;
using EmployeeTracking.Web.Models.Domain;

namespace EmployeeTracking.Web.Repositories
{
    public interface IProjectInterface
    {
        Task<Project?> GetAsync(Guid id);
        Task<Project?> AddAsync(Project project);
        Task<Project?> UpdateAsync(Project project);
        Task<Project?> DeleteAsync(Guid id);
        Task<IEnumerable<Project>> GetAllAsync();
    }
}

