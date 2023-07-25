using System;
namespace EmployeeTracking.Web.Models.Domain
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
		public string? ImgUrl { get; set; }
		public string Note { get; set; }
        public double? Salary { get; set; }
        public ICollection<Company>? Companies { get; set; }
        public ICollection<Department>? Departments { get; set; }
        public ICollection<Project>? Projects { get; set; }
    }
}