using _AbsoPickUp.Data;
using _AbsoPickUp.IServices;
using _AbsoPickUp.Models;
using _AbsoPickUp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static _AbsoPickUp.Common.GlobalVariables;


namespace _AbsoPickUp.Repositories
{
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _context;
        public VehicleService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<SouthAfricanProvincesViewModel> GetAllProvinces()
        {
            // Fetch all south african provinces
            var _allProvinces = _context.SouthAfricaProvinces.Select(
                                                 x => new SouthAfricanProvincesViewModel
                                                 {
                                                     ProvinceId = x.ProvinceId,
                                                     Name = x.Name
                                                 }).OrderBy(x => x.Name).ToList();
            return _allProvinces;
        }
        //get all vehicle brands for SA
        public List<VehicleBrandViewModel> GetAllVehicleBrands()
        {
            var _allVehicleBrands = _context.VehicleBrand.Select(
                                                     x => new VehicleBrandViewModel
                                                     {
                                                         BrandId = x.BrandId,
                                                         BrandName = x.BrandName,
                                                         VehicleTypeId = x.VehicleTypeId
                                                     }).OrderBy(x => x.BrandName).ToList();
            return _allVehicleBrands;
        }
        //get all vehicle color for SA
        public List<VehicleColourViewModel> GetAllVehicleColours()
        {
            var _allVehicleColors = _context.VehicleColour.Select(
                                                     x => new VehicleColourViewModel
                                                     {
                                                         VehicleColorId = x.VehicleColorId,
                                                         VehicleColorName = x.VehicleColorName,
                                                         VehicleColorCode = x.VehicleColorCode
                                                     }).OrderBy(x => x.VehicleColorName).ToList();
            return _allVehicleColors;
        }
        //add or update vehicle brands for SA
        public int AddUpdateVehicleBrands(VehicleBrand model)
        {
            try
            {
                if (model.BrandId > 0)
                {
                    _context.VehicleBrand.Update(model);
                }
                else
                {
                    _context.VehicleBrand.Add(model);
                }
                return _context.SaveChanges();
            }
            catch { throw; }
        }
        //add or update vehicle color for SA
        public int AddUpdateVehicleColours(VehicleColour model)
        {
            try
            {
                if (model.VehicleColorId > 0)
                {
                    _context.VehicleColour.Update(model);
                }
                else
                {
                    _context.VehicleColour.Add(model);
                }
                return _context.SaveChanges();
            }
            catch { throw; }
        }
        //get all vehicle types
        public List<VehicleTypeViewModel> GetAllVehicleTypes()
        {
            var _allVehicleTypes = _context.VehicleTypes.Select(
                                                     x => new VehicleTypeViewModel
                                                     {
                                                         VehicleTypeId = x.VehicleTypeId,
                                                         VehicleType = x.VehicleType
                                                     }).OrderBy(x => x.VehicleType).ToList();
            return _allVehicleTypes;
        }
        //add vehicle details
        public bool AddVehicleDetails(VehicleDetails model)
        {
            try
            {
                var result = _context.DriverDetails.FirstOrDefault(x => x.DriverId == model.DriverId);
                if (result != null)
                {
                    result.ScreenId = (int)Screens.VehicleDetails;
                    _context.DriverDetails.Update(result);
                }
                var response = _context.VehicleDetails.Add(model);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //add vehicle docs
        public bool AddVehicleDocuments(DriverDocuments model)
        {
            try
            {
                var response = _context.DriverDocuments.Add(model);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //update vehicle docs
        public int UpdateVehicleDocuments(DriverDocuments model)
        {
            try
            {
                var existingDoc = _context.DriverDocuments.FirstOrDefault(x => x.DriverId == model.DriverId && x.DocTypeId == model.DocTypeId);
                if (existingDoc != null)
                {
                    existingDoc.DocImgPath = model.DocImgPath;
                    var response = _context.DriverDocuments.Update(existingDoc);
                    return _context.SaveChanges();
                }
                return 0;
            }
            catch
            {
                throw;
            }
        }
        //update vehicle details
        public int UpdateVehicleDetails(VehicleDetails model)
        {
            try
            {
                var result = _context.VehicleDetails.FirstOrDefault(x => x.DriverId == model.DriverId);
                if (result != null)
                {
                    result.BrandId = model.BrandId;
                    result.RegisterationNumber = model.RegisterationNumber;
                    result.VehicleColorId = model.VehicleColorId;
                    result.VehicleTypeId = model.VehicleTypeId;
                    _context.VehicleDetails.Update(result);
                    return _context.SaveChanges();
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //get vehicle details
        public async Task<VehicleInfoViewModel> GetVehicleDetails(string id)
        {
            try
            {
                var details = await (from vd in _context.VehicleDetails
                                     join vb in _context.VehicleBrand
                                     on vd.BrandId equals vb.BrandId
                                     join vt in _context.VehicleTypes
                                     on vd.VehicleTypeId equals vt.VehicleTypeId
                                     join vc in _context.VehicleColour
                                     on vd.VehicleColorId equals vc.VehicleColorId
                                     where vd.VehicleId.ToString() == id
                                     select new VehicleInfoViewModel()
                                     {
                                         DriverId = vd.DriverId,
                                         VehicleType = vt.VehicleType,
                                         VehicleBrand = vb.BrandName,
                                         VehicleColor = vc.VehicleColorName,
                                         RegistrationNumber = vd.RegisterationNumber
                                     }).FirstOrDefaultAsync();

                var VehicleFrontImgPath = _context.DriverDocuments.FirstOrDefault(x => x.DocTypeId == (int)DocTypes.VehicleFrontsideImage && x.DriverId == details.DriverId);
                details.VehicleFrontImgPath = VehicleFrontImgPath == null ? string.Empty : VehicleFrontImgPath.DocImgPath;
                var VehicleBackImgPath = _context.DriverDocuments.FirstOrDefault(x => x.DocTypeId == (int)DocTypes.VehicleBacksideImage && x.DriverId == details.DriverId);
                details.VehicleBackImgPath = VehicleBackImgPath == null ? string.Empty : VehicleBackImgPath.DocImgPath;

                return details;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
