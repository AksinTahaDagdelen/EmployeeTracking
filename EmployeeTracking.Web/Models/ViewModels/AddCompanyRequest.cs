using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeTracking.Web.Models.ViewModels
{
    public class AddCompanyRequest
    {
        public string Name { get; set; }
		public string? ImgUrl { get; set; }
		public DateTime FoundationDate { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }
        public string[] SelectedDepartments { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }
        public string[] SelectedProjects { get; set; }
        public IEnumerable<SelectListItem> Employees { get; set; }
        public string[] SelectedEmployees { get; set; }
    }
}

