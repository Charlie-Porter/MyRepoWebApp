﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyRepoWebApp.Models
{
   
        public class CredentialModel
        {
            [Required]
            [Key]
            public long UserId { get; set; }
            
            [Required]
            [Display(Name = "Email")]
            public string Email { get; set; }
            
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            
            [Display(Name = "Remeber Me")]
            public bool RememberMe { get; set; }
    }
    
}
