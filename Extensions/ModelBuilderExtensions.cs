using _AbsoPickUp.Models;
using Microsoft.EntityFrameworkCore;

namespace _AbsoPickUp.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTypes>().HasData(
               new UserTypes { ID = 1, Type = "Individual" },
               new UserTypes { ID = 2, Type = "Business" },
               new UserTypes { ID = 3, Type = "Admin" });

            modelBuilder.Entity<VehicleTypes>().HasData(
               new VehicleTypes { VehicleTypeId = 1, VehicleType = "Scooter" },
               new VehicleTypes { VehicleTypeId = 2, VehicleType = "Car" },
               new VehicleTypes { VehicleTypeId = 3, VehicleType = "Bakkie" });

            modelBuilder.Entity<DeliveryTypes>().HasData(
               new DeliveryTypes { DeliveryTypeId = 1, Description = "Normal" },
               new DeliveryTypes { DeliveryTypeId = 2, Description = "Express" },
               new DeliveryTypes { DeliveryTypeId = 3, Description = "Bakkie" });

            modelBuilder.Entity<DeliveryStatus>().HasData(
               new DeliveryStatus { StatusId = 1, Description = "Unassigned" },
               new DeliveryStatus { StatusId = 2, Description = "Assigned" },
               new DeliveryStatus { StatusId = 3, Description = "With Driver" },
               new DeliveryStatus { StatusId = 4, Description = "Delivery on Route" },
               new DeliveryStatus { StatusId = 5, Description = "Arrived" },
               new DeliveryStatus { StatusId = 6, Description = "Delivered" },
               new DeliveryStatus { StatusId = 7, Description = "Cancelled" });

            modelBuilder.Entity<DeliveryPrice>().HasData(
               new DeliveryPrice { Id = 1, TypeId = 1, DeliverBy = "Same day delivery", Amount = 45 },
               new DeliveryPrice { Id = 2, TypeId = 2, DeliverBy = "Within 2 hrs delivery", Amount = 80 },
               new DeliveryPrice { Id = 3, TypeId = 3, DeliverBy = "Immediate delivery", Amount = 300 });

            modelBuilder.Entity<VehicleBrand>().HasData(
                new VehicleBrand { BrandId = 1, BrandName = "GWM", VehicleTypeId = 3 },
                new VehicleBrand { BrandId = 2, BrandName = "Ferrari", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 3, BrandName = "Lexus", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 4, BrandName = "Austin-Healey", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 5, BrandName = "Alfa Romeo", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 6, BrandName = "Fiat", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 7, BrandName = "Aston Martin", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 8, BrandName = "Maserati", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 9, BrandName = "Audi", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 10, BrandName = "Ford", VehicleTypeId = 3 },
                new VehicleBrand { BrandId = 11, BrandName = "Mahindra", VehicleTypeId = 3 },
                new VehicleBrand { BrandId = 12, BrandName = "Subaru", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 13, BrandName = "Hero", VehicleTypeId = 1 },
                new VehicleBrand { BrandId = 14, BrandName = "BMW", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 15, BrandName = "Land Cruiser", VehicleTypeId = 3 },
                new VehicleBrand { BrandId = 16, BrandName = "Suzuki", VehicleTypeId = 1 },
                new VehicleBrand { BrandId = 17, BrandName = "TVS", VehicleTypeId = 1 },
                new VehicleBrand { BrandId = 18, BrandName = "Mazda", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 19, BrandName = "Tata", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 20, BrandName = "KTM", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 21, BrandName = "Cadillac", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 22, BrandName = "Harley-Davidson", VehicleTypeId = 1 },
                new VehicleBrand { BrandId = 23, BrandName = "Toyota", VehicleTypeId = 3 },
                new VehicleBrand { BrandId = 24, BrandName = "Pontiac", VehicleTypeId = 3 },
                new VehicleBrand { BrandId = 25, BrandName = "Packard", VehicleTypeId = 3 },
                new VehicleBrand { BrandId = 26, BrandName = "Honda", VehicleTypeId = 1 },
                new VehicleBrand { BrandId = 27, BrandName = "Yamaha", VehicleTypeId = 1 },
                new VehicleBrand { BrandId = 28, BrandName = "Hyundai", VehicleTypeId = 3 },
                new VehicleBrand { BrandId = 29, BrandName = "Volkswagen", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 30, BrandName = "Chevrolet", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 31, BrandName = "Hummer", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 32, BrandName = "Volvo", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 33, BrandName = "Vauxhall", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 34, BrandName = "Chrysler", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 35, BrandName = "Isuzu", VehicleTypeId = 3 },
                new VehicleBrand { BrandId = 36, BrandName = "Nissan", VehicleTypeId = 3 },
                new VehicleBrand { BrandId = 37, BrandName = "Leyland", VehicleTypeId = 3 },
                new VehicleBrand { BrandId = 38, BrandName = "Citroen", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 39, BrandName = "Bajaj", VehicleTypeId = 1 },
                new VehicleBrand { BrandId = 40, BrandName = "Opel", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 41, BrandName = "Mitsubishi", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 42, BrandName = "Jeep", VehicleTypeId = 3 },
                new VehicleBrand { BrandId = 43, BrandName = "Peugeot", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 44, BrandName = "Kawasaki", VehicleTypeId = 1 },
                new VehicleBrand { BrandId = 45, BrandName = "Porsche", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 46, BrandName = "Datsun", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 47, BrandName = "Daewoo", VehicleTypeId = 3 },
                new VehicleBrand { BrandId = 48, BrandName = "Kia", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 49, BrandName = "Daihatsu", VehicleTypeId = 3 },
                new VehicleBrand { BrandId = 50, BrandName = "Renault", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 51, BrandName = "Piaggio", VehicleTypeId = 1 },
                new VehicleBrand { BrandId = 52, BrandName = "Daimler", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 53, BrandName = "Rover", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 54, BrandName = "Austin", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 55, BrandName = "Saab", VehicleTypeId = 2 },
                new VehicleBrand { BrandId = 56, BrandName = "Willys", VehicleTypeId = 3 });

            modelBuilder.Entity<VehicleColour>().HasData(
               new VehicleColour { VehicleColorId = 1, VehicleColorName = "White", VehicleColorCode = "#ffffff" },
                new VehicleColour { VehicleColorId = 2, VehicleColorName = "Silver", VehicleColorCode = "#c0c0c0" },
                new VehicleColour { VehicleColorId = 3, VehicleColorName = "Gray", VehicleColorCode = "#808080" },
                new VehicleColour { VehicleColorId = 4, VehicleColorName = "Red", VehicleColorCode = "#ff3b30" },
                new VehicleColour { VehicleColorId = 5, VehicleColorName = "Blue", VehicleColorCode = "#007aff" },
                new VehicleColour { VehicleColorId = 6, VehicleColorName = "Beige", VehicleColorCode = "#f5f5dc" },
                new VehicleColour { VehicleColorId = 7, VehicleColorName = "Orange", VehicleColorCode = "#ff9500" },
                new VehicleColour { VehicleColorId = 8, VehicleColorName = "Gold", VehicleColorCode = "#ffd700" },
                new VehicleColour { VehicleColorId = 9, VehicleColorName = "Black", VehicleColorCode = "#000000" },
                new VehicleColour { VehicleColorId = 10, VehicleColorName = "Green", VehicleColorCode = "#4cd964" },
                new VehicleColour { VehicleColorId = 11, VehicleColorName = "Yellow", VehicleColorCode = "#ffcc00" },
                new VehicleColour { VehicleColorId = 12, VehicleColorName = "Teal Blue", VehicleColorCode = "#5ac8fa" },
                new VehicleColour { VehicleColorId = 13, VehicleColorName = "Purple", VehicleColorCode = "#5856d6" },
                new VehicleColour { VehicleColorId = 14, VehicleColorName = "Pink", VehicleColorCode = "#ff2d55" },
                new VehicleColour { VehicleColorId = 15, VehicleColorName = "Brown", VehicleColorCode = "#a52a2a" },
                new VehicleColour { VehicleColorId = 16, VehicleColorName = "Metallic Silver", VehicleColorCode = "#999da0" },
                new VehicleColour { VehicleColorId = 17, VehicleColorName = "Metallic Gun Metal", VehicleColorCode = "#444e54" },
                new VehicleColour { VehicleColorId = 18, VehicleColorName = "Matte Gray", VehicleColorCode = "#26282a" },
                new VehicleColour { VehicleColorId = 19, VehicleColorName = "Matte Light Grey", VehicleColorCode = "#515554" },
                new VehicleColour { VehicleColorId = 20, VehicleColorName = "Util Black", VehicleColorCode = "#151921" },
                new VehicleColour { VehicleColorId = 21, VehicleColorName = "Worn Graphite", VehicleColorCode = "#363a3f" },
                new VehicleColour { VehicleColorId = 22, VehicleColorName = "Worn Silver", VehicleColorCode = "#d3d3d3" },
                new VehicleColour { VehicleColorId = 23, VehicleColorName = "Metallic Red", VehicleColorCode = "#c00e1a" },
                new VehicleColour { VehicleColorId = 24, VehicleColorName = "Metallic Classic Gold", VehicleColorCode = "#c2944f" },
                new VehicleColour { VehicleColorId = 25, VehicleColorName = "Metallic Orange", VehicleColorCode = "#f78616" },
                new VehicleColour { VehicleColorId = 26, VehicleColorName = "Matte Yellow", VehicleColorCode = "#ffc91f" },
                new VehicleColour { VehicleColorId = 27, VehicleColorName = "Metallic Green", VehicleColorCode = "#155c2d" },
                new VehicleColour { VehicleColorId = 28, VehicleColorName = "Worn Sea Wash", VehicleColorCode = "#65867f" },
                new VehicleColour { VehicleColorId = 29, VehicleColorName = "Metallic Blue", VehicleColorCode = "#47578f" },
                new VehicleColour { VehicleColorId = 30, VehicleColorName = "Metallic Taxi Yellow", VehicleColorCode = "#ffcf20" },
                new VehicleColour { VehicleColorId = 31, VehicleColorName = "Metallic Lime", VehicleColorCode = "#98d223" },
                new VehicleColour { VehicleColorId = 32, VehicleColorName = "Metallic Champagne", VehicleColorCode = "#9b8c78" },
                new VehicleColour { VehicleColorId = 33, VehicleColorName = "Metallic Dark Ivory", VehicleColorCode = "#473f2b" },
                new VehicleColour { VehicleColorId = 34, VehicleColorName = "Metallic Light Brown", VehicleColorCode = "#775c3e" },
                new VehicleColour { VehicleColorId = 35, VehicleColorName = "Metallic Beechwood", VehicleColorCode = "#a4965f" },
                new VehicleColour { VehicleColorId = 36, VehicleColorName = "Metallic Sun Bleeched Sand", VehicleColorCode = "#dfd5b2" },
                new VehicleColour { VehicleColorId = 37, VehicleColorName = "Metallic Cream", VehicleColorCode = "#f7edd5" },
                new VehicleColour { VehicleColorId = 38, VehicleColorName = "Metallic White", VehicleColorCode = "#fffff6" },
                new VehicleColour { VehicleColorId = 39, VehicleColorName = "Worn Brown", VehicleColorCode = "#453831" },
                new VehicleColour { VehicleColorId = 40, VehicleColorName = "Brushed Steel", VehicleColorCode = "#6a747c" },
                new VehicleColour { VehicleColorId = 41, VehicleColorName = "Brushed Aluminium", VehicleColorCode = "#9ba0a8" },
                new VehicleColour { VehicleColorId = 42, VehicleColorName = "Chrome", VehicleColorCode = "#5870a1" },
                new VehicleColour { VehicleColorId = 43, VehicleColorName = "Matte Brown", VehicleColorCode = "#bcac8f" },
                new VehicleColour { VehicleColorId = 44, VehicleColorName = "Matte Purple", VehicleColorCode = "#6b1f7b" });

            modelBuilder.Entity<SouthAfricaProvinces>().HasData(
               new SouthAfricaProvinces { ProvinceId = 1, Name = "Eastern Cape" },
                new SouthAfricaProvinces { ProvinceId = 2, Name = "Free State" },
                new SouthAfricaProvinces { ProvinceId = 3, Name = "Gauteng" },
                new SouthAfricaProvinces { ProvinceId = 4, Name = "KwaZulu-Natal" },
                new SouthAfricaProvinces { ProvinceId = 5, Name = "Limpopo" },
                new SouthAfricaProvinces { ProvinceId = 6, Name = "Mpumalanga" },
                new SouthAfricaProvinces { ProvinceId = 7, Name = "Northern Cape" },
                new SouthAfricaProvinces { ProvinceId = 8, Name = "North West" },
                new SouthAfricaProvinces { ProvinceId = 9, Name = "Western Cape" });

            modelBuilder.Entity<DocumentTypes>().HasData(
               new DocumentTypes { DocTypeId = 1, DocTypeDescription = "License" },
               new DocumentTypes { DocTypeId = 2, DocTypeDescription = "ProofOfResidence" },
               new DocumentTypes { DocTypeId = 3, DocTypeDescription = "NumberPlatePic" },
               new DocumentTypes { DocTypeId = 4, DocTypeDescription = "VehicleRegisteration" },
               new DocumentTypes { DocTypeId = 5, DocTypeDescription = "IDBook" },
               new DocumentTypes { DocTypeId = 6, DocTypeDescription = "IDCard" },
               new DocumentTypes { DocTypeId = 7, DocTypeDescription = "Selfie" },
               new DocumentTypes { DocTypeId = 8, DocTypeDescription = "ElectionCard" },
               new DocumentTypes { DocTypeId = 9, DocTypeDescription = "BirthCertificate" },
               new DocumentTypes { DocTypeId = 10, DocTypeDescription = "Passport" },
               new DocumentTypes { DocTypeId = 11, DocTypeDescription = "CarDiscPic" },
               new DocumentTypes { DocTypeId = 12, DocTypeDescription = "DriverProfilePic" },
               new DocumentTypes { DocTypeId = 13, DocTypeDescription = "Any ID Proof" },
               new DocumentTypes { DocTypeId = 14, DocTypeDescription = "Image Before Packing" },
               new DocumentTypes { DocTypeId = 15, DocTypeDescription = "Image After Packing" },
               new DocumentTypes { DocTypeId = 16, DocTypeDescription = "Vehicle Frontside Image" },
               new DocumentTypes { DocTypeId = 17, DocTypeDescription = "Vehicle Backside Image" },
               new DocumentTypes { DocTypeId = 18, DocTypeDescription = "IDCardBack" },
               new DocumentTypes { DocTypeId = 19, DocTypeDescription = "VAT File" },
               new DocumentTypes { DocTypeId = 20, DocTypeDescription = "ChamberOfCommerce File" },
               new DocumentTypes { DocTypeId = 21, DocTypeDescription = "Business License File" },
               new DocumentTypes { DocTypeId = 22, DocTypeDescription = "Agreement File" });

            modelBuilder.Entity<UserDetails>().Property(p => p.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<UserDetails>().Property(p => p.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<UserDetails>().Property(p => p.Email).IsRequired(false);
            modelBuilder.Entity<UserDetails>().Property(p => p.PhoneNumber).IsRequired(false);
            modelBuilder.Entity<DriverDetails>().Property(p => p.Otp).IsRequired(false);

            modelBuilder.Entity<DeliveryRequest>().Property(a => a.SenderLat).HasPrecision(18, 9);
            modelBuilder.Entity<DeliveryRequest>().Property(a => a.SenderLong).HasPrecision(18, 9);

            modelBuilder.Entity<DeliveryRequest>().Property(a => a.ReceiverLat).HasPrecision(18, 9);
            modelBuilder.Entity<DeliveryRequest>().Property(a => a.ReceiverLong).HasPrecision(18, 9);

            modelBuilder.Entity<DeliveryDetails>().Property(a => a.DriverLat).HasPrecision(18, 9);
            modelBuilder.Entity<DeliveryDetails>().Property(a => a.DriverLong).HasPrecision(18, 9);

            modelBuilder.Entity<ParcelNotifications>().Property(a => a.DriverLat).HasPrecision(18, 9);
            modelBuilder.Entity<ParcelNotifications>().Property(a => a.DriverLon).HasPrecision(18, 9);
        }
    }
}
