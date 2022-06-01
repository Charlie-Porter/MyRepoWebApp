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
using MyRepoWebApp.Services.Logger;

namespace MyRepoWebApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public CredentialModel Credential { get; set; }

        public string statusMessage { get; set; }

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
            
            // if user exists and password is correct
            if(CredentialModelUsernameExists(Credential.Email, Credential.Password))
            {
                // check if email is verfied
                if (IsUserVerified(Credential.Email))
                {
                    WriteToLog.writeToLogInformation($@"{Credential.Email} exists, password is valid and is verified, creating a claim for this user!");
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

                    WriteToLog.writeToLogInformation($@"{Credential.Email} claim created and siging in to app");

                    await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);
                    return RedirectToPage("/Folders/Folders");
                }
                // if not verified, report the status message to the page
                else
                {
                    WriteToLog.writeToLogInformation($@"********************* {Credential.Email} is not activated.");
                    ModelState.AddModelError("LoginValidationMessage", $@"Just another step, please activate your email by verifiying the email we sent you, thanks!");                
                    return Page(); 
                }                    
            }
            // if credentials are not correct, send message back to the page
            WriteToLog.writeToLogInformation($@"********************* {Credential.Email} does not exist or password is invalid.");
            ModelState.AddModelError("LoginValidationMessage", $@"Oh no, your email or password does not appear to be valid.");            
            return Page();
        }
        /// <summary>
        /// Checks if users email and password is valid. 
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        private bool CredentialModelUsernameExists(String Email, String Password)
        {
            
            doesUserExistinDB = _context.CredentialModel.Where(a => a.Email.Equals(Email)).FirstOrDefault();

            if (doesUserExistinDB != null) {

                SecurityService sc = new SecurityService();
                if (sc.VerifyHashedPassword(doesUserExistinDB.Password,Password) == PasswordVerificationResult.Success)
                {
                    WriteToLog.writeToLogInformation($@"{Email} exists in database.");
                    return true;
                } 
            }
            WriteToLog.writeToLogInformation($@"{Email} does not exist in database.");
            return false;            
        }

        /// <summary>
        /// This methods checks if the users email is verified.
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        private bool IsUserVerified(String Email)
        {

            doesUserExistinDB = _context.CredentialModel.Where(a => a.Email.Equals(Email)).FirstOrDefault();

            if (doesUserExistinDB.Verified)
            {
                WriteToLog.writeToLogInformation($@"{Email} is verifed.");
                return true;
                
            }
            else
            {
                WriteToLog.writeToLogInformation($@"{Email} is not verifed.");
                return false;
            }
                        
        }

    }
}
