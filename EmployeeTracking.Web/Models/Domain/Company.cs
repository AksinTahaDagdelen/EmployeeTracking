using System;

namespace EmployeeTracking.Web.Models.Domain
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? ImgUrl { get; set; }
        public DateTime FoundationDate { get; set; }

        public ICollection<Employee>? Employees { get; set; }
        public ICollection<Department>? Departments { get; set; }
        public ICollection<Project>? Projects { get; set; }
    }
}

