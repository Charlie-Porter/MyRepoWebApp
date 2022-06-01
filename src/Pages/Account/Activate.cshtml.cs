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
using MyRepoWebApp.Services.Logger;
using MyRepoWebApp.Data;
using Microsoft.EntityFrameworkCore;

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
            WriteToLog.writeToLogInformation($@"*************Azure Informattion Logger enabled************************************");

            string userId = Request.Query["id"];
            string code = Request.Query["code"];

            WriteToLog.writeToLogInformation($@"*************Activate page found {userId} and {code}");           
            if (userId != null && code != null)
            {
                try
                {

                    var connectionstring = _context.Database.GetDbConnection();

                    using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                    {
                        WriteToLog.writeToLogInformation($@"*************{_context.Database.GetDbConnection().ConnectionString}");

                        var activateCount = connection.Query<int>($@"SELECT count(*) FROM CredentialModel WHERE UserId = '{userId}' AND ActivationCode = '{code}'").FirstOrDefault();

                        WriteToLog.writeToLogInformation($@"*************Activate query --SELECT count(*) FROM CredentialModel WHERE UserId = '{userId}' AND ActivationCode = '{code}'--");
                        WriteToLog.writeToLogInformation($@"*************Activate count for {userId} was {activateCount}");
                        if (activateCount > 0)
                        {
                            connection.Query($@"UPDATE CredentialModel SET Verified = 'true' WHERE UserId = '{userId}'").FirstOrDefault();
                            WriteToLog.writeToLogInformation($@"*************Email activated successful for {userId}");
                            Confirmation = "Great news, your email was activated successful.";
                            return;
                        }
                        else
                        {
                            WriteToLog.writeToLogInformation($@"*************Email failed activation for {userId}");
                            Confirmation = "Bad news, your email was not activated.";
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {                    
                    Confirmation = $@"An exception occurred, your email was not activated.";
                    WriteToLog.writeToLogError($@"*************An exception occurred when activating email: {ex.Message}  {ex.InnerException}");
                }
                                
            }
            WriteToLog.writeToLogInformation($@"*************Gone into last return for some reason...  Bad news, your email was not activated");
            Confirmation =  "Bad news, your email was not activated.";
            return;
        }
    }
}
