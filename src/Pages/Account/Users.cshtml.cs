using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyRepoWebApp.Data;
using MyRepoWebApp.Models;

namespace MyRepoWebApp.Pages.Account
{
    [Authorize]
    public class UsersModel : PageModel
    {
        private readonly MyRepoWebApp.Data.MyRepoWebAppContext _context;

        public UsersModel(MyRepoWebApp.Data.MyRepoWebAppContext context)
        {
            _context = context;
        }

        public IList<CredentialModel> CredentialModel { get;set; }

        public async Task OnGetAsync()
        {
            CredentialModel = await _context.CredentialModel.ToListAsync();
        }
    }
}
