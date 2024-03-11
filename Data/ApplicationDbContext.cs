using _AbsoPickUp.Extensions;
using _AbsoPickUp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _AbsoPickUp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Seed();
        }
        public DbSet<BusinessDocuments> BusinessDocuments { get; set; }
        public DbSet<BusinessDetails> BusinessDetails { get; set; }
        public DbSet<UserTypes> UserTypes { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<DocumentTypes> DocumentTypes { get; set; }
        public DbSet<DriverDocuments> DriverDocuments { get; set; }
        public DbSet<DriverDetails> DriverDetails { get; set; }
        public DbSet<IndividualUserDocuments> IndividualUserDocuments { get; set; }
        public DbSet<VehicleBrand> VehicleBrand { get; set; }
        public DbSet<VehicleColour> VehicleColour { get; set; }
        public DbSet<VehicleDetails> VehicleDetails { get; set; }
        public DbSet<VehicleTypes> VehicleTypes { get; set; }
        public DbSet<DeliveryTypes> DeliveryTypes { get; set; }
        public DbSet<SouthAfricaProvinces> SouthAfricaProvinces { get; set; }
        public DbSet<DeliveryPrice> DeliveryPrice { get; set; }
        public DbSet<DeliveryStatus> DeliveryStatus { get; set; }
        public DbSet<DeliveryRequest> DeliveryRequest { get; set; }
        public DbSet<DeliveryDetails> DeliveryDetails { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
        public DbSet<ParcelDetails> ParcelDetails { get; set; }
        public DbSet<ParcelSubCategory> ParcelSubCategory { get; set; }
        public DbSet<ParcelCategory> ParcelCategory { get; set; }
        public DbSet<TransactionMaster> TransactionMaster { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<ParcelNotifications> ParcelNotifications { get; set; }
        public DbSet<DriverRatings> DriverRatings { get; set; }
        public DbSet<DriverWorkStatus> DriverWorkStatus { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<CancelledOrders> CancelledOrders { get; set; }
        public DbSet<ParcelDelivery> ParcelDelivery { get; set; }
        public DbSet<DriverBankDetails> DriverBankDetails { get; set; }
    }
}
