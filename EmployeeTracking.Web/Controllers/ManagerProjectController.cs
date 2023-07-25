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


namespace EmployeeTracking.Web.Controllers
{
    //[Authorize(Roles = "Manager")]
    [Authorize(Roles = "Manager")]
    public class ManagerProjectController : Controller
    {
        private readonly IProjectInterface projectInterface;

        public ManagerProjectController(IProjectInterface projectInterface)
        {
            this.projectInterface = projectInterface;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProjectRequest addProjectRequest)
        {
            var project = new Project
            {
                Name = addProjectRequest.Name,
                ImgUrl = addProjectRequest.ImgUrl,
                Note=addProjectRequest.Note,
            };

            await projectInterface.AddAsync(project);

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var Projects = await projectInterface.GetAllAsync();
            return View(Projects);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var project = await projectInterface.GetAsync(id);

            if (project != null)
            {
                var editProjectReq = new EditProjectRequest
                {
                    Id = project.Id,
                    Name = project.Name,
                    ImgUrl = project.ImgUrl,
					Note = project.Note
                };
                return View(editProjectReq);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProjectRequest editProjectRequest)
        {
            var project = new Project
            {
                Id = editProjectRequest.Id,
                Name = editProjectRequest.Name,
                ImgUrl = editProjectRequest.ImgUrl,
				Note = editProjectRequest.Note  
            };


            var updatedProject = await projectInterface.UpdateAsync(project);
            if (updatedProject != null)
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit", new { id = editProjectRequest.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditProjectRequest editProjectRequest)
        {

            var deletedProject = await projectInterface.DeleteAsync(editProjectRequest.Id);
            if (deletedProject != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = editProjectRequest.Id });
        }
    }
}

