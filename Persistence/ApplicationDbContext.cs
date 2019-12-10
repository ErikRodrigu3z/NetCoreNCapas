using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public object TEntity;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //DbSet 
        public DbSet<Cutomers> Cutomers { get; set; }
         




    }
}
