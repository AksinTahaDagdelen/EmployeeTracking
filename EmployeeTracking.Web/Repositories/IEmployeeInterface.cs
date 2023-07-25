using System;
using EmployeeTracking.Web.Models.Domain;

namespace EmployeeTracking.Web.Repositories
{
    public interface IEmployeeInterface
    {
        //Task<IEnumerable<Company>> GetAllCompaniesAsync();
        //Task<IEnumerable<Project>> GetAllProjectsAsync();
        //Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        Task<Employee?> GetAsync(Guid id);
        Task<Employee?> AddAsync(Employee employee);
        Task<Employee?> UpdateAsync(Employee employee);
        Task<Employee?> DeleteAsync(Guid id);
        Task<IEnumerable<Employee>> GetAllAsync();
    }
}

