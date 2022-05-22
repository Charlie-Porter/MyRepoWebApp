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

namespace MyRepoWebApp.Pages.Account
{
    
    public class CreateModel : PageModel
    {
        private readonly RandomNumberGenerator _rng;
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

            if (CredentialModelUsernameExists(CredentialModel.Email))
            {
                ModelState.AddModelError("DuplicateUser", "This email already exists");                
                return Page();
            }

            if (CredentialModel.Password == null)
            {
                throw new ArgumentNullException(nameof(CredentialModel.Password));
            }
            RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();
            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            CredentialModel.Password = Convert.ToBase64String(HashPasswordV2(CredentialModel.Password, _rng));

            _context.CredentialModel.Add(CredentialModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Users");
        }

        private bool CredentialModelUsernameExists(String Email)
        {
            return _context.CredentialModel.Any(e => e.Email == Email);
        }

        private static byte[] HashPasswordV2(string password, RandomNumberGenerator rng)
        {
            const KeyDerivationPrf Pbkdf2Prf = KeyDerivationPrf.HMACSHA1; // default for Rfc2898DeriveBytes
            const int Pbkdf2IterCount = 1000; // default for Rfc2898DeriveBytes
            const int Pbkdf2SubkeyLength = 256 / 8; // 256 bits
            const int SaltSize = 128 / 8; // 128 bits

            // Produce a version 2 (see comment above) text hash.
            byte[] salt = new byte[SaltSize];
            rng.GetBytes(salt);
            byte[] subkey = KeyDerivation.Pbkdf2(password, salt, Pbkdf2Prf, Pbkdf2IterCount, Pbkdf2SubkeyLength);

            var outputBytes = new byte[1 + SaltSize + Pbkdf2SubkeyLength];
            outputBytes[0] = 0x00; // format marker
            Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
            Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SaltSize, Pbkdf2SubkeyLength);
            return outputBytes;
        }

    }
}
