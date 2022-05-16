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
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Updated Date")]
        [DataType(DataType.Date)]
        public DateTime UpdateDate { get; set; }
        public string Type { get; set; } = string.Empty;
        public byte[]? contents { get; set; }
    }
}
