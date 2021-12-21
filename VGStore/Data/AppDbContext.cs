using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VGStore.Models;

namespace VGStore.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Consoles> Consoles { get; set;}
        public DbSet<Productos> Productos { get; set; }
        public DbSet<AppUsuario> AppUsuarios { get; set; }
    }
}
