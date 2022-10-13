using System.ComponentModel.DataAnnotations;

namespace Pharmacy.ViewModels
{
    public class LoginVM
    { 
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        [MaxLength(256)]
        public string Email { get; set; }
        [DataType(DataType.Password, ErrorMessage = "Password must be strong")]
        [MinLength(8, ErrorMessage = "Password should at least have 8 characters")]
        public string Password { get; set; }
    }
}
