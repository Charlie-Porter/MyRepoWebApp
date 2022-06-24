using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyRepoWebApp.Data;

namespace MyRepoWebApp.Pages.Uploads
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly MyRepoWebApp.Data.MyRepoWebAppContext _context;

        public DetailsModel(MyRepoWebApp.Data.MyRepoWebAppContext context)
        {
            _context = context;
        }

        public Models.UploadModel Upload { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Upload = await _context.Upload.FirstOrDefaultAsync(m => m.Id == id);

            if (Upload == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
