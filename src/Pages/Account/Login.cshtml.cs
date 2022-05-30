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
using MyRepoWebApp.Services;
using Microsoft.AspNetCore.Identity;

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
           // if (!ModelState.IsValid) return Page();

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
                return RedirectToPage("/Folders/Folders");
            }
            return Page();
        }

        private bool CredentialModelUsernameExists(String Email, String Password)
        {
            
            doesUserExistinDB = _context.CredentialModel.Where(a => a.Email.Equals(Email)).FirstOrDefault();

            if (doesUserExistinDB != null) {

                SecurityService sc = new SecurityService();
                if (sc.VerifyHashedPassword(doesUserExistinDB.Password,Password) == PasswordVerificationResult.Success)
                {
                    return true;
                } 
            }
            return false;            
        }       

    }
}
