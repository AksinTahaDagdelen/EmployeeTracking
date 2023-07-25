using System;
namespace EmployeeTracking.Web.Models.ViewModels
{
    public class EditProjectRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
		public string? ImgUrl { get; set; }
		public string Note { get; set; }
	}
}

