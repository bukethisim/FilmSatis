﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MVCFilmSatis.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : Customer
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here

            //Sitenin içinde kullanıcı bilgilerine her yerden erişebilmek amacıyla claim yaptık.
            Claim c = new Claim("TC",this.TC.ToString());  //ApplicationUser'ın içinde olduğundan this o andaki userı verir.
            userIdentity.AddClaim(c);

            Claim c2 = new Claim("NameLastname", NameSurName);
            userIdentity.AddClaim(c2);

            if(this.Age < 22)
            {
                //Session["AgeGroup"]="Young";
                Claim c3 = new Claim("AgeGroup", "Young");
                userIdentity.AddClaim(c3);
            }

            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //public virtual DbSet<Customer> Customers { get; set; }
        //public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Slider> Sliders { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasKey(x => x.MovieId);

            modelBuilder.Entity<ShoppingCart>()
               .HasKey(x => x.ShoppingCartId);

            modelBuilder.Entity<Order>()
                .HasKey(x => x.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasKey(x => x.OrderItemId);

            modelBuilder.Entity<Slider>()
                .HasKey(x => x.SliderId);

            modelBuilder.Entity<ShoppingCart>()
                .HasMany(x => x.Movies)
                .WithMany(x => x.ShoppingCarts);

            modelBuilder.Entity<Customer>()
                .HasOptional(x => x.ShoppingCart)
                .WithRequired(x => x.Customer);

            modelBuilder.Entity<Order>()     //1
                .HasMany(x => x.OrderItems)
                .WithRequired(x => x.Order);

            modelBuilder.Entity<Order>()        //2                 1-2 aynı gösterim
                .HasRequired(x => x.Customer)
                .WithMany(x => x.Orders);

            modelBuilder.Entity<Movie>()
                .HasMany(x => x.OrderItems)
                .WithRequired(x => x.Movie);


            base.OnModelCreating(modelBuilder);
        }
    }
}