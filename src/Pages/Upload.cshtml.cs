using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRepoWebApp.Models;

namespace MyRepoWebApp.Pages
{
    [Authorize]
    public class UploadModel : PageModel
    {
        [BindProperty]
        public BufferedSingleFileUploadDb FileUpload { get; set; }
        private readonly Data.MyRepoWebAppContext _context;

        public void OnGet()
        {
        }
        [BindProperty]
        public Models.UploadModel uploadModel { get; set; }
        
        public UploadModel(Data.MyRepoWebAppContext context)
        {
            // this.ihostingEnvironment = ihostingEnvironment;
            _context = context;
        }

        public class BufferedSingleFileUploadDb
        {
            [Required]
            [Display(Name = "File")]
            public IFormFile FormFile { get; set; }
        }

        public async Task<IActionResult> OnPostUploadAsync()
        {
            using (var memoryStream = new MemoryStream())
            {
                await FileUpload.FormFile.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    var file = new Models.UploadModel()
                    {
                        contents = memoryStream.ToArray()
                      
                    };

                    file.Name = FileUpload.FormFile.FileName;
                    file.UpdateDate = System.DateTime.Now;

                    _context.Upload.Add(file);
                    
                    await _context.SaveChangesAsync();
                   
                }
                else
                {
                    ModelState.AddModelError("File", "The file is too large.");
                }
            }

            return Page();
        }
    }
}
