using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFromGitToMongo.Models
{
    public class UserRegister
    {
        [Required]
        public string username { get; set; } = string.Empty;
        
        [Required, EmailAddress]
        public string email { get; set; } = string.Empty;
        
        [Required, StringLength(100, MinimumLength =6)]
        public string password { get; set; } = string.Empty;

        [Compare("password", ErrorMessage ="The passwords do not match.")]
        public string confirmpassword { get; set; } = string.Empty;
    }
}
