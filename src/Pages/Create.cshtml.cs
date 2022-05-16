﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyRepoWebApp.Data;
using MyRepoWebApp.Models;

namespace MyRepoWebApp.Pages
{
    public class CreateModel : PageModel
    {
       // private IHostingEnvironment ihostingEnvironment;
        private readonly MyRepoWebApp.Data.MyRepoWebAppContext _context;

        public string FileName { get; set; }

        public CreateModel(MyRepoWebApp.Data.MyRepoWebAppContext context)
        {
           // this.ihostingEnvironment = ihostingEnvironment;
            _context = context;
        }

        /* public void OnPost(IFormFile photo)
         {
             var path = Path.Combine(ihostingEnvironment.WebRootPath, "images", photo.FileName);
             var stream = new FileStream(path, FileMode.Create);
             photo.CopyToAsync(stream);
             FileName = photo.FileName;
         }
 */



        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public BufferedSingleFileUploadDb FileUpload { get; set; }

        public class BufferedSingleFileUploadDb
        {
            [Required]
            [Display(Name = "File")]
            public IFormFile FormFile { get; set; }
        }


        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
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

                    _context.Upload.Add(file);

                    await _context.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError("File", "The file is too large.");
                }
            }          

            return RedirectToPage("./Index");
        }

        // extension method
      

    }
}
