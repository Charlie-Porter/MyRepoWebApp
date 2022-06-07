using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyRepoWebApp.Data;
using MyRepoWebApp.Models;
using MyRepoWebApp.Services;

namespace MyRepoWebApp.Pages.Account
{
    [Authorize(Policy = "AdminOnly")]
    public class CreateModel : PageModel
    {
        RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();
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
        public CredentialModel CredentialModel { get; set; } = new CredentialModel();

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (CredentialModelUsernameExists(CredentialModel.Email))
            {
                ModelState.AddModelError("DuplicateUser", "This email already exists");                
                return Page();
            }

            if (CredentialModel.Password == null)
            {
                throw new ArgumentNullException(nameof(CredentialModel.Password));
            }

            CredentialModel.Password = Convert.ToBase64String(SecurityService.HashPasswordV2(CredentialModel.Password, _rng));

            _context.CredentialModel.Add(CredentialModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Users");
        }

        private bool CredentialModelUsernameExists(String Email)
        {
            return _context.CredentialModel.Any(e => e.Email == Email);
        }
    }
}
