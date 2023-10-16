using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace AirAidans.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "invalid Email Type")]
        [Required(ErrorMessage = "Email Name is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please include subject")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "please fill out your message")]
        public string Message { get; set; }

    }
}