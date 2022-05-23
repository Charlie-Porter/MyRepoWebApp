using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyRepoWebApp.Data;
using MyRepoWebApp.Models;
using MyRepoWebApp.Services;

namespace MyRepoWebApp.Pages.Account
{
    [Authorize(Policy = "AdminOnly")]
    public class EditModel : PageModel
    {
        private readonly MyRepoWebApp.Data.MyRepoWebAppContext _context;
        RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();

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
                CredentialModel.Password = Convert.ToBase64String(SecurityService.HashPasswordV2(CredentialModel.Password, _rng));
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
