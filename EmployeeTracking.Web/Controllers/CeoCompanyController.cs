using System;
using System.Collections.Generic;
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
    [Authorize(Roles = "Ceo")]
    public class CeoCompanyController : Controller
    {
        private readonly ICompanyInterface companyInterface;
        private readonly IDepartmentInterface departmentInterface;
        private readonly IEmployeeInterface employeeInterface;
        private readonly IProjectInterface projectInterface;

        public CeoCompanyController(ICompanyInterface companyInterface, IDepartmentInterface departmentInterface, IEmployeeInterface employeeInterface, IProjectInterface projectInterface)
        {
            this.companyInterface = companyInterface;
            this.departmentInterface = departmentInterface;
            this.employeeInterface = employeeInterface;
            this.projectInterface = projectInterface;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var department = await departmentInterface.GetAllAsync();
            var project = await projectInterface.GetAllAsync();
            var employee = await employeeInterface.GetAllAsync();

            var model = new AddCompanyRequest
            {
                Departments = department.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Projects = project.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Employees = employee.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCompanyRequest addCompanyRequest)
        {
            var company = new Company
            {
                Name = addCompanyRequest.Name,
                FoundationDate = addCompanyRequest.FoundationDate,
                ImgUrl = addCompanyRequest.ImgUrl
                

            };

            var selectedDepartments = new List<Department>();
            var selectedEmployees = new List<Employee>();
            var selectedProjects = new List<Project>();

            foreach (var selectedDepartmentId in addCompanyRequest.SelectedDepartments)
            {
                var selectedDepartmentIdAsGuid = Guid.Parse(selectedDepartmentId);
                var existingDepartment = await departmentInterface.GetAsync(selectedDepartmentIdAsGuid);

                if (existingDepartment != null)
                {
                    selectedDepartments.Add(existingDepartment);
                }
            }
            foreach (var selectedEmployeeId in addCompanyRequest.SelectedEmployees)
            {
                var selectedEmployeeIdAsGuid = Guid.Parse(selectedEmployeeId);
                var existingEmployee = await employeeInterface.GetAsync(selectedEmployeeIdAsGuid);

                if (existingEmployee != null)
                {
                    selectedEmployees.Add(existingEmployee);
                }
            }
            foreach (var selectedProjectId in addCompanyRequest.SelectedProjects)
            {
                var selectedProjectIdAsGuid = Guid.Parse(selectedProjectId);
                var existingProject = await projectInterface.GetAsync(selectedProjectIdAsGuid);

                if (existingProject != null)
                {
                    selectedProjects.Add(existingProject);
                }
            }
            company.Departments = selectedDepartments;
            company.Employees = selectedEmployees;
            company.Projects = selectedProjects;

            await companyInterface.AddAsync(company);

            return RedirectToAction("Add");
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var company = await companyInterface.GetAllAsync();
            return View(company);
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            var company = await companyInterface.GetAsync(id);
            var departmentsFromDomainModel = await departmentInterface.GetAllAsync();
            var employeesFromDomainModel = await employeeInterface.GetAllAsync();
            var projectsFromDomainModel = await projectInterface.GetAllAsync();
      
            if (company != null)
            {
                var model = new EditCompanyRequest
                {
                    Id = company.Id,
                    Name = company.Name,
					ImgUrl = company.ImgUrl,

					FoundationDate = company.FoundationDate,
                    Departments = departmentsFromDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    Employees = employeesFromDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    Projects = projectsFromDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedDepartments = company.Departments.Select(x => x.Id.ToString()).ToArray(),
                    SelectedEmployees = company.Employees.Select(x => x.Id.ToString()).ToArray(),
                    SelectedProjects = company.Projects.Select(x => x.Id.ToString()).ToArray()

                };
                return View(model);
            }

            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditCompanyRequest editCompanyRequest)
        {
            //view modeldan gelenler ile domain modeli maplayelim.
            var companiesFromDomainModel = new Company
            {
                Id = editCompanyRequest.Id,
                Name = editCompanyRequest.Name,
                FoundationDate = editCompanyRequest.FoundationDate,
                ImgUrl = editCompanyRequest.ImgUrl,

            };

            var selectedDepartments = new List<Department>();
            foreach (var selectedDepartment in editCompanyRequest.SelectedDepartments)
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
            var selectedEmployees = new List<Employee>();
            foreach (var selectedEmployee in editCompanyRequest.SelectedEmployees)
            {
                if (Guid.TryParse(selectedEmployee, out var employee))
                {
                    var foundEmployee = await employeeInterface.GetAsync(employee);
                    if (foundEmployee != null)
                    {
                        selectedEmployees.Add(foundEmployee);
                    }
                }
            }
            var selectedProjects = new List<Project>();
            foreach (var selectedProject in editCompanyRequest.SelectedProjects)
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
            companiesFromDomainModel.Departments = selectedDepartments;
            companiesFromDomainModel.Employees = selectedEmployees;
            companiesFromDomainModel.Projects = selectedProjects;


            var updatedCompany = await companyInterface.UpdateAsync(companiesFromDomainModel);

            if (updatedCompany != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditCompanyRequest editCompanyRequest)
        {
            var deletedCompany = await companyInterface.DeleteAsync(editCompanyRequest.Id);

            if (deletedCompany != null)
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit", new { id = editCompanyRequest.Id });

        }
    }
}

