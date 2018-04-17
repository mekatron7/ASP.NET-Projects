namespace CarDealership.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CarDealershipContext : DbContext
    {
        public CarDealershipContext()
            : base("name=DealershipDB")
        {
        }

        public virtual DbSet<BodyStyle> BodyStyles { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<ExteriorColor> ExteriorColors { get; set; }
        public virtual DbSet<InteriorColor> InteriorColors { get; set; }
        public virtual DbSet<Make> Makes { get; set; }
        public virtual DbSet<Model> Models { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<Special> Specials { get; set; }
        public virtual DbSet<Transmission> Transmissions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BodyStyle>()
                .Property(e => e.BSName)
                .IsUnicode(false);

            modelBuilder.Entity<BodyStyle>()
                .HasMany(e => e.Cars)
                .WithRequired(e => e.BodyStyle)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Car>()
                .Property(e => e.VIN_)
                .IsUnicode(false);

            modelBuilder.Entity<Car>()
                .Property(e => e.CarType)
                .IsUnicode(false);

            modelBuilder.Entity<Car>()
                .Property(e => e.Purchased)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Car>()
                .Property(e => e.Featured)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Car>()
                .Property(e => e.MSRP)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Car>()
                .Property(e => e.SalePrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Car>()
                .Property(e => e.CarDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Car>()
                .Property(e => e.CarPicture)
                .IsUnicode(false);

            modelBuilder.Entity<Car>()
                .HasMany(e => e.Purchases)
                .WithRequired(e => e.Car)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.ContactName)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.ContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.ContactPhone)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.ContactMessage)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.VIN_)
                .IsUnicode(false);

            modelBuilder.Entity<ExteriorColor>()
                .Property(e => e.ExtColorName)
                .IsUnicode(false);

            modelBuilder.Entity<ExteriorColor>()
                .Property(e => e.ExtColorType)
                .IsUnicode(false);

            modelBuilder.Entity<ExteriorColor>()
                .HasMany(e => e.Cars)
                .WithRequired(e => e.ExteriorColor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InteriorColor>()
                .Property(e => e.IntColorName)
                .IsUnicode(false);

            modelBuilder.Entity<InteriorColor>()
                .Property(e => e.IntColorType)
                .IsUnicode(false);

            modelBuilder.Entity<InteriorColor>()
                .HasMany(e => e.Cars)
                .WithRequired(e => e.InteriorColor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Make>()
                .Property(e => e.MakeName)
                .IsUnicode(false);

            modelBuilder.Entity<Make>()
                .HasMany(e => e.Models)
                .WithRequired(e => e.Make)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Model>()
                .Property(e => e.ModelName)
                .IsUnicode(false);

            modelBuilder.Entity<Model>()
                .Property(e => e.ModelEdition)
                .IsUnicode(false);

            modelBuilder.Entity<Model>()
                .HasMany(e => e.Cars)
                .WithRequired(e => e.Model)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Purchase>()
                .Property(e => e.VIN_)
                .IsUnicode(false);

            modelBuilder.Entity<Purchase>()
                .Property(e => e.PurchaseName)
                .IsUnicode(false);

            modelBuilder.Entity<Purchase>()
                .Property(e => e.PurchasePhone)
                .IsUnicode(false);

            modelBuilder.Entity<Purchase>()
                .Property(e => e.PurchaseEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Purchase>()
                .Property(e => e.Street1)
                .IsUnicode(false);

            modelBuilder.Entity<Purchase>()
                .Property(e => e.Street2)
                .IsUnicode(false);

            modelBuilder.Entity<Purchase>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<Purchase>()
                .Property(e => e.PState)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Purchase>()
                .Property(e => e.PurchasePrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Purchase>()
                .Property(e => e.PurchaseType)
                .IsUnicode(false);

            modelBuilder.Entity<Special>()
                .Property(e => e.SpecialName)
                .IsUnicode(false);

            modelBuilder.Entity<Special>()
                .Property(e => e.SpecialDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Transmission>()
                .Property(e => e.TransmissionType)
                .IsUnicode(false);

            modelBuilder.Entity<Transmission>()
                .HasMany(e => e.Cars)
                .WithRequired(e => e.Transmission)
                .WillCascadeOnDelete(false);
        }
    }
}
