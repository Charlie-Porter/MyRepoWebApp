using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRepoWebApp.Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using Dna;
using System.Configuration;

namespace MyRepoWebApp.Pages.Account
{
    public class ActivateModel : PageModel
    {

        private readonly MyRepoWebApp.Data.MyRepoWebAppContext _context;

        public ActivateModel(MyRepoWebApp.Data.MyRepoWebAppContext context)
        {
            _context = context;
        }    
        [BindProperty]
        public string Confirmation { get; set; }
        
        public void OnGet()
        {            
            string userId = Request.Query["id"];
            string code = Request.Query["code"];

            if (userId != null && code != null)
            {
                try
                {
                    using (IDbConnection connection = new System.Data.SqlClient.SqlConnection("Server=(localdb)\\mssqllocaldb;Database=MyRepoWebAppContext-87d5f408-9f32-465d-838a-210968c3083b;Trusted_Connection=True;MultipleActiveResultSets=true"))
                    {

                        var activateCount = connection.Query<int>($@"SELECT count(*) FROM CredentialModel WHERE UserId = '{userId}' AND ActivationCode = '{code}'").FirstOrDefault();
                        if (activateCount > 0)
                        {
                            connection.Query($@"UPDATE CredentialModel SET Verified = 'true' WHERE UserId = '{userId}'").FirstOrDefault();
                            Confirmation = "Great news, your email was activated successful.";
                            return;
                        }
                        else
                        {
                            Confirmation = "Bad news, your email was not activated.";
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {                    
                    Confirmation = $@"An exception occurred, your email was not activated.";
                    Dna.FrameworkDI.Logger.LogErrorSource($@"An exception occurred when activating email: {ex.Message}  {ex.InnerException}");
                }
                                
            }
            Confirmation =  "Bad news, your email was not activated.";
            return;
        }
    }
}
