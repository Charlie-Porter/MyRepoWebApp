using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
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
            if(CredentialModelUsernameExists(Credential.Username, Credential.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Credential.Username),
                    new Claim(ClaimTypes.Email, "admin@mywebsite.com")
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

        private bool CredentialModelUsernameExists(String Username, String Password)
        {
            return _context.CredentialModel.Any(e => e.Username == Username) && _context.CredentialModel.Any(c => c.Password == Password);
        }

    }
}
