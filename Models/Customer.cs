using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace minimal_api.Models
{
    public record Customer(int Id, string FirstName, string LastName)
    {
        public Customer() : this(Id: default, FirstName: default, LastName : default)
        {

        }
    }

    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
