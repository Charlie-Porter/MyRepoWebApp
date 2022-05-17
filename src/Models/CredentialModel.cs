using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyRepoWebApp.Models
{
   
        public class CredentialModel
        {
            [Required]
            [Display(Name = "User Name")]
            public string Username { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    
}
