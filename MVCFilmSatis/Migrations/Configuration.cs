namespace MVCFilmSatis.Migrations
{
    using MVCFilmSatis.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;

    internal sealed class Configuration : DbMigrationsConfiguration<MVCFilmSatis.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MVCFilmSatis.Models.ApplicationDbContext context)
        {
            //admin@.com
            //Aa123456!

            //bir kullanýcý yoksa oluþtur

            //administrator adýnda bir role oluþtur

            //admin kullanýcýsýný administrator rolüne dahil et

            if (!context.Users.Any(x => x.Email == "admin@a.com"))
            {
                Employee e1 = new Employee();
                e1.Email = "admin@a.com";
                e1.UserName = "admin@a.com";
                e1.ReportsToEmail = "admin@a.com";
                e1.Age = 24;
                e1.Gender = Gender.Female;

                IdentityRole role = new IdentityRole(); // yönetim kýsmý için  
                role.Name = "Administrator";
                context.Roles.Add(role);
                context.SaveChanges();

                UserStore<Employee> employeeStore = new UserStore<Employee>(context);
                UserManager<Employee> employeeManager = new UserManager<Employee>(employeeStore);

                employeeManager.Create(e1,"Aa123456!"); //kullanýcýnýn yönetici olduðunu belirtiyoruz.
                employeeManager.AddToRole(e1.Id, "Administrator");

                context.SaveChanges();
            }
        }
    }
}
