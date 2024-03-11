using _AbsoPickUp.Common;
using _AbsoPickUp.IServices;
using _AbsoPickUp.LoggerService;
using _AbsoPickUp.Models;
using _AbsoPickUp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static _AbsoPickUp.Common.GlobalVariables;

namespace _AbsoPickUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IDriverService _driverService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILoggerManager _logger;
        public VehiclesController(ILoggerManager logger, IDriverService driverService, IVehicleService vehicleService, IWebHostEnvironment hostingEnvironment)
        {
            _vehicleService = vehicleService;
            _hostingEnvironment = hostingEnvironment;
            _driverService = driverService;
            _logger = logger;
        }

        #region Get All Provinces
        [HttpGet]
        [Route("GetAllProvinces")]
        public ActionResult GetAllProvinces()
        {
            try
            {
                var list = _vehicleService.GetAllProvinces();
                return Ok(new { status = true, message = ResponseMessages.msgGotSuccess, data = new { list }, code = StatusCodes.Status200OK });
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Vehicles/GetAllProvinces : " + ex.Message.ToString());
                return Ok(new { status = false, message = ResponseMessages.msgSomethingWentWrong, error = ex.Message, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Get All Vehicle Brands
        [HttpGet]
        [Route("GetAllVehicleBrands")]
        public ActionResult GetAllVehicleBrands()
        {
            try
            {
                var list = _vehicleService.GetAllVehicleBrands();

                var carBrands = list.Where(x => x.VehicleTypeId == (int)VehicleType.Car).ToList();
                var scooterBrands = list.Where(x => x.VehicleTypeId == (int)VehicleType.Scooter).ToList();
                var bakkieBrands = list.Where(x => x.VehicleTypeId == (int)VehicleType.Bakkie).ToList();

                return Ok(new { status = true, message = ResponseMessages.msgGotSuccess, carBrands = new { carBrands }, scooterBrands = new { scooterBrands }, bakkieBrands = new { bakkieBrands }, code = StatusCodes.Status200OK });
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Vehicles/GetAllVehicleBrands : " + ex.Message.ToString());
                return Ok(new { status = false, message = ResponseMessages.msgSomethingWentWrong, error = ex.Message, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Get All Vehicle Colors
        [HttpGet]
        [Route("GetAllVehicleColours")]
        public ActionResult GetAllVehicleColours()
        {
            try
            {
                var list = _vehicleService.GetAllVehicleColours();
                return Ok(new { status = true, message = ResponseMessages.msgGotSuccess, data = new { list }, code = StatusCodes.Status200OK });
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Vehicles/GetAllVehicleColours : " + ex.Message.ToString());
                return Ok(new { status = false, message = ResponseMessages.msgSomethingWentWrong, error = ex.Message, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Add or Update Vehicle Brands
        [Authorize]
        [HttpPost("AddUpdateVehicleBrands")]
        public ActionResult AddUpdateVehicleBrands(VehicleBrand model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Vehicles/AddUpdateVehicleBrands : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var _res = _vehicleService.AddUpdateVehicleBrands(model);
                if (_res > 0)
                {
                    return Ok(new { status = true, message = "Vehicle Brand" + ResponseMessages.msgAddUpdationSuccess, data = new { }, code = StatusCodes.Status200OK });
                }
                return Ok(new { status = true, message = "Vehicle Brand" + ResponseMessages.msgAddUpdateFailed, data = new { }, code = StatusCodes.Status200OK });
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Vehicles/AddUpdateVehicleBrands : " + ex.Message.ToString());
                return Ok(new { status = false, message = ResponseMessages.msgSomethingWentWrong, error = ex.Message, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Add or Update Vehicle Colors
        [Authorize]
        [HttpPost("AddUpdateVehicleColours")]
        public ActionResult AddUpdateVehicleColours(VehicleColour model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Vehicles/AddUpdateVehicleColours : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var _res = _vehicleService.AddUpdateVehicleColours(model);
                if (_res > 0)
                {
                    return Ok(new { status = true, message = "Vehicle Color" + ResponseMessages.msgAddUpdationSuccess, data = new { }, code = StatusCodes.Status200OK });
                }
                return Ok(new { status = true, message = "Vehicle Color" + ResponseMessages.msgAddUpdateFailed, data = new { }, code = StatusCodes.Status200OK });
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Vehicles/AddUpdateVehicleColours : " + ex.Message.ToString());
                return Ok(new { status = false, message = ResponseMessages.msgSomethingWentWrong, error = ex.Message, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Get All Vehicle Types
        [HttpGet]
        [Route("GetAllVehicleTypes")]
        public ActionResult GetAllVehicleTypes()
        {
            try
            {
                var list = _vehicleService.GetAllVehicleTypes();
                return Ok(new { status = true, message = ResponseMessages.msgGotSuccess, data = new { list }, code = StatusCodes.Status200OK });
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Vehicles/GetAllVehicleTypes : " + ex.Message.ToString());
                return Ok(new { status = false, message = ResponseMessages.msgSomethingWentWrong, error = ex.Message, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Add Vehicle Details
        [Authorize]
        [HttpPost("AddVehicleDetails")]
        public IActionResult AddVehicleDetails([FromForm] VehicleDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Vehicles/AddVehicleDetails : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                int _status;
                //add vehicle front side image
                var documentFile = ContentDispositionHeaderValue.Parse(model.FrontSideImage.ContentDisposition).FileName.Trim('"');
                documentFile = CommonFunctions.EnsureCorrectFilename(documentFile);
                documentFile = CommonFunctions.RenameFileName(documentFile);
                using (FileStream fsF = System.IO.File.Create(GetPathAndFilename(documentFile, VehicleImagesContainer)))
                {
                    model.FrontSideImage.CopyTo(fsF);
                    fsF.Flush();
                }
                string documentPath = VehicleImagesContainer + documentFile;

                var _documentdetails = new DriverDocuments()
                {
                    DocTypeId = (int)DocTypes.VehicleFrontsideImage,
                    DriverId = model.DriverId,
                    DocImgPath = documentPath
                };
                var resultFront = _vehicleService.AddVehicleDocuments(_documentdetails);

                //add vehicle back side image
                var documentFileBack = ContentDispositionHeaderValue.Parse(model.BackSideImage.ContentDisposition).FileName.Trim('"');
                documentFileBack = CommonFunctions.EnsureCorrectFilename(documentFileBack);
                documentFileBack = CommonFunctions.RenameFileName(documentFileBack);
                using (FileStream fsB = System.IO.File.Create(GetPathAndFilename(documentFileBack, VehicleImagesContainer)))
                {
                    model.BackSideImage.CopyTo(fsB);
                    fsB.Flush();
                }
                string documentPathBack = VehicleImagesContainer + documentFileBack;

                var _documentdetailsback = new DriverDocuments()
                {
                    DocTypeId = (int)DocTypes.VehicleBacksideImage,
                    DriverId = model.DriverId,
                    DocImgPath = documentPathBack
                };
                var resultBack = _vehicleService.AddVehicleDocuments(_documentdetailsback);

                var _vehicleDetails = new VehicleDetails()
                {
                    VehicleTypeId = model.VehicleTypeId,
                    DriverId = model.DriverId,
                    BrandId = model.BrandId,
                    VehicleColorId = model.VehicleColorId,
                    RegisterationNumber = model.RegisterationNumber
                };
                var result = _vehicleService.AddVehicleDetails(_vehicleDetails);
                if (result && resultFront && resultBack)
                {
                    if (documentPath != string.Empty && documentPathBack != string.Empty)
                    {
                        _status = _driverService.SetDriverApplicationStatus(model.DriverId, (int)ApplicationStatus.AwaitingVerification).Result;
                    }
                    else
                    {
                        _status = _driverService.SetDriverApplicationStatus(model.DriverId, (int)ApplicationStatus.Incomplete).Result;
                    }
                    return Ok(new { status = true, data = new { }, message = ResponseMessages.msgAdditionSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Vehicles/AddVehicleDetails : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Update Vehicle Details
        [Authorize]
        [HttpPost("UpdateVehicleDetails")]
        public async Task<IActionResult> UpdateVehicleDetails([FromForm] VehicleDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Vehicles/UpdateVehicleDetails : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                int _status = 0;
                VehicleDetailsUpdateResponseViewModel _vehRes = new VehicleDetailsUpdateResponseViewModel();
                var documentFile = ContentDispositionHeaderValue.Parse(model.FrontSideImage.ContentDisposition).FileName.Trim('"');
                documentFile = CommonFunctions.EnsureCorrectFilename(documentFile);
                documentFile = CommonFunctions.RenameFileName(documentFile);
                using (FileStream fsF = System.IO.File.Create(GetPathAndFilename(documentFile, VehicleImagesContainer)))
                {
                    model.FrontSideImage.CopyTo(fsF);
                    fsF.Flush();
                }
                string documentPath = VehicleImagesContainer + documentFile;

                var _documentdetails = new DriverDocuments()
                {
                    DocTypeId = (int)DocTypes.VehicleFrontsideImage,
                    DriverId = model.DriverId,
                    DocImgPath = documentPath
                };
                var resultFront = _vehicleService.UpdateVehicleDocuments(_documentdetails);

                var documentFileBack = ContentDispositionHeaderValue.Parse(model.BackSideImage.ContentDisposition).FileName.Trim('"');
                documentFileBack = CommonFunctions.EnsureCorrectFilename(documentFileBack);
                documentFileBack = CommonFunctions.RenameFileName(documentFileBack);
                using (FileStream fsB = System.IO.File.Create(GetPathAndFilename(documentFileBack, VehicleImagesContainer)))
                {
                    model.BackSideImage.CopyTo(fsB);
                    fsB.Flush();
                }
                string documentPathBack = VehicleImagesContainer + documentFileBack;

                var _documentdetailsback = new DriverDocuments()
                {
                    DocTypeId = (int)DocTypes.VehicleBacksideImage,
                    DriverId = model.DriverId,
                    DocImgPath = documentPathBack
                };
                var resultBack = _vehicleService.UpdateVehicleDocuments(_documentdetailsback);

                var _vehicleDetails = new VehicleDetails()
                {
                    VehicleTypeId = model.VehicleTypeId,
                    DriverId = model.DriverId,
                    BrandId = model.BrandId,
                    VehicleColorId = model.VehicleColorId,
                    RegisterationNumber = model.RegisterationNumber
                };
                var result = _vehicleService.UpdateVehicleDetails(_vehicleDetails);
                var _res = result + resultFront + resultBack;
                _vehRes.FrontSideImgPath = resultFront > 0 ? documentPath : string.Empty;
                _vehRes.BackSideImgPath = resultBack > 0 ? documentPathBack : string.Empty;
                if (result > 0 && resultFront > 0 && resultBack > 0)
                {
                    _status = await _driverService.SetDriverApplicationStatus(model.DriverId, (int)ApplicationStatus.Verified);
                    _vehRes.ApplicationStatus = (int)ApplicationStatus.Verified;
                }
                else
                {
                    _status = await _driverService.SetDriverApplicationStatus(model.DriverId, (int)ApplicationStatus.Incomplete);
                    _vehRes.ApplicationStatus = (int)ApplicationStatus.Incomplete;
                }
                if (_res == 3)
                {
                    return Ok(new { status = true, data = _vehRes, message = "Vehicle details" + ResponseMessages.msgUpdationSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Vehicles/UpdateVehicleDetails : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        //#region Add Vehicle Documents
        //[HttpPost("AddVehicleDocuments")]
        //public IActionResult AddVehicleDocuments([FromForm] DriverVehicleDocumentsViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
        //    }
        //    try
        //    {
        //        var documentFile = ContentDispositionHeaderValue.Parse(model.PersonalId.ContentDisposition).FileName.Trim('"');
        //        documentFile = CommonFunctions.EnsureCorrectFilename(documentFile);
        //        documentFile = CommonFunctions.RenameFileName(documentFile);
        //        using (FileStream fs = System.IO.File.Create(GetPathAndFilename(documentFile, ProfilePicContainer)))
        //        {
        //            model.PersonalId.CopyTo(fs);
        //            fs.Flush();
        //        }
        //        string documentPath = ProfilePicContainer + documentFile;

        //        var _documentdetails = new DriverDocuments()
        //        {
        //            DocTypeId = model.DocTypeId,
        //            DriverId = model.DriverId,
        //            DocImgPath = documentPath
        //        };
        //        var result = _vehicleService.AddVehicleDocuments(_documentdetails);
        //        if (result)
        //        {
        //            return Ok(new { status = true, data = new { }, message = ResponseMessages.msgAdditionSuccess, code = StatusCodes.Status200OK });
        //        }
        //        else
        //        {
        //            return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
        //    }
        //}
        //#endregion

        #region Get Vehicles Details
        [Authorize]
        [HttpPost("GetVehicleDetails")]
        public async Task<IActionResult> GetVehicleDetails(string vehicleId)
        {
            if (string.IsNullOrEmpty(vehicleId))
            {
                _logger.LogInfo("API: Vehicles/GetVehicleDetails : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var result = await _vehicleService.GetVehicleDetails(vehicleId);
                if (result != null)
                {
                    return Ok(new { status = true, data = result, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Vehicles/GetVehicleDetails : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Vehicles/GetVehicleDetails : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Private Member
        private string GetPathAndFilename(string filename, string foldername)
        {
            string path = _hostingEnvironment.WebRootPath + "//" + foldername + "//";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path + filename;
        }
        #endregion
    }
}
