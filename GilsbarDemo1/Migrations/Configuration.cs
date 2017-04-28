namespace GilsbarDemo1.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GilsbarDemo1.Models.GilsbarDemo1Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "GilsbarDemo1.Models.GilsbarDemo1Context";
        }

        protected override void Seed(GilsbarDemo1.Models.GilsbarDemo1Context context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //    );
            //

            // seed data for GilsbarDemo1

            if (context.Managers.Count() == 0)
            {
                context.Managers.AddOrUpdate(
                    m => m.Id,
                    new Manager { Id = 1, Firstname = "John", Lastname = "Miller", Email = "texas.toast@email.com", HomePhone = "(888) 555-1111", CellPhone = "(888) 555-1111", WorkPhone = "(888) 555-1111", DateOfBirth = Convert.ToDateTime("1960-01-10") },
                    new Manager { Id = 2, Firstname = "Mary", Lastname = "Jones", Email = "rocket.man@email.com", HomePhone = "(877) 555-2222", CellPhone = "(877) 555-2222", WorkPhone = "(877) 555-2222", DateOfBirth = Convert.ToDateTime("1983-03-14") },
                    new Manager { Id = 3, Firstname = "Steve", Lastname = "Moore", Email = "cargo.ship@email.com", HomePhone = "(866) 555-3333", CellPhone = "(866) 555-3333", WorkPhone = "(866) 555-3333", DateOfBirth = Convert.ToDateTime("1972-09-21") }
                    );
                context.SaveChanges();
            }  // save seed data for Manager

            if (context.JobTasks.Count() == 0)
            {
                context.JobTasks.AddOrUpdate(
                    j => j.Id,
                    new JobTask { Id = 1, Name = "Design the App", Description = "Description1", ImageFilename = "/Content/Images/SampleImage1Small.jpg" },
                    new JobTask { Id = 2, Name = "Build the App", Description = "Description2", ImageFilename = "/Content/Images/SampleImage2Small.jpg" },
                    new JobTask { Id = 3, Name = "Deploy the App", Description = "Description3", ImageFilename = "/Content/Images/SampleImage3Small.jpg" }
                    );
                context.SaveChanges();
            }  // save seed data for JobTask

            if (context.Buildings.Count() == 0)
            {
                context.Buildings.AddOrUpdate(
                    b => b.Id,
                    new Building { Id = 1, Name = "Building One", Address = "Address1" },
                    new Building { Id = 2, Name = "Building Two", Address = "Address2" },
                    new Building { Id = 3, Name = "Building Three", Address = "Address3" }
                    );
                context.SaveChanges();
            }  // save seed data for Building

            if (context.Persons.Count() == 0)
            {
                context.Persons.AddOrUpdate(
                    p => p.Id,
                    new Person { Id = 1, Firstname = "John", Lastname = "Miller", Email = "texas.toast@email.com", HomePhone = "(888) 555-1111", CellPhone = "(888) 555-1111", WorkPhone = "(888) 555-1111", DateOfBirth = Convert.ToDateTime("1960-01-10"), ManagerId = 1, JobFlowchartDiagramData = "JobFlowchartDiagramData1", JobTaskFlowchartDiagramData = "JobTaskFlowchartDiagramData1" },
                    new Person { Id = 2, Firstname = "Mary", Lastname = "Jones", Email = "rocket.man@email.com", HomePhone = "(877) 555-2222", CellPhone = "(877) 555-2222", WorkPhone = "(877) 555-2222", DateOfBirth = Convert.ToDateTime("1983-03-14"), ManagerId = 2, JobFlowchartDiagramData = "JobFlowchartDiagramData2", JobTaskFlowchartDiagramData = "JobTaskFlowchartDiagramData2" },
                    new Person { Id = 3, Firstname = "Steve", Lastname = "Moore", Email = "cargo.ship@email.com", HomePhone = "(866) 555-3333", CellPhone = "(866) 555-3333", WorkPhone = "(866) 555-3333", DateOfBirth = Convert.ToDateTime("1972-09-21"), ManagerId = 3, JobFlowchartDiagramData = "JobFlowchartDiagramData3", JobTaskFlowchartDiagramData = "JobTaskFlowchartDiagramData3" }
                    );
                context.SaveChanges();
            }  // save seed data for Person

            if (context.Departments.Count() == 0)
            {
                context.Departments.AddOrUpdate(
                    d => d.Id,
                    new Department { Id = 1, Name = "Accounting", BuildingId = 1 },
                    new Department { Id = 2, Name = "IT", BuildingId = 2 },
                    new Department { Id = 3, Name = "Operations", BuildingId = 3 }
                    );
                context.SaveChanges();
            }  // save seed data for Department

            if (context.JobTaskPersons.Count() == 0)
            {
                context.JobTaskPersons.AddOrUpdate(
                    j => j.Id,
                    new JobTaskPersons { Id = 1, JobTaskId = 1, PersonId = 1 },
                    new JobTaskPersons { Id = 2, JobTaskId = 2, PersonId = 2 },
                    new JobTaskPersons { Id = 3, JobTaskId = 3, PersonId = 3 }
                    );
                context.SaveChanges();
            }  // save seed data for JobTaskPersons


        }
    }
}
