using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyRepoWebApp.Data;
using MyRepoWebApp.Models;

namespace MyRepoWebApp.Pages.Account
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly MyRepoWebApp.Data.MyRepoWebAppContext _context;

        public EditModel(MyRepoWebApp.Data.MyRepoWebAppContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(CredentialModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CredentialModelExists(CredentialModel.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Users");
        }

        private bool CredentialModelExists(long id)
        {
            return _context.CredentialModel.Any(e => e.UserId == id);
        }
    }
}
