using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRepoWebApp.Models;

namespace MyRepoWebApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public CredentialModel Credential { get; set; }

        private readonly MyRepoWebApp.Data.MyRepoWebAppContext _context;
        private CredentialModel doesUserExistinDB;
        public LoginModel(MyRepoWebApp.Data.MyRepoWebAppContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            //if(Credential.Username == "admin" && Credential.Password == "password")
            if(CredentialModelUsernameExists(Credential.Email, Credential.Password))
            {
                var claims = new List<Claim>
                {         
                    new Claim(ClaimTypes.Name, Credential.Email),
                    new Claim(ClaimTypes.Email, Credential.Email),
                    new Claim("Admin", doesUserExistinDB.Admin.ToString())
                };
                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Credential.RememberMe
                };
                

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);
                return RedirectToPage("/Index");
            }
            return Page();
        }

        private bool CredentialModelUsernameExists(String Email, String Password)
        {
            
            doesUserExistinDB = _context.CredentialModel.Where(a => a.Email.Equals(Email)).FirstOrDefault();

            if (doesUserExistinDB != null) {

                if (VerifyHashedPassword(doesUserExistinDB.Password,Password) == PasswordVerificationResult.Success)
                {
                    return true;
                } 
            }
            return false;            
        }

        private readonly PasswordHasherCompatibilityMode _compatibilityMode;
        private readonly int _iterCount;
        private readonly RandomNumberGenerator _rng;
        public virtual PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (hashedPassword == null)
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }
            if (providedPassword == null)
            {
                throw new ArgumentNullException(nameof(providedPassword));
            }

            byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);

            // read the format marker from the hashed password
            if (decodedHashedPassword.Length == 0)
            {
                return PasswordVerificationResult.Failed;
            }
            if (VerifyHashedPasswordV2(decodedHashedPassword, providedPassword))
            {
                // This is an old password hash format - the caller needs to rehash if we're not running in an older compat mode.
                return (_compatibilityMode == PasswordHasherCompatibilityMode.IdentityV3)
                    ? PasswordVerificationResult.SuccessRehashNeeded
                    : PasswordVerificationResult.Success;
            }
            else
            {
                return PasswordVerificationResult.Failed;
            }
        }

        private static bool VerifyHashedPasswordV2(byte[] hashedPassword, string password)
        {
            const KeyDerivationPrf Pbkdf2Prf = KeyDerivationPrf.HMACSHA1; // default for Rfc2898DeriveBytes
            const int Pbkdf2IterCount = 1000; // default for Rfc2898DeriveBytes
            const int Pbkdf2SubkeyLength = 256 / 8; // 256 bits
            const int SaltSize = 128 / 8; // 128 bits

            // We know ahead of time the exact length of a valid hashed password payload.
            if (hashedPassword.Length != 1 + SaltSize + Pbkdf2SubkeyLength)
            {
                return false; // bad size
            }

            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(hashedPassword, 1, salt, 0, salt.Length);

            byte[] expectedSubkey = new byte[Pbkdf2SubkeyLength];
            Buffer.BlockCopy(hashedPassword, 1 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

            // Hash the incoming password and verify it
            byte[] actualSubkey = KeyDerivation.Pbkdf2(password, salt, Pbkdf2Prf, Pbkdf2IterCount, Pbkdf2SubkeyLength);
            #if NETSTANDARD2_0 || NETFRAMEWORK
                    return ByteArraysEqual(actualSubkey, expectedSubkey);
            #elif NETCOREAPP
                        return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
            #else
            #error Update target frameworks
            #endif
        }

    }
}
