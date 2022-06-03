using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRepoWebApp.Models;
using MyRepoWebApp.Services;
using MyRepoWebApp.Services.Logger;

namespace MyRepoWebApp.Pages.Account
{

    public class RegisterUser : PageModel
    {
        /// <summary>
        /// The manager for handling user creation, deletion, searching, roles etc...
        /// </summary>
     //   protected UserManager<CredentialModel> mUserManager;


        RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();
        private readonly MyRepoWebApp.Data.MyRepoWebAppContext _context;

        public RegisterUser(Data.MyRepoWebAppContext context) => _context = context;

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
                WriteToLog.writeToLogInformation($@"Model state is not valid");
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
            //hash password before saving it to the database
            CredentialModel.Password = Convert.ToBase64String(SecurityService.HashPasswordV2(CredentialModel.Password, _rng));
            
            // Generates an email verifcation code                
            CredentialModel.ActivationCode = Guid.NewGuid();
            WriteToLog.writeToLogInformation($@"{CredentialModel.Email} activation code created and sent");

            //adds updated model to the context before saving
            _context.CredentialModel.Add(CredentialModel);
            await _context.SaveChangesAsync();

            try
            {           

                var confirmationUrl = $@"https://{Request.Host.Value}/Account/Activate?id={CredentialModel.UserId}&code={CredentialModel.ActivationCode}";
                var sendEmailResponseModel = await Services.SendGrid.RepoEmailSender.SendUserVerificationEmailAsync(displayName: string.Empty, CredentialModel.Email, confirmationUrl);                            

            }
            catch (Exception ex)
            {
                WriteToLog.writeToLogError($@"exception on register user: {ex.Message} {ex.InnerException}");
            }
            return RedirectToPage("./Users");
        }



        private bool CredentialModelUsernameExists(String Email)
        {
            return _context.CredentialModel.Any(e => e.Email == Email);
        }

       
    }
}
