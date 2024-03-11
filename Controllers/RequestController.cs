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
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static _AbsoPickUp.Common.GlobalVariables;

namespace _AbsoPickUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ITwilioManager _twilioManager;
        private readonly ILoggerManager _logger;
        public RequestController(ILoggerManager logger, IRequestService requestService, IWebHostEnvironment hostingEnvironment, ITwilioManager twilioManager)
        {
            _requestService = requestService;
            _hostingEnvironment = hostingEnvironment;
            _twilioManager = twilioManager;
            _logger = logger;
        }

        #region Delivery Request
        [Authorize]
        [HttpPost("DeliveryRequest")]
        public async Task<IActionResult> DeliveryRequest(DeliveryRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Request/DeliveryRequest : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var _senderId = CommonFunctions.getUserId(User);

                var _request = new DeliveryRequest
                {
                    SenderId = _senderId,
                    SenderPlaceId = model.SenderPlaceId,
                    SenderLat = model.SenderLat,
                    SenderLong = model.SenderLong,
                    SenderAddress = model.SenderAddress,
                    ReceiverName = model.ReceiverName,
                    ReceiverEmail = model.ReceiverEmail,
                    DialCode = model.DialCode,
                    ReceiverMobileNumber = model.ReceiverMobileNumber,
                    ReceiverLat = model.ReceiverLat,
                    ReceiverLong = model.ReceiverLong,
                    ReceiverPlaceId = model.ReceiverPlaceId,
                    ReceiverAddress = model.ReceiverAddress,
                    DeliveryTypeId = model.DeliveryTypeId,
                    TotalDeliveryTime = model.TotalDeliveryTime,
                    TotalDeliveryDistance = model.TotalDeliveryDistance,
                    CreatedDateTime = DateTime.UtcNow
                };

                var result = await _requestService.AddDeliveryRequest(_request);
                var _result = 0;

                if (result != null)
                {
                    var delDeets = new DeliveryDetails
                    {
                        RequestId = result.RequestId,
                        StatusId = (int)ParcelStatus.Unassigned,
                        DeliveryDateTime = DateTime.UtcNow
                    };
                    _result = await _requestService.AddDeliveryDetails(delDeets);
                }

                if (result != null && _result == 1)
                {
                    return Ok(new DeliveryResponse { status = true, data = result, message = "Request " + ResponseMessages.msgAdditionSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Request/DeliveryRequest : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Request/DeliveryRequest : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize]
        [HttpPost("RequestParcelDelivery")]
        public async Task<IActionResult> RequestParcelDelivery([FromForm] DeliverRequestWithParcelViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Request/RequestParcelDelivery : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new UserDeliveryResponse { status = false, Request_data = new DeliveryRequest { }, Parcel_data = new ParcelDetails { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var _res = await DeliveryRequest(model.DeliveryRequest);
                var val = ((OkObjectResult)_res).Value as DeliveryResponse;
                var _reqId = val.@data.RequestId;
                if (_reqId > 0)
                {
                    model.Parcel.RequestId = _reqId;
                    var res = ((OkObjectResult)(ParcelDetails(model.Parcel).Result)).Value as ParcelDetailsResponse;
                    if (res != null)
                    {
                        return Ok(new UserDeliveryResponse { status = true, Request_data = val.@data, message = "Request" + ResponseMessages.msgAdditionSuccess, code = StatusCodes.Status200OK });
                    }
                    else if (_res != null)
                    {
                        return Ok(new UserDeliveryResponse { status = false, Parcel_data = res.@data, message = "Parcel" + ResponseMessages.msgAdditionSuccess, code = StatusCodes.Status200OK });
                    }
                    else
                    {
                        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                    }
                }
                else
                {
                    _logger.LogInfo("API: Request/RequestParcelDelivery : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Request/RequestParcelDelivery : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize]
        [HttpPost("RequestDeliveryByUser")]
        public async Task<IActionResult> RequestDeliveryByUser([FromForm] DeliveryRequestUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Request/RequestDeliveryByUser : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                ParcelDetailsViewModel ParcelModel = new ParcelDetailsViewModel
                {
                    ParcelName = model.ParcelName,
                    ParcelNotes = model.ParcelNotes,
                    ImgBeforePacking = model.ImgBeforePacking,
                    ImgAfterPacking = model.ImgAfterPacking
                };

                DeliveryRequestViewModel details = new DeliveryRequestViewModel
                {
                    SenderPlaceId = model.SenderPlaceId,
                    SenderLat = model.SenderLat,
                    SenderLong = model.SenderLong,
                    SenderAddress = model.SenderAddress,
                    ReceiverName = model.ReceiverName,
                    ReceiverEmail = model.ReceiverEmail,
                    DialCode = model.DialCode,
                    ReceiverMobileNumber = model.ReceiverMobileNumber,
                    ReceiverLat = model.ReceiverLat,
                    ReceiverLong = model.ReceiverLong,
                    ReceiverPlaceId = model.ReceiverPlaceId,
                    ReceiverAddress = model.ReceiverAddress,
                    DeliveryTypeId = model.DeliveryTypeId,
                    TotalDeliveryTime = model.TotalDeliveryTime,
                    TotalDeliveryDistance = model.TotalDeliveryDistance
                };

                DeliverRequestWithParcelViewModel dModel = new DeliverRequestWithParcelViewModel
                {
                    DeliveryRequest = details,
                    Parcel = ParcelModel
                };

                var _res = await RequestParcelDelivery(dModel);

                if (((OkObjectResult)_res).Value is UserDeliveryResponse val)
                {
                    return Ok(new { val.status, data = val.Request_data, val.message, val.code });
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgRequestStatusNotValid, code = StatusCodes.Status206PartialContent });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Request/RequestDeliveryByUser : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Delivery Request for Bakkie Book Later
        [Authorize]
        [HttpPost("DeliveryRequestLater")]
        public async Task<IActionResult> DeliveryRequestLater(DeliveryRequestLaterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Request/DeliveryRequestLater : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                if (model.request.DeliveryTypeId == (int)ParcelType.Bakkie)
                {
                    var details = new DeliveryRequest
                    {
                        SenderId = CommonFunctions.getUserId(User),
                        SenderLat = model.request.SenderLat,
                        SenderLong = model.request.SenderLong,
                        SenderPlaceId = model.request.SenderPlaceId,
                        ReceiverName = model.request.ReceiverName,
                        ReceiverEmail = model.request.ReceiverEmail,
                        DialCode = model.request.DialCode,
                        ReceiverMobileNumber = model.request.ReceiverMobileNumber,
                        ReceiverLat = model.request.ReceiverLat,
                        ReceiverLong = model.request.ReceiverLong,
                        ReceiverPlaceId = model.request.ReceiverPlaceId,
                        DeliveryTypeId = model.request.DeliveryTypeId,
                        TotalDeliveryTime = model.request.TotalDeliveryTime,
                        TotalDeliveryDistance = model.request.TotalDeliveryDistance,
                        CreatedDateTime = DateTime.UtcNow,
                        RequestDateTime = model.RequestDateTime
                    };
                    var result = await _requestService.AddDeliveryRequest(details);
                    var _result = 0;
                    if (result != null)
                    {
                        var delDeets = new DeliveryDetails
                        {
                            RequestId = result.RequestId,
                            StatusId = (int)ParcelStatus.Unassigned,
                            DeliveryDateTime = DateTime.UtcNow
                        };
                        _result = await _requestService.AddDeliveryDetails(delDeets);
                    }

                    if (result != null && _result == 1)
                    {
                        return Ok(new { status = true, data = result, message = "Parcel Request " + ResponseMessages.msgAdditionSuccess, code = StatusCodes.Status200OK });
                    }
                    else
                    {
                        _logger.LogInfo("API: Request/DeliveryRequestLater : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                    }
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = "Enter valid Parcel Type.", code = StatusCodes.Status204NoContent });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Request/DeliveryRequestLater : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Delivery Details
        [Authorize]
        [HttpPost("DeliveryDetails")]
        public async Task<IActionResult> DeliveryDetails(UpdateParcelStatusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Request/DeliveryDetails : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                //add delivery details when status updated
                var requestDetail = new DeliveryDetails
                {
                    RequestId = model.RequestId,
                    StatusId = model.StatusId,
                    DeliveryDateTime = DateTime.UtcNow
                };

                //add driver details when parcel is not unassigned
                if (model.StatusId != (int)ParcelStatus.Unassigned)
                {
                    requestDetail.DriverId = model.DriverId;
                    requestDetail.DriverLat = model.DriverLat;
                    requestDetail.DriverLong = model.DriverLong;
                }

                var result = await _requestService.AddDeliveryDetails(requestDetail);
                var _sent = await _requestService.SendSaveUserNotifications(model.RequestId, model.StatusId);

                var _response = new DeliveryDetailsResponseViewModel
                {
                    ParcelStatusSetTo = ((ParcelStatus)model.StatusId).ToString()
                };

                //SMS for parcel delivery processing.
                if (result == 2 && model.StatusId == (int)(ParcelStatus.DeliveryOnRoute))
                {
                    var receiverPhoneNo = _requestService.GetReceiverPhoneNo(model.RequestId);
                    var receiverAddress = _requestService.GetReceiverAddress(model.RequestId);
                    var _request = _requestService.GetRequestDetailsById(model.RequestId);
                    var elemRES = CommonFunctions.GoogleDistanceMatrixAPILatLon(model.DriverLat.ToString(), model.DriverLong.ToString(), _request.UOVM.DestinationLat, _request.UOVM.DestinationLong);
                    var ETA = elemRES.element.Duration.Text;
                    _response.ParcelETA = ETA;
                    _response.SMSSentWithETA = true;
                    var SMSBody = DriverEnRouteSentSMSTextETA + ETA + DriverEnRouteSentSMSTextAddress + receiverAddress;
                    // todo
                    // await _twilioManager.SendMessage(SMSBody, receiverPhoneNo);
                }

                //SMS for parcel delivery PIN.
                if (result == 1 && model.StatusId == (int)(ParcelStatus.Arrived))
                {
                    var receiverPhoneNo = _requestService.GetReceiverPhoneNo(model.RequestId);
                    var RequestPIN = CommonFunctions.getFourDigitCode();
                    var SMSBody = ReceiverPINSentSMSText + RequestPIN;
                    // todo
                    // await _twilioManager.SendMessage(SMSBody, receiverPhoneNo);
                    _response.PINSent = RequestPIN.ToString();
                    _response.SMSSentWithPIN = true;

                    var _pinDeets = new ParcelDelivery
                    {
                        DeliveryPIN = RequestPIN,
                        DriverId = model.DriverId,
                        ReceiverPhoneNo = receiverPhoneNo,
                        RequestId = model.RequestId,
                        PINSentAt = DateTime.UtcNow,
                        IsVerified = false
                    };

                    var _pinResult = _requestService.AddParcelDelivery(_pinDeets);

                    if (_pinResult != null)
                    {
                        return Ok(new { status = true, data = _response, message = "Delivery Details " + ResponseMessages.msgPINSentSuccess, code = StatusCodes.Status200OK });
                    }
                }

                if (result == 1)
                {
                    return Ok(new { status = true, data = _response, message = "Delivery Details " + ResponseMessages.msgAdditionSuccess, code = StatusCodes.Status200OK });
                }
                else if (result == 2)
                {
                    return Ok(new { status = true, data = _response, message = "Delivery Details " + ResponseMessages.msgAdditionSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Request/DeliveryDetails : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new DeliveryDetailsResponseViewModel(), message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Request/DeliveryDetails : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Parcel Details
        [Authorize]
        [HttpPost("ParcelDetails")]
        public async Task<IActionResult> ParcelDetails([FromForm] ParcelDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Request/ParcelDetails : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                //add parcel image before packing
                var documentBeforeFileOne = ContentDispositionHeaderValue.Parse(model.ImgBeforePacking.ContentDisposition).FileName.Trim('"');
                documentBeforeFileOne = CommonFunctions.EnsureCorrectFilename(documentBeforeFileOne);
                documentBeforeFileOne = CommonFunctions.RenameFileName(documentBeforeFileOne);
                using (FileStream fsb = System.IO.File.Create(GetPathAndFilename(documentBeforeFileOne, ParcelContainer)))
                {
                    model.ImgBeforePacking.CopyTo(fsb);
                    fsb.Flush();
                }
                string documentBeforeFileOnePath = ParcelContainer + documentBeforeFileOne;

                //add parcel image after packing
                var documentAfterFileOne = ContentDispositionHeaderValue.Parse(model.ImgAfterPacking.ContentDisposition).FileName.Trim('"');
                documentAfterFileOne = CommonFunctions.EnsureCorrectFilename(documentAfterFileOne);
                documentAfterFileOne = CommonFunctions.RenameFileName(documentAfterFileOne);
                using (FileStream fsa = System.IO.File.Create(GetPathAndFilename(documentAfterFileOne, ParcelContainer)))
                {
                    model.ImgAfterPacking.CopyTo(fsa);
                    fsa.Flush();
                }
                string documentAfterFileOnePath = ParcelContainer + documentAfterFileOne;

                var details = new ParcelDetails()
                {
                    RequestId = model.RequestId,
                    ParcelName = model.ParcelName,
                    ParcelNotes = model.ParcelNotes,
                    ImgBeforePacking = documentBeforeFileOnePath,
                    ImgAfterPacking = documentAfterFileOnePath,
                };
                var result = await _requestService.AddParcelDetails(details);
                if (result != null)
                {
                    return Ok(new ParcelDetailsResponse { status = true, data = result, message = "Parcel Details " + ResponseMessages.msgAdditionSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status204NoContent });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Request/ParcelDetails : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Distance Matrix
        [Authorize]
        [HttpGet("RequestDistance")]
        public async Task<IActionResult> RequestDistance(string source, string destination, int deliveryTypeId)
        {
            try
            {
                if (source == string.Empty || source == null || destination == string.Empty || destination == null || deliveryTypeId <= 0)
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
                }
                else
                {
                    var result = await _requestService.DistanceMatrixRequest(source, destination, deliveryTypeId);
                    if (result != null)
                    {
                        return Ok(new { status = true, data = result, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                    }
                    else
                    {
                        _logger.LogInfo("API: Request/RequestDistance : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Request/RequestDistance : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize]
        [HttpGet("RequestDistanceLatLon")]
        public async Task<IActionResult> RequestDistanceLatLon(string oLat, string oLon, string dLat, string dLon, int deliveryTypeId)
        {
            try
            {
                if (oLat == string.Empty || oLon == string.Empty || dLat == string.Empty || dLon == string.Empty || deliveryTypeId <= 0)
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
                }
                else
                {
                    var result = await _requestService.DistanceMatrixRequestLatLon(oLat, oLon, dLat, dLon, deliveryTypeId);
                    if (result != null)
                    {
                        return Ok(new { status = true, data = result, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                    }
                    else
                    {
                        _logger.LogInfo("API: Request/RequestDistanceLatLon : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Request/RequestDistanceLatLon : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Get Price
        [Authorize]
        [HttpGet("GetTypePriceDetails")]
        public async Task<IActionResult> GetTypePriceDetails()
        {
            try
            {
                var result = await _requestService.GetPrice();
                if (result != null)
                {
                    return Ok(new { status = true, data = result, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Request/GetTypePriceDetails : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Request/GetTypePriceDetails : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Get Request Details
        [Authorize]
        [HttpGet("GetRequestDetails")]
        public IActionResult GetRequestDetails(int requestId)
        {
            if (requestId <= 0)
            {
                _logger.LogInfo("API: Request/GetRequestDetails : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var result = _requestService.GetRequestDetailsById(requestId);
                if (result != null)
                {
                    return Ok(new { status = true, data = result, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Request/GetRequestDetails : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Request/GetRequestDetails : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region "private Methods"
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
