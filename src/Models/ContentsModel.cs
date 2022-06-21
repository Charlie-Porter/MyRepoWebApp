using System.ComponentModel.DataAnnotations;

namespace MyRepoWebApp.Models
{
    public class ContentsModel
    {
        [Required]
        [Key]
        public long Id { get; set; }

        [Required]        
        public byte[] Contents { get; set; } = new byte[0];        

    }
}
