using System;
using EmployeeTracking.Web.Models.Domain;

namespace EmployeeTracking.Web.Repositories
{
    public interface IDepartmentInterface
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department?> GetAsync(Guid id);
        Task<Department?> AddAsync(Department department);
        Task<Department?> UpdateAsync(Department department);
        Task<Department?> DeleteAsync(Guid id);
    }
}

