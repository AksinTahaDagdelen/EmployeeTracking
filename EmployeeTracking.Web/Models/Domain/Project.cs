using System;
namespace EmployeeTracking.Web.Models.Domain
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
		public string? ImgUrl { get; set; }
		public ICollection<Department>? Departments { get; set; }
        public ICollection<Employee>? Employees { get; set; }
        public ICollection<Company>? Companies { get; set; }
    }
}

