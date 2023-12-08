using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Project.Models
{
    public class RegisterUserModel
    {
        [Required(ErrorMessage = "Please enter your username")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]{4,19}$", ErrorMessage = "Please provide a valid username between 4 and 19 characters.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [Display(Name = "Email Address")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please Provide a Valid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password")]

        [Compare("ConfirmPassword", ErrorMessage = "Password does not match")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
