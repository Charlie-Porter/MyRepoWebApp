using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyRepoWebApp.Models;

namespace MyRepoWebApp.Data
{
    public class MyRepoWebAppContext : DbContext
    {

        //constructor
        public MyRepoWebAppContext(DbContextOptions<MyRepoWebAppContext> options)
            : base(options)
        {
        }

        public DbSet<UploadModel> Upload { get; set; }

        public DbSet<ContentsModel> Content { get; set; }

        public DbSet<CredentialModel> CredentialModel { get; set; }

        public DbSet<FolderModel> FolderModel { get; set; } 
        public DbSet<ApplicationUser> ApplicationUser { get; set; } 
    }
}
