using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Team.Web.Models
{
    public class UserViewModel
    {
        [Required]
        [StringLength(25)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25)]
        public string LastName { get; set; }

        [Required]
        [StringLength(120)]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(10)]
        public string Password { get; set; }
    }
}
