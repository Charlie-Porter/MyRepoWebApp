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
using Microsoft.Extensions.Primitives;
using MyRepoWebApp.Models;

namespace MyRepoWebApp.Pages.Photos
{
    [Authorize]
    public class UploadModel : PageModel 
    {
        [BindProperty]
        public BufferedSingleFileUploadDb FileUpload { get; set; }
        private readonly Data.MyRepoWebAppContext _context;
        
        [BindProperty, TempData]
        public int folderId { get; set; }

        public void OnGet(int Id)
        {
            if (Request.QueryString.ToString().Contains("id"))
            {
                folderId = Id;
            }
            else
            {
                ModelState.AddModelError("FolderDoesNotExist", "The folder is invalid, please navigate to the folder page and try again!");
            }                        
        }

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
            if (folderId > 0)
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
                        file.owner = User.Identity.Name;
                        file.FolderId = folderId;
                        file.Type = FileUpload.FormFile.ContentType;



                        _context.Upload.Add(file);

                        await _context.SaveChangesAsync();


                    }
                    else
                    {
                        return RedirectToPage("/Folders/Folders");

                    }
                }

                return RedirectToPage("/Folders/Folders");
            }
            else
            {
                ModelState.AddModelError("FolderDoesNotExist", "The folder is invalid, please navigate to the folder page and try again!");
                return Page();
            }
        }
    }
}
