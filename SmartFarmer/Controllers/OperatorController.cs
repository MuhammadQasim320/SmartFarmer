using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using SmartFarmer.API.Extension;
using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.Service;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Model;
using static SmartFarmer.Core.Common.Enums;
using static SmartFarmer.Core.ViewModel.OperatorViewModel;

namespace SmartFarmer.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OperatorController : ControllerBase
    {
        private static Logger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly IOperatorService _operatorService;
        private readonly IFileService _fileService;
        private readonly IUserGroupService _userGroupService;
        private readonly IMasterService _masterService;
        private readonly IMachineService _machineService;
        private readonly IEventService _eventService;
        private readonly IFarmService _farmService;

        public OperatorController(UserManager<ApplicationUser> userManager, IUserService userService, IOperatorService operatorService, IFileService fileService, IUserGroupService userGroupService, IMasterService masterService, IMachineService machineService, IEventService eventService, IFarmService farmService)
        {
            _userManager = userManager;
            _userService = userService;
            _operatorService = operatorService;
            _fileService = fileService;
            _userGroupService = userGroupService;
            _masterService = masterService;
            _machineService = machineService;
            _eventService = eventService;
            _farmService = farmService;
        }

        ///// <summary>
        ///// add operator into the system
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        ///// <response code="400">If the item is null</response>
        //[HttpPost]
        //[Route("add-operator")]
        //public async Task<IActionResult> AddOperatorUser([FromForm] OperatorUserViewModel model)
        //{
        //    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
        //    NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);

        //    string userId = User.GetUserId();
        //    var loginUser = _userManager.Users.Where(a => a.Id == userId).FirstOrDefault();

        //    var userExists = _userService.CheckUserEmailExistence(model.Email);
        //    if (userExists != false)
        //    {
        //        NLog.LogManager.GetLogger("").Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response);
        //        return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel { Status = "Error", Message = "Email already exists!" });
        //    }
        //    if (model.UserGroupId != null)
        //    {
        //        if (_userGroupService.IsUserGroupExist(model.UserGroupId.Value) == false)
        //        {
        //            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + model.UserGroupId);
        //            return Ok(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserGroupIdIncorrectError, data = model.UserGroupId });
        //        }
        //    }
        //    if (model.OperatorStatusId != null)
        //    {
        //        if (_operatorService.IsOperatorStatusExist(model.OperatorStatusId.Value) == false)
        //        {
        //            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + model.OperatorStatusId);
        //            return Ok(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.OperatorStatusIdIncorrectError, data = model.OperatorStatusId });
        //        }
        //    }
        //    FileViewModel fileResponse = new FileViewModel();
        //    if(model.ProfileImage != null)
        //    {
        //        fileResponse = _fileService.UploadFile(model.ProfileImage);
        //        if (fileResponse == null)
        //        {
        //            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(fileResponse);
        //            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FileUploadUnSuccessfull + jsonResponse);
        //            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FileUploadUnSuccessfull, data = fileResponse });
        //        }
        //    }
        //    var createdBy = User.GetUserId();
        //    ApplicationUser user = new ApplicationUser()
        //    {
        //        SecurityStamp = Guid.NewGuid().ToString(),
        //        UserName = model.Email,
        //        Email = model.Email,
        //        ApplicationUserTypeId = (int)Core.Common.Enums.ApplicationUserTypeEnum.Operator,
        //        ApplicationUserStatusId = (int)Core.Common.Enums.ApplicationUserStatusEnum.Live,
        //        FirstName = model.FirstName,
        //        LastName = model.LastName,
        //        ProfileImageLink = fileResponse.FileLink,
        //        ProfileImageName = fileResponse.Name,
        //        MobileHouseNameNumber = model.MobileHouseNameNumber,
        //        Street = model.Street,
        //        Addressline2 = model.Addressline2,
        //        Town = model.Town,
        //        County = model.County,
        //        PostCode = model.PostCode,
        //        CreatedDate = DateTime.Now,
        //        UserGroupId = model.UserGroupId,
        //        OperatorStatusId = model.OperatorStatusId,
        //        //CreatedBy = createdBy
        //    };
        //    if (loginUser.ApplicationUserTypeId == 2)
        //    {
        //        user.CreatedBy = createdBy;
        //        user.MasterAdminId = loginUser.Id;
        //    }
        //    if (loginUser.ApplicationUserTypeId != 2 && loginUser.ApplicationUserTypeId != 1)
        //    {
        //        user.CreatedBy = createdBy;
        //        user.MasterAdminId = loginUser.MasterAdminId;
        //    }
        //    var result = await _userManager.CreateAsync(user, model.Password);
        //    if (result.Succeeded != true)
        //    {
        //        var deletingServerFile = _fileService.DeleteUploadFile(user.ProfileImageName);
        //        NLog.LogManager.GetLogger("").Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + false);
        //        return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.AccountCreationFail });
        //    }
        //    await _userManager.AddToRoleAsync(user, Enums.ApplicationUserTypeEnum.Operator.ToString());

        //    NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response);
        //    return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.OperatorAccountCreationSucess, data = user.Id });
        //}

        /// <summary>
        ///get operator list by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-operator-list-by-search-with-pagination")]
        public IActionResult GetOperatorListBySearchWithPagination(SearchOperatorRequestViewModel model)
        {
            if (model.PageSize <= 0) model.PageSize = 1;
            if (model.PageNumber <= 0) model.PageNumber = 1;
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (model.UserGroupId != null)
            {
                if (!_userGroupService.IsUserGroupExist(model.UserGroupId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + model.UserGroupId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserGroupIdIncorrectError, data = model.UserGroupId });

                }
            }
            if (model.OperatorStatusId != null)
            {
                if (!_operatorService.IsOperatorStatusExist(model.OperatorStatusId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + model.OperatorStatusId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.OperatorStatusIdIncorrectError, data = model.OperatorStatusId });
                }
            }
            var response = _operatorService.GetOperatorListBySearchWithPagination(UserMasterAdminId,model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///add Clock_In into system 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize(Roles = "Operator, Both")]
        [HttpPost]
        [Route("clock-in")]
        public IActionResult AddClockInEvent(EventRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var EventTypeId = (int)Core.Common.Enums.EventTypeEnum.Clock_In;
            var CreatedBy = User.GetUserId();
            //var user = _userManager.FindByIdAsync(CreatedBy).Result; 
            //if (user != null)
            //{
            //    user.Location = model.Location; 
            //    _userManager.UpdateAsync(user).Wait();
            //}
            var response = _eventService.AddEvent(EventTypeId,CreatedBy, model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            if (response != null)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
                return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
            }
            NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RecordAddError, data = response });

        }

        /// <summary>
        ///add Clock_Out into system 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize(Roles = "Operator, Both")]
        [HttpPost]
        [Route("clock-out")]
        public IActionResult AddClockOutEvent(EventRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var EventTypeId = (int)Core.Common.Enums.EventTypeEnum.Clock_Out;
            var CreatedBy = User.GetUserId();
            //var user = _userManager.FindByIdAsync(CreatedBy).Result;
            //if (user != null)
            //{
            //    user.Location = model.Location;
            //    _userManager.UpdateAsync(user).Wait();
            //}
            var response = _eventService.AddEvent(EventTypeId, CreatedBy, model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            if (response != null)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
                return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
            }
            NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RecordAddError, data = response });

        }

        /// <summary>
        ///set fall detection for app
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize(Roles = "Operator, Both")]
        [HttpPost]
        [Route("set-fall-detection")]
        public IActionResult SetFallDetection(EventRequestViewModel model, bool fallDetectionTriggered)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var CreatedBy = User.GetUserId();
            //var user = _userManager.FindByIdAsync(CreatedBy).Result;
            //if (user != null)
            //{
            //    user.Location = model.Location;
            //    _userManager.UpdateAsync(user).Wait();
            //}
            var EventTypeId = (int)Core.Common.Enums.EventTypeEnum.Fall;
            var response = _eventService.AddEvent(EventTypeId, CreatedBy, model);
            if (response != null)
            {
                var responseViewModel = new EventFallResponseViewModel
                {
                    EventId = response.EventId,
                    CreatedDate = response.CreatedDate,
                    CreatedBy = response.CreatedBy.ToString(),
                    CreatedByName = User.Identity.Name,
                    Location = model.Location,
                    EventTypeId = response.EventTypeId,
                    EventType = response.EventType,
                    FallDetectionTriggered = fallDetectionTriggered,
                    Message = response.Message,
                    ShowWebPopup=response.ShowWebPopup,

                };
                var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(responseViewModel);
                NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);

                return Ok(new ResponseViewModel
                {
                    Status = ResponseStatusType.Success.ToString(),
                    Message = ResponseMessageConstants.FallDetectionSuccess,
                    data = responseViewModel
                });
            }

            NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonString);
            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FallDetectionError, data = response });

        }


        /// <summary>
        ///set sos for app
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize(Roles = "Operator, Both")]
        [HttpPost]
        [Route("set-sos")]
        public IActionResult SetSOS(EventRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var CreatedBy = User.GetUserId();
            //var user = _userManager.FindByIdAsync(CreatedBy).Result;
            //if (user != null)
            //{
            //    user.Location = model.Location;
            //    _userManager.UpdateAsync(user).Wait();
            //}
            var EventTypeId = (int)Core.Common.Enums.EventTypeEnum.SOS;
            var response = _eventService.AddEvent(EventTypeId, CreatedBy, model);
            if (response != null)
            {
                var responseViewModel = new SOSResponseViewModel
                {
                    EventId = response.EventId,
                    CreatedDate = response.CreatedDate,
                    CreatedBy = response.CreatedBy.ToString(),
                    CreatedByName = User.Identity.Name,
                    Location = model.Location,
                    EventTypeId = response.EventTypeId,
                    EventType = response.EventType,
                    Message = response.Message,
                    ShowWebPopup = response.ShowWebPopup,
                };
                var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(responseViewModel);
                NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);

                return Ok(new ResponseViewModel
                {
                    Status = ResponseStatusType.Success.ToString(),
                    Message = ResponseMessageConstants.SOSSuccess,
                    data = responseViewModel
                });
            }

            NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonString);
            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.SOSError, data = response });

        }






        /// <summary>
        /// check_In for app
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize(Roles = "Operator, Both")]
        [HttpPost]
        [Route("check_in")]
        public IActionResult SetCheck_In(EventRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var CreatedBy = User.GetUserId();
            //var user = _userManager.FindByIdAsync(CreatedBy).Result;
            //if (user != null)
            //{
            //    user.Location = model.Location;
            //    _userManager.UpdateAsync(user).Wait();
            //}
            var EventTypeId = (int)Core.Common.Enums.EventTypeEnum.Check_In;
            var response = _eventService.AddEvent(EventTypeId, CreatedBy, model);
            if (response != null)
            {
                var responseViewModel = new EventResponseViewModel
                {
                    EventId = response.EventId,
                    CreatedDate = response.CreatedDate,
                    CreatedBy = response.CreatedBy.ToString(),
                    CreatedByName = User.Identity.Name,
                    Location = model.Location,
                    EventTypeId = response.EventTypeId,
                    EventType = response.EventType,
                    Message = response.Message
                };
                var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(responseViewModel);
                NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);

                return Ok(new ResponseViewModel
                {
                    Status = ResponseStatusType.Success.ToString(),
                    Message = ResponseMessageConstants.CheckInSuccess,
                    data = responseViewModel
                });
            }

            NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonString);
            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.CheckInError, data = response });

        }



        /// <summary>
        ///get all notification List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize(Roles = "Operator, Both")]
        [HttpGet]
        [Route("get-notification-list")]
        public IActionResult GetAllNotification()
        {
            var userId = User.GetUserId();
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(userId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var response = _operatorService.GetAllNotification(userId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }
    }
}
