using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyRepoWebApp.Data;
using MyRepoWebApp.Models;

namespace MyRepoWebApp.Pages.Account
{
    [Authorize]
    public class CreateModel : PageModel
    {

        private readonly MyRepoWebApp.Data.MyRepoWebAppContext _context;

        public CreateModel(MyRepoWebApp.Data.MyRepoWebAppContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CredentialModel CredentialModel { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (CredentialModelUsernameExists(CredentialModel.Username))
            {
                ModelState.AddModelError("DuplicateUser", "This username already exists");                
                return Page();
            }
        
            _context.CredentialModel.Add(CredentialModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Users");
        }

        private bool CredentialModelUsernameExists(String Username)
        {
            return _context.CredentialModel.Any(e => e.Username == Username);
        }
    }
}
