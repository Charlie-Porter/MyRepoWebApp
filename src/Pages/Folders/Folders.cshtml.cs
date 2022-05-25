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

namespace MyRepoWebApp.Pages.Folders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly MyRepoWebApp.Data.MyRepoWebAppContext _context;
        public IList<Models.FolderModel> Folder { get; set; }

        public IndexModel(MyRepoWebApp.Data.MyRepoWebAppContext context)
        {
            _context = context;
        }

        [ModelBinder]
        public IList<FolderModel> FolderModel { get;set; }

        public async Task OnGetAsync()
        {

            var Folders = from m in _context.FolderModel
                              where m.owner == User.Identity.Name
                          select m;

            FolderModel = await Folders.ToListAsync();


            //FolderModel = await _context.FolderModel.ToListAsync();
        }
    }
}
