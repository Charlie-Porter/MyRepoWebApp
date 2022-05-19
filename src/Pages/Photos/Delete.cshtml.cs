using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyRepoWebApp.Data;
using MyRepoWebApp.Models;

namespace MyRepoWebApp.Pages.Photos
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly MyRepoWebApp.Data.MyRepoWebAppContext _context;

        public DeleteModel(MyRepoWebApp.Data.MyRepoWebAppContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.UploadModel Upload { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Upload = await _context.Upload.FirstOrDefaultAsync(m => m.ID == id);

            if (Upload == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Upload = await _context.Upload.FindAsync(id);

            if (Upload != null)
            {
                _context.Upload.Remove(Upload);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
