using GilsbarDemo1.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace GilsbarDemo1.Models
{
    public class GilsbarDemo1Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public GilsbarDemo1Context() : base("name=GilsbarDemo1Context")
        {
            Database.SetInitializer<GilsbarDemo1Context>(new MigrateDatabaseToLatestVersion<GilsbarDemo1Context, Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }


        public System.Data.Entity.DbSet<GilsbarDemo1.Models.Person> Persons { get; set; }

        public System.Data.Entity.DbSet<GilsbarDemo1.Models.Manager> Managers { get; set; }

        public System.Data.Entity.DbSet<GilsbarDemo1.Models.JobTask> JobTasks { get; set; }

        public System.Data.Entity.DbSet<GilsbarDemo1.Models.Department> Departments { get; set; }

        public System.Data.Entity.DbSet<GilsbarDemo1.Models.Building> Buildings { get; set; }

        public System.Data.Entity.DbSet<GilsbarDemo1.Models.JobTaskPersons> JobTaskPersons { get; set; }
    } // public class GilsbarDemo1Context : DbContext
}
