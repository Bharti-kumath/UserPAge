using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BAL.Models
{
    public class UserViewModel
    {
        public int ID { get; set; }
        [Required]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Only letters are allowed in the First Name")]
        [StringLength(16, ErrorMessage = "First name must be at most 16 characters long")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Only letters are allowed in the Last Name")]
        [StringLength(16, ErrorMessage = "Last name must be at most 16 characters long")]
        public string LastName { get; set;}

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z]+\.[a-zA-Z]{1,}$", ErrorMessage = "Enter Valid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter phone number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone Number must be at most 10 characters long.")]
        public string PhoneNUmber { get; set; }

        [Required]
        public string DAteOfBirth { get; set; }

        [Required(ErrorMessage = "Please select a country from the list.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Please select a city from the list.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter Pincode")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Pincode must be 6 characters long.")]
        public string PinCode { get; set; }
        [Required]
        public string Address { get; set; }
        public int TotalCount { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must be Minimum 8 characters, at least one uppercase letter, one lowercase letter, one number and one special character.")]
        public string Password { get; set; }


        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        [Required(AllowEmptyStrings = true)]
        public string ConfirmPassword { get; set; }


    }
}