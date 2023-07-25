using System;
using System.ComponentModel.Design;
using Azure;
using EmployeeTracking.Web.Data;
using EmployeeTracking.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracking.Web.Repositories
{
    public class CompanyRepository : ICompanyInterface
    {
        private readonly EmployeeTrackingDbContext employeeTrackingDbContext;

        public CompanyRepository(EmployeeTrackingDbContext employeeTrackingDbContext)
        {
            this.employeeTrackingDbContext = employeeTrackingDbContext;
        }

        public async Task<Company?> AddAsync(Company company)
        {
            await employeeTrackingDbContext.AddAsync(company);
            await employeeTrackingDbContext.SaveChangesAsync();
            return company;
        }

        public async Task<Company?> DeleteAsync(Guid id)
        {
            var existingCompany = await employeeTrackingDbContext.Companies.FindAsync(id);

            if (existingCompany != null)
            {
                employeeTrackingDbContext.Remove(existingCompany);
                await employeeTrackingDbContext.SaveChangesAsync();

                return existingCompany;
            }
            return null;
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            var department = await employeeTrackingDbContext.Companies.Include(x => x.Departments).ToListAsync();
            var project = await employeeTrackingDbContext.Companies.Include(x => x.Projects).ToListAsync();
            var employee = await employeeTrackingDbContext.Companies.Include(x => x.Employees).ToListAsync();
            //var list = new List<Company>();
            //list.Add(department);
            //list.Add(project);
            //list.Add(employee);
            //return (IEnumerable<Company>)list;
            //var company = employeeTrackingDbContext.Companies
            //             .Include("Companies.Departments")
            //             .Include("Companies.Projects")
            //             .Include("Companies.Employees").ToList();
            //var company = await employeeTrackingDbContext.Companies
            //                .Include(co => co.Employees.Select(emp => emp.Name))
            //                .Include(co => co.Departments.Select(emp => emp.Name))
            //                .Include(co => co.Projects.Select(emp => emp.Name)).ToListAsync();
            var company = employeeTrackingDbContext.Companies.Include("Projects").Include("Employees").Include("Departments").ToList();
            return company;
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return await employeeTrackingDbContext.Departments.ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await employeeTrackingDbContext.Projects.ToListAsync();
        }

        public async Task<Company?> GetAsync(Guid id)
        {
            return await employeeTrackingDbContext.Companies.Include("Projects").Include("Employees").Include("Departments").FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<Company?> UpdateAsync(Company company)
        {
            var existingCompany = await employeeTrackingDbContext.Companies.Include("Projects").Include("Employees").Include("Departments").FirstOrDefaultAsync(x => x.Id == company.Id);
            //var company = employeeTrackingDbContext.Companies.Include("Projects").Include("Employees").Include("Departments").ToList();

            if (existingCompany != null)
            {
                existingCompany.Id = company.Id;
                existingCompany.Name = company.Name;
                existingCompany.FoundationDate = company.FoundationDate;
                existingCompany.Projects = company.Projects;
                existingCompany.Employees = company.Employees;
                existingCompany.Departments = company.Departments;
                existingCompany.ImgUrl = company.ImgUrl;
                await employeeTrackingDbContext.SaveChangesAsync();
                return existingCompany;
            }
            return null;
        }
    }
}

