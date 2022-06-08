using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyRepoWebApp.Data;
using MyRepoWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;

namespace MyRepoWebApp.Pages.Photos
{
    [Authorize]
    public class PhotosModel : PageModel
    {
        [BindProperty]
        public BufferedSingleFileUploadDb FileUpload { get; set; } = new BufferedSingleFileUploadDb();

        public class BufferedSingleFileUploadDb
        {
            [Required]
            [Display(Name = "File")]
            public IFormFile? FormFile { get; set; }
        }


        private readonly MyRepoWebAppContext? _context;

        public PhotosModel(MyRepoWebAppContext context)
        { 
            if (context != null) _context = context;
                        
        }
       
        public int folderId { get; set; }

        public IList<Models.UploadModel> Upload { get;set; } = new List<Models.UploadModel>();

        [BindProperty(SupportsGet = true)] 
        public string? SearchString { get; set; }
        
        public async Task OnGetAsync(int Id)
        {            
            if (_context != null)
            {
                //get folder id from query string or tempdata from last request

                if (Request.QueryString.ToString().Contains("id"))
                {
                    folderId = Id;
                    Response.Cookies.Append("FolderId", Id.ToString());
                }                
                else
                {
                    folderId = Convert.ToInt32(Request.Cookies["FolderId"]);                                        
                }

                if (folderId > 0)
                {

                    folderId = Id;

                    var Uploads = from m in _context.Upload
                                  where m.owner == User.Identity.Name
                                  where m.FolderId == folderId
                                  select m;                 

                    Upload = await Uploads.ToListAsync();
                }
                else
                {
                    RedirectToPage($@"/Folders/Photos/Photos", new { id = folderId });
                }
            }
            else
            {
                RedirectToPage($@"/Folders/Photos/Photos", new { id = folderId });
            }
        }

        public async Task<IActionResult> OnPostUploadAsync()
        {
            //If the upload button is pressed without refreshing the pagem, get folder id from cookie

            if (folderId == 0) 
            { 
                folderId = Convert.ToInt32(Request.Cookies["FolderId"]); 
            }
             
            // if folder id is still 0 (cookie didnt have the value, return validation message
            if (folderId == 0)
            {
                ModelState.AddModelError("ValidationMessage", "The folder is invalid, please refresh this page to try again!");
                return Page();
            }

            if (FileUpload.FormFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {

                    await FileUpload.FormFile.CopyToAsync(memoryStream);

                    // Upload the file if less than 20 MB
                    if (memoryStream.Length < 20971520)
                    {
                        var file = new Models.UploadModel()
                        {
                            contents = memoryStream.ToArray()

                        };

                        file.Name = FileUpload.FormFile.FileName;
                        file.UpdateDate = DateTime.Now;
                        file.owner = User.Identity.Name == null ? string.Empty : User.Identity.Name;
                        file.FolderId = folderId;
                        file.Type = FileUpload.FormFile.ContentType;



                        _context.Upload.Add(file);

                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return RedirectToPage($@"/Folders/Photos/Photos", new { id = folderId });

                    }
                }            
                return RedirectToPage($@"/Folders/Photos/Photos", new { id = folderId });            
            }            
            else 
            { 
                ModelState.AddModelError("ValidationMessage", "This choose a file to upload..");
                return RedirectToPage($@"/Folders/Photos/Photos", new { id = folderId });
            }
        }
    }
}
