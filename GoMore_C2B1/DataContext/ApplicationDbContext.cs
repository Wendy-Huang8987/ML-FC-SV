using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using GoMore_C2B1.Models;

namespace GoMore_C2B1.DataContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base(nameOrConnectionString: "BIES_DB")
        {
        }
        public virtual DbSet<USERCLASS> USER { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<FactoryModel> FCM { get; set; }
        public virtual DbSet<LinkFileModel> LinkModel { get; set; }
        public virtual DbSet<LinkFileUrl> LinkFiles { get; set; }
        public virtual DbSet<ModelNameDB> ModelNames { get; set; }
        public virtual DbSet<SVModelControl> SVModelControl { get; set; }

    }
}