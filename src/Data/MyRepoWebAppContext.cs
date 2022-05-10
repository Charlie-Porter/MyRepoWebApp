﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyRepoWebApp.Models;

namespace MyRepoWebApp.Data
{
    public class MyRepoWebAppContext : DbContext
    {
        public MyRepoWebAppContext (DbContextOptions<MyRepoWebAppContext> options)
            : base(options)
        {
        }

        public DbSet<MyRepoWebApp.Models.Movie> Movie { get; set; }
    }
}