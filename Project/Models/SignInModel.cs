using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class SignInModel
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]{4,19}$", ErrorMessage = "Please provide a valid username between 4 and 19 characters.")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]{9}$", ErrorMessage = "Please provide a valid password more than 8 characters.")]
        public string Password { get; set; }

       
      


    }
}
