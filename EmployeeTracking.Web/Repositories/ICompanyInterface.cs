using System;
using EmployeeTracking.Web.Models.Domain;

namespace EmployeeTracking.Web.Repositories
{
    public interface ICompanyInterface
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        Task<Company?> GetAsync(Guid id);
        Task<Company?> AddAsync(Company company);
        Task<Company?> UpdateAsync(Company company);
        Task<Company?> DeleteAsync(Guid id);
        Task<IEnumerable<Company>> GetAllAsync();
    }
}

