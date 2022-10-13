using System.ComponentModel.DataAnnotations;

namespace Pharmacy.ViewModels
{
    public class SaveUserAccount
    {
        [Display(Name = "Name")]
        [MaxLength(256)]
        public string Name { get; set; }
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        [MaxLength(256)]
        //[Remote(action: "IsEmailInUse", controller: "Account")]
        public string Email { get; set; }
        [Display(Name = "Phone Number")]
        [MaxLength(256)]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Password, ErrorMessage = "Password must be strong")]
        [MinLength(8, ErrorMessage = "Password should at least have 8 characters")]
        public string Password { get; set; }
        [DataType(DataType.Password, ErrorMessage = "Password doesn't match")]
        [Compare("Password", ErrorMessage = "Password doesn't match")]
        [Display(Name = "Confirm your password")]
        public string ConfirmPassword { get; set; }

    }
}
