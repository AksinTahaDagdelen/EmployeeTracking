using System;
using EmployeeTracking.Web.Data;
using EmployeeTracking.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracking.Web.Repositories
{
    public class EmployeeRepository : IEmployeeInterface
    {
        private readonly EmployeeTrackingDbContext employeeTrackingDbContext;

        public EmployeeRepository(EmployeeTrackingDbContext employeeTrackingDbContext)
        {
            this.employeeTrackingDbContext = employeeTrackingDbContext;
        }
        public async Task<Employee?> AddAsync(Employee employee)
        {
            await employeeTrackingDbContext.AddAsync(employee);
            await employeeTrackingDbContext.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee?> DeleteAsync(Guid id)
        {
            var existingEmployee = await employeeTrackingDbContext.Employees.FindAsync(id);

            if (existingEmployee != null)
            {
                employeeTrackingDbContext.Employees.Remove(existingEmployee);
                await employeeTrackingDbContext.SaveChangesAsync();
                return existingEmployee;
            }
            return null;
        }
        //HATALI KSIIM TEKRAR BAKILACAK 
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {

            var company = employeeTrackingDbContext.Employees.Include("Projects").Include("Companies").Include("Departments").ToList();
            return company;

        }

        //public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        //{
        //    return await employeeTrackingDbContext.Employees.Include(x => x.Companies).ToListAsync();
        //}

        //public Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IEnumerable<Project>> GetAllProjectsAsync()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<Employee?> GetAsync(Guid id)
        {
            //TEKRAR BAKILACAK
            //return await employeeTrackingDbContext.Employees.Include(x => x.Projects).FirstOrDefaultAsync(x => x.Id == id);
            return await employeeTrackingDbContext.Employees.Include("Projects").Include("Companies").Include("Departments").FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Employee?> UpdateAsync(Employee employee)
        {
            var existingEmployee = await employeeTrackingDbContext.Employees.Include("Projects").Include("Companies").Include("Departments").FirstOrDefaultAsync(x => x.Id == employee.Id);

            if (existingEmployee != null)
            {
                existingEmployee.Id = employee.Id;
                existingEmployee.Address = employee.Address;
                existingEmployee.PhoneNumber = employee.PhoneNumber;
                existingEmployee.Surname = employee.Surname;
                existingEmployee.Name = employee.Name;
                existingEmployee.Salary = employee.Salary;
                existingEmployee.Companies = employee.Companies;
                existingEmployee.Departments = employee.Departments;
                existingEmployee.Projects = employee.Projects;
                existingEmployee.Note= employee.Note;
                existingEmployee.ImgUrl = employee.ImgUrl;




                await employeeTrackingDbContext.SaveChangesAsync();
                return existingEmployee;

            }
            return null;
        }
    }
}

