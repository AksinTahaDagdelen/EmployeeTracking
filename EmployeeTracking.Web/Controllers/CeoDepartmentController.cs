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
    [Authorize(Roles = "Ceo")]
    public class CeoDepartmentController : Controller
    {
        private readonly IDepartmentInterface departmentInterface;
        private readonly IProjectInterface projectInterface;

        public CeoDepartmentController(IDepartmentInterface departmentInterface, IProjectInterface projectInterface)
        {
            this.departmentInterface = departmentInterface;
            this.projectInterface = projectInterface;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var project = await projectInterface.GetAllAsync();

            var model = new AddDepartmentRequest
            {
                Projects = project.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddDepartmentRequest addDepartmentRequest)
        {
            var department = new Department
            {
                Name = addDepartmentRequest.Name,
                ImgUrl = addDepartmentRequest.ImgUrl,
            };
            var selectedProjects = new List<Project>();
            foreach (var selectedProjectId in addDepartmentRequest.SelectedProjects)
            {
                var selectedProjectIdAsGuid = Guid.Parse(selectedProjectId);
                var existingProject = await projectInterface.GetAsync(selectedProjectIdAsGuid);

                if (existingProject != null)
                {
                    selectedProjects.Add(existingProject);
                }
            }
            department.Projects = selectedProjects;

            await departmentInterface.AddAsync(department);

            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var department = await departmentInterface.GetAllAsync();
            return View(department);
        }


        //Edit Get Action
        public async Task<IActionResult> Edit(Guid id)
        {
            var department = await departmentInterface.GetAsync(id);
            var projectFromDomainModel = await projectInterface.GetAllAsync();

            if (department != null)
            {
                //domain modeldaki verilerimizi viewmodela mapliyoruz.
                var model = new EditDepartmentRequest
                {
                    Id = department.Id,
                    Name = department.Name,
                    ImgUrl= department.ImgUrl,
                    Projects = projectFromDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedProjects = department.Projects.Select(x => x.Id.ToString()).ToArray()
                };
                return View(model);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditDepartmentRequest editdepartmentRequest)
        {

            var departmentDomainModel = new Department
            {
                Id = editdepartmentRequest.Id,
                Name = editdepartmentRequest.Name,
                ImgUrl = editdepartmentRequest.ImgUrl
            };

            var selectedProjects = new List<Project>();
            foreach (var selectedProject in editdepartmentRequest.SelectedProjects)
            {
                if (Guid.TryParse(selectedProject, out var Project))
                {
                    var foundProject = await projectInterface.GetAsync(Project);
                    if (foundProject != null)
                    {
                        selectedProjects.Add(foundProject);
                    }
                }
            }
            departmentDomainModel.Projects = selectedProjects;


            var updatedDepartment = await departmentInterface.UpdateAsync(departmentDomainModel);

            if (updatedDepartment != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditDepartmentRequest editdepartmentRequest)
        {
            var deleteddepartment = await departmentInterface.DeleteAsync(editdepartmentRequest.Id);

            if (deleteddepartment != null)
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit", new { id = editdepartmentRequest.Id });

        }
    }
}

