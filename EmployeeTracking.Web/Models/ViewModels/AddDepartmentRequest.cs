using System;
using EmployeeTracking.Web.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeTracking.Web.Models.ViewModels
{
    public class AddDepartmentRequest
    {
        public string Name { get; set; }
		public string? ImgUrl { get; set; }
		public IEnumerable<SelectListItem> Projects { get; set; }
        public string[] SelectedProjects { get; set; }
    }
}

