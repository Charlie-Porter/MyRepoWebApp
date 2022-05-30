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
        public CredentialModel CredentialModel { get; set; }

        public void OnGet()
        {
            
                //Guid activationCode = new Guid(RouteData.Values["id"].ToString());

                string userId = Request.Query["id"];
                string code = Request.Query["code"];

            if (userId != null && code != null)
            {

                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection("Server=(localdb)\\mssqllocaldb;Database=MyRepoWebAppContext-87d5f408-9f32-465d-838a-210968c3083b;Trusted_Connection=True;MultipleActiveResultSets=true"))
                {
                    var result = connection.Query<CredentialModel>($@"SELECT UserId, ActivationCode FROM CredentialModel WHERE UserId = '{userId}'");
                }

                // ViewBag.Message = "Activation successful.";
               
            }

        }
    }
}
