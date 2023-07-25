using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using EmployeeTracking.Web.Models.Domain;
using EmployeeTracking.Web.Models.ViewModels;
using EmployeeTracking.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeTracking.Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerEmployeeController : Controller
    {
        private readonly IProjectInterface projectInterface;
        private readonly IDepartmentInterface departmentInterface;
        private readonly ICompanyInterface companyInterface;
        private readonly IEmployeeInterface employeeInterface;

        public ManagerEmployeeController(IProjectInterface projectInterface, IDepartmentInterface departmentInterface, ICompanyInterface companyInterface, IEmployeeInterface employeeInterface)
        {
            this.projectInterface = projectInterface;
            this.departmentInterface = departmentInterface;
            this.companyInterface = companyInterface;
            this.employeeInterface = employeeInterface;
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var projects = await projectInterface.GetAllAsync();
            var departments = await departmentInterface.GetAllAsync();
            var companies = await companyInterface.GetAllAsync();

            var model = new AddEmployeeRequest
            {
                Departments = departments.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Projects = projects.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Companies = companies.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })

            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeRequest addEmployeeRequest)
        {
            var employee = new Employee
            {
                Name = addEmployeeRequest.Name,
                Surname = addEmployeeRequest.Surname,
                Address = addEmployeeRequest.Address,
                PhoneNumber = addEmployeeRequest.PhoneNumber,
                Salary = addEmployeeRequest.Salary,
                ImgUrl = addEmployeeRequest.ImgUrl,
                Note=addEmployeeRequest.Note,


            };

            var selectedDepartments = new List<Department>();
            foreach (var selectedDepartmentId in addEmployeeRequest.SelectedDepartments)
            {
                var selectedDepartmentIdAsGuid = Guid.Parse(selectedDepartmentId);
                var existingDepartment = await departmentInterface.GetAsync(selectedDepartmentIdAsGuid);

                if (existingDepartment != null)
                {
                    selectedDepartments.Add(existingDepartment);
                }
            }
            var selectedCompanies = new List<Company>();
            foreach (var selectedCompanyId in addEmployeeRequest.SelectedCompanies)
            {
                var selectedCompanyIdAsGuid = Guid.Parse(selectedCompanyId);
                var existingCompany = await companyInterface.GetAsync(selectedCompanyIdAsGuid);

                if (existingCompany != null)
                {
                    selectedCompanies.Add(existingCompany);
                }
            }
            var selectedProjects = new List<Project>();
            foreach (var selectedProjectId in addEmployeeRequest.SelectedProjects)
            {
                var selectedProjectIdIdAsGuid = Guid.Parse(selectedProjectId);
                var existingProject = await projectInterface.GetAsync(selectedProjectIdIdAsGuid);

                if (existingProject != null)
                {
                    selectedProjects.Add(existingProject);
                }
            }

            //blogPostumuzun taglerine bu tagleri eşitleyelim.

            employee.Departments = selectedDepartments;
            employee.Projects = selectedProjects;
            employee.Companies = selectedCompanies;

            await employeeInterface.AddAsync(employee);

            return RedirectToAction("Add");
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var employee = await employeeInterface.GetAllAsync();
            return View(employee);
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            var employee = await employeeInterface.GetAsync(id);
            var departmentsFromDomainModel = await departmentInterface.GetAllAsync();
            var companiesFromDomainModel = await companyInterface.GetAllAsync();
            var projectsFromDomainModel = await projectInterface.GetAllAsync();

            if (employee != null)
            {
                //domain modeldaki verilerimizi viewmodela mapliyoruz.
                var model = new EditEmployeeRequest
                {
                    Id = employee.Id,
                    Address = employee.Address,
                    Name = employee.Name,
                    Surname = employee.Surname,
                    PhoneNumber = employee.PhoneNumber,
                    Salary = employee.Salary,
                    ImgUrl = employee.ImgUrl,
                    Note= employee.Note,


                    Departments = departmentsFromDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    Projects = projectsFromDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    Companies = companiesFromDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedCompanies = employee.Companies.Select(x => x.Id.ToString()).ToArray(),
                    SelectedDepartments = employee.Departments.Select(x => x.Id.ToString()).ToArray(),
                    SelectedProjects = employee.Projects.Select(x => x.Id.ToString()).ToArray()
                };
                return View(model);
            }

            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditEmployeeRequest editEmployeeRequest)
        {

            var employeeDomainModel = new Employee
            {
                Id = editEmployeeRequest.Id,
                Address = editEmployeeRequest.Address,
                Name = editEmployeeRequest.Name,
                Surname = editEmployeeRequest.Surname,
                PhoneNumber = editEmployeeRequest.PhoneNumber,
                Salary = editEmployeeRequest.Salary,
                ImgUrl=editEmployeeRequest.ImgUrl,
                Note = editEmployeeRequest.Note,
            };

            var selectedDepartments = new List<Department>();
            var selectedProjects = new List<Project>();
            var selectedCompanies = new List<Company>();
            foreach (var selectedDepartment in editEmployeeRequest.SelectedDepartments)
            {
                if (Guid.TryParse(selectedDepartment, out var department))
                {
                    var foundDepartment = await departmentInterface.GetAsync(department);
                    if (foundDepartment != null)
                    {
                        selectedDepartments.Add(foundDepartment);
                    }
                }
            }
            foreach (var selectedProject in editEmployeeRequest.SelectedProjects)
            {
                if (Guid.TryParse(selectedProject, out var project))
                {
                    var foundProject = await projectInterface.GetAsync(project);
                    if (foundProject != null)
                    {
                        selectedProjects.Add(foundProject);
                    }
                }
            }
            foreach (var selectedCompany in editEmployeeRequest.SelectedCompanies)
            {
                if (Guid.TryParse(selectedCompany, out var company))
                {
                    var foundCompany = await departmentInterface.GetAsync(company);
                    if (foundCompany != null)
                    {
                        selectedDepartments.Add(foundCompany);
                    }
                }
            }
            employeeDomainModel.Projects = selectedProjects;
            employeeDomainModel.Departments = selectedDepartments;
            employeeDomainModel.Companies = selectedCompanies;


            var updatedBlog = await employeeInterface.UpdateAsync(employeeDomainModel);

            if (updatedBlog != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditEmployeeRequest editEmployeeRequest)
        {
            var deletedBlogPost = await employeeInterface.DeleteAsync(editEmployeeRequest.Id);

            if (deletedBlogPost != null)
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit", new { id = editEmployeeRequest.Id });

        }
    }
}

