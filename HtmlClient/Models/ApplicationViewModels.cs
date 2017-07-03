using System;
using System.ComponentModel.DataAnnotations;

namespace HtmlClient.Models
{
    public class HomeViewModel
    {
        
    }

    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }

    public class UserViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? LastLogin { get; set; }
    }

    public class XmlLink
    {
        public string Name { get; set; }
        public string LinkValue { get; set; }
    }

}