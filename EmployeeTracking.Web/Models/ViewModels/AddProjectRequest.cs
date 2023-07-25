using System;
namespace EmployeeTracking.Web.Models.ViewModels
{
    public class AddProjectRequest
    {
        public string Name { get; set; }
		public string? ImgUrl { get; set; }
		public string Note { get; set; }
	}
}

