using System;
namespace EmployeeTracking.Web.Models.Domain
{
    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
		public string? ImgUrl { get; set; }
		public ICollection<Project>? Projects { get; set; }
        public ICollection<Employee>? Employees { get; set; }
        public ICollection<Company>? Companies { get; set; }
    }
}

