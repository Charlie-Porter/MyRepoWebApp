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

namespace MyRepoWebApp.Pages.Account
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
        public CredentialModel CredentialModel { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CredentialModel = await _context.CredentialModel.FirstOrDefaultAsync(m => m.UserId == id);

            if (CredentialModel == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CredentialModel = await _context.CredentialModel.FindAsync(id);

            if (CredentialModel != null)
            {
               if(CredentialModel.Email == User.Identity.Name)
                {
                    ModelState.AddModelError("UnableToDeleteLoggedInUser", "Unable to delete logged in user");
                    return Page();
                }
                
                _context.CredentialModel.Remove(CredentialModel);
                await _context.SaveChangesAsync();

            }

            return RedirectToPage("./Users");
        }
    }
}
