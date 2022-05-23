using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRepoWebApp.Models
{
    public class UploadModel
    {        
        [Required]
        [Key]
        public int ID { get; set; }
        [Required]        
        public string Name { get; set; } = string.Empty;      
        public string owner { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Updated Date")]
        [DataType(DataType.Date)]
        public DateTime UpdateDate { get; set; }
        public string Type { get; set; } = string.Empty;
        
        [Required]
        public int FolderId { get; set; } 

        [Required]
        public byte[]? contents { get; set; }
    }
}
