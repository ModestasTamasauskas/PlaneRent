using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Runtime.Serialization;
using PlaneRental.Business.Entities;
using Core.Common.Contracts;

namespace PlaneRental.Data
{
    public class PlaneRentalContext : DbContext
    {
        public PlaneRentalContext()
            : base(@"Data Source=DESKTOP-H8DGSLF\SQLEXPRESS;Initial Catalog=PlaneRental;Integrated Security=True")
        {
            Database.SetInitializer<PlaneRentalContext>(null);
        }

        public DbSet<Account> AccountSet { get; set; }

        public DbSet<Plane> PlaneSet { get; set; }

        public DbSet<Rental> RentalSet { get; set; }

        public DbSet<Reservation> ReservationSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Ignore<PropertyChangedEventHandler>();
            modelBuilder.Ignore<ExtensionDataObject>();
            modelBuilder.Ignore<IIdentifiableEntity>();

            modelBuilder.Entity<Account>().HasKey<int>(e => e.AccountId).Ignore(e => e.EntityId);
            modelBuilder.Entity<Plane>().HasKey<int>(e => e.PlaneId).Ignore(e => e.EntityId);
            modelBuilder.Entity<Rental>().HasKey<int>(e => e.RentalId).Ignore(e => e.EntityId);
            modelBuilder.Entity<Reservation>().HasKey<int>(e => e.ReservationId).Ignore(e => e.EntityId);
            modelBuilder.Entity<Plane>().Ignore(e => e.CurrentlyRented);
        }
    }
}
