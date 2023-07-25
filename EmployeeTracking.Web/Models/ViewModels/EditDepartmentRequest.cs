using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeTracking.Web.Models.ViewModels
{
    public class EditDepartmentRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
		public string? ImgUrl { get; set; }
	
		public IEnumerable<SelectListItem> Projects { get; set; }
        public string[] SelectedProjects { get; set; }
    }
}

