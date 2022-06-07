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

        [TempData]
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
                }                
                else
                {
                    folderId = Convert.ToInt32(TempData["folderId"].ToString());
                    TempData.Keep();
                }


                if (Request.QueryString.ToString().Contains("id") )
                {

                    folderId = Id;

                    var Uploads = from m in _context.Upload
                                  where m.owner == User.Identity.Name
                                  where m.FolderId == folderId
                                  select m;
                    if (!string.IsNullOrEmpty(SearchString))
                    {
                        Uploads = Uploads.Where(s => s.Name.Contains(SearchString));
                    }

                    Upload = await Uploads.ToListAsync();
                }
                else
                {
                    RedirectToPage("/Folders/Photos/Photos");
                }
            }
            else
            {
                RedirectToPage("/photos");
            }
        }

        public async Task<IActionResult> OnPostUploadAsync()
        {
            //get folder id from query string or tempdata from last request

           folderId = Convert.ToInt32(TempData["folderId"].ToString());
           TempData.Keep();

            if (FileUpload.FormFile != null)
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
                            file.UpdateDate = DateTime.Now;
                            file.owner = User.Identity.Name == null ? string.Empty : User.Identity.Name;
                            file.FolderId = folderId;
                            file.Type = FileUpload.FormFile.ContentType;



                            _context.Upload.Add(file);

                            await _context.SaveChangesAsync();


                        }
                        else
                        {
                            return RedirectToPage($@"/Folders/Photos/Photos?id={folderId}");  //https://localhost:61328/Folders/Photos/Photos?id=1

                        }
                    }

                    return RedirectToPage($@"/Folders/Photos/Photos?id={folderId}");
                }
                else
                {
                    ModelState.AddModelError("FolderDoesNotExist", "The folder is invalid, please navigate to the folder page and try again!");
                    return Page();
                }
            }
            else 
            { 
                ModelState.AddModelError("ChooseAFile", "This choose a file to upload..");
                return Page();

            }
        }

            

    }
}
