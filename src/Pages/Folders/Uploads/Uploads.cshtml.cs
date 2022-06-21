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
using System.Text;

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

        public IList<UploadModel> Upload { get; set; } = new List<UploadModel>();
        //public IList<ContentsModel> Content { get; set; } = new List<ContentsModel>();

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
                            Name = FileUpload.FormFile.FileName,
                            UpdateDate = DateTime.Now,
                            Owner = User.Identity.Name == null ? string.Empty : User.Identity.Name,
                            FolderId = folderId,
                            Type = FileUpload.FormFile.ContentType,
                            Thumbnail = GetThumbnail(FileUpload.FormFile.ContentType, 80, 80, memoryStream),

                        };

                        var content = new Models.ContentsModel()
                        {
                            Contents = memoryStream.ToArray(),
                            Id = file.Id
                        };

                        _context.Upload.Add(file);
                        _context.Content.Add(content);

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
                //if file type is any type of image, return preview of the image in as a thumbnail
                using (var thumbPhoto = Image.FromStream(memoryStream,
                    true).GetThumbnailImage(height, width, null,
                    new System.IntPtr()))
                {
                    return (byte[])(new ImageConverter()).ConvertTo(thumbPhoto, typeof(byte[]));
                }
            }
            if (type.Contains("pdf"))
            {
                //if file type is pdf, return generic pdf thumbnail
                return System.Convert.FromBase64String("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");
                
            }
            
            if (type.Contains("text"))
            {
                //if file type is text, return generic text thumbnail
                return System.Convert.FromBase64String("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");
                
            }
            if (type.Contains("video"))
            {
                //if file type is video, return generic video thumbnail
                return System.Convert.FromBase64String("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");
                
            }
            if (type.Contains("excel"))
            {
                //if file type is excel, return generic excel thumbnail
                return System.Convert.FromBase64String("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");
                
            }           
            if (type.Contains("zip") || type.Contains("compressed"))
            {
                //if file type is compressed, return generic compressed thumbnail
                return System.Convert.FromBase64String("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");                
            }
            if (type.Contains("application"))
            {
                //if file type is application, return generic application thumbnail
                return System.Convert.FromBase64String("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");

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
            var content = await _context.Set<ContentsModel>().FindAsync(Id);

            //Sends the File to Download.
            return File(content.Contents, file.Type, file.Name);
        }
    }
}
