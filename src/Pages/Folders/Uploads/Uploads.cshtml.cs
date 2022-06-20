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
using System.Drawing;

namespace MyRepoWebApp.Pages.Uploads
{
    [Authorize]
    public class UploadsModel : PageModel
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

        public UploadsModel(MyRepoWebAppContext context)
        {
            if (context != null) _context = context;

        }

        public int folderId { get; set; }

        public IList<Models.UploadModel> Upload { get; set; } = new List<Models.UploadModel>();

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
                    //TODO: set expiry of cookie
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
                                  where m.Owner == User.Identity.Name
                                  where m.FolderId == folderId
                                  select m;

                    Upload = await Uploads.ToListAsync();
                }
                else
                {
                    RedirectToPage($@"/Folders/Uploads/Uploads", new { id = folderId });
                }
            }
            else
            {
                RedirectToPage($@"/Folders/Uploads/Uploads", new { id = folderId });
            }
        }

        public async Task<IActionResult> OnPostUploadAsync()
        {
            //If the upload button is pressed without refreshing the page, get folder id from cookie

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
                            Contents = memoryStream.ToArray(),
                            Thumbnail = GetThumbnail(FileUpload.FormFile.ContentType, 80, 80, memoryStream),
                            Name = FileUpload.FormFile.FileName,
                            UpdateDate = DateTime.Now,
                            Owner = User.Identity.Name == null ? string.Empty : User.Identity.Name,
                            FolderId = folderId,
                            Type = FileUpload.FormFile.ContentType
                        };

                        _context.Upload.Add(file);

                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return RedirectToPage($@"/Folders/Uploads/Uploads", new { id = folderId });

                    }
                }
                return RedirectToPage($@"/Folders/Uploads/Uploads", new { id = folderId });
            }
            else
            {
                ModelState.AddModelError("ValidationMessage", "This choose a file to upload..");
                return RedirectToPage($@"/Folders/Uploads/Uploads", new { id = folderId });
            }
        }
        /// <summary>
        ///Create a Image Thumbnail from the memory stream and convert it back into a byte array as EF does not support Image reference types
        /// </summary>
        /// <param name="type"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public byte[] GetThumbnail(string type, int height, int width, Stream memoryStream)
        {
            //EF understands only basic types and each mapped class must be either recognized as entity or complex type. So an Impage must be defined as a byte array 
            //By marking it as byte[] EF will understand that it must be stored as varbinary in SQL server. 

            if (type.Contains("image"))
            {
                using (var thumbPhoto = Image.FromStream(memoryStream,
                    true).GetThumbnailImage(height, width, null,
                    new System.IntPtr()))
                {
                    return (byte[])(new ImageConverter()).ConvertTo(thumbPhoto, typeof(byte[]));
                }
            }
            else
            {
                return new byte[0];
            }

        }
        /// <summary>
        /// Method to download a file from the database
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        public async Task<FileResult> OnGetDownloadFile(long Id)
        {
            //gets the file from the context (db) using its Id 
            var file = await _context.Set<UploadModel>().FindAsync(Id);

            //Sends the File to Download.
            return File(file.Contents, file.Type, file.Name);
        }
    }
}
