using _AbsoPickUp.Models;
using _AbsoPickUp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _AbsoPickUp.IServices
{
    public interface IVehicleService
    {
        List<SouthAfricanProvincesViewModel> GetAllProvinces();
        List<VehicleBrandViewModel> GetAllVehicleBrands();
        List<VehicleColourViewModel> GetAllVehicleColours();
        List<VehicleTypeViewModel> GetAllVehicleTypes();
        bool AddVehicleDetails(VehicleDetails model);
        bool AddVehicleDocuments(DriverDocuments model);
        Task<VehicleInfoViewModel> GetVehicleDetails(string id);
        int UpdateVehicleDetails(VehicleDetails model);
        int UpdateVehicleDocuments(DriverDocuments model);
        int AddUpdateVehicleBrands(VehicleBrand model);
        int AddUpdateVehicleColours(VehicleColour model);
    }
}
