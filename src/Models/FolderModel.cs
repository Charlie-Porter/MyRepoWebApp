using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyRepoWebApp.Models
{
    public class FolderModel
    {
        [Required]        
        public int ID { get; set; }

        [Required]
        [Key]
        public string Name { get; set; } = string.Empty;

        [Required] 
        public string owner { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Updated Date")]
        [DataType(DataType.Date)]
        public DateTime UpdateDate { get; set; }
    }
}
