using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyRepoWebApp.Data;
using MyRepoWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace MyRepoWebApp.Pages.Photos
{
    [Authorize]
    public class PhotosModel : PageModel
    {
        private readonly MyRepoWebApp.Data.MyRepoWebAppContext _context;

        public PhotosModel(MyRepoWebApp.Data.MyRepoWebAppContext context)
        {
            _context = context;
        }


        public IList<Models.UploadModel> Upload { get;set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public SelectList Genres { get; set; }
        [BindProperty(SupportsGet = true)]
        public string UploadGenre { get; set; }

        public async Task OnGetAsync()
        {
            var Uploads = from m in _context.Upload where m.owner == User.Identity.Name
                         select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                Uploads = Uploads.Where(s => s.Name.Contains(SearchString));
            }

            Upload = await Uploads.ToListAsync();
        }
    }
}
