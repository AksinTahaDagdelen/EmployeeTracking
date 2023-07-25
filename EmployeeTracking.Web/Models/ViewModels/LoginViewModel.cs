using System;
namespace EmployeeTracking.Web.Models.ViewModels
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}

