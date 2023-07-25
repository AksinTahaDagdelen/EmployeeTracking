using System;
using EmployeeTracking.Web.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeTracking.Web.Models.ViewModels
{
    public class AddEmployeeRequest
    {

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
		public string Note { get; set; }
		public string PhoneNumber { get; set; }
		public string? ImgUrl { get; set; }
		public double? Salary { get; set; }


        public IEnumerable<SelectListItem>? Projects { get; set; }
        public string[]? SelectedProjects { get; set; }

        public IEnumerable<SelectListItem>? Departments { get; set; }
        public string[]? SelectedDepartments { get; set; }


        public IEnumerable<SelectListItem>? Companies { get; set; }
        public string[]? SelectedCompanies { get; set; }
    }
}

