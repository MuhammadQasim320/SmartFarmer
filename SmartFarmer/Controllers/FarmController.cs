using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using SmartFarmer.API.Extension;
using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.Service;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Data.Context;
using SmartFarmer.Domain.Model;
using static SmartFarmer.Core.Common.Enums;

namespace SmartFarmer.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FarmController : Controller
    {
        private static Logger _logger;
        private readonly IFarmService _farmService;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMasterService _masterService;
        private readonly IMachineService _machineService;
        private SmartFarmerContext _dbContext;

        public FarmController(IFarmService farmService, IUserService userService, UserManager<ApplicationUser> userManager, IMasterService masterService, IMachineService machineService,SmartFarmerContext dbContext)
        {
            _farmService = farmService;
            _userService = userService;
            _userManager = userManager;
            _masterService = masterService;
            _machineService = machineService;
            _dbContext= dbContext;
        }

        /// <summary>
        ///create farm into system 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [Route("create-farm")]
        public async Task<IActionResult> CreateFarm(FarmRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var CreatedBy = User.GetUserId();

            var userExists = _userService.CheckUserEmailExistence(model.Email);
            if (userExists != false)
            {
                NLog.LogManager.GetLogger("").Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response);
                return BadRequest(new ResponseViewModel { Status = "Error", Message = "Email already exists!" });
            }

            ApplicationUser user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ApplicationUserTypeId = (int)Core.Common.Enums.ApplicationUserTypeEnum.MasterAdmin,
                ApplicationUserStatusId = (int)Core.Common.Enums.ApplicationUserStatusEnum.Live,
                CreatedDate = DateTime.Now,
                OperatorStatusId = null,
                CreatedBy = CreatedBy,
            };
            user.MasterAdminId = user.Id;
            user.MainAdminId = user.Id;
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                NLog.LogManager.GetLogger("").Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + false);
                return BadRequest( new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.AccountCreationFail });
            }
            await _userManager.AddToRoleAsync(user, Enums.ApplicationUserTypeEnum.MasterAdmin.ToString());
            FarmViewModel farmViewModel = new FarmViewModel() { FarmName = model.FarmName, CreatedBy = CreatedBy, MasterAdminId = user.Id};

            var response = _farmService.AddFarm(farmViewModel);

              if (response != null)
              {
                    List<HazardKey> hazardKeys = new()
                    {
                        new HazardKey { HazardKeyId = Guid.NewGuid(), Name = "Bog/Marsh Gound", Color = "#4CAF50", CreatedDate = DateTime.Now, HazardTypeId = 2, CreatedBy = user.Id },
                        new HazardKey { HazardKeyId = Guid.NewGuid(), Name = "Bridleway/Path", Color = "#800080", CreatedDate = DateTime.Now, HazardTypeId = 2, CreatedBy = user.Id },
                        new HazardKey { HazardKeyId = Guid.NewGuid(), Name = "Cliff Edge", Color = "#FFFACD", CreatedDate = DateTime.Now, HazardTypeId = 2, CreatedBy = user.Id },
                        new HazardKey { HazardKeyId = Guid.NewGuid(), Name = "Ditch - enter only with extreme caution", Color = "#5CC7D5", CreatedDate = DateTime.Now, HazardTypeId = 2, CreatedBy = user.Id },
                        new HazardKey { HazardKeyId = Guid.NewGuid(), Name = "Gateway", Color = "#78D2EB", CreatedDate = DateTime.Now, HazardTypeId = 2, CreatedBy = user.Id },
                        new HazardKey { HazardKeyId = Guid.NewGuid(), Name = "Manhole", Color = "#000080", CreatedDate = DateTime.Now, HazardTypeId = 2, CreatedBy = user.Id },
                        new HazardKey { HazardKeyId = Guid.NewGuid(), Name = "Mast/Pylon", Color = "#4169E1", CreatedDate = DateTime.Now, HazardTypeId = 2, CreatedBy = user.Id },
                        new HazardKey { HazardKeyId = Guid.NewGuid(), Name = "Overhead Power", Color = "#8B0000", CreatedDate = DateTime.Now, HazardTypeId = 2, CreatedBy = user.Id },
                        new HazardKey { HazardKeyId = Guid.NewGuid(), Name = "Pedestrians", Color = "#DD45DF", CreatedDate = DateTime.Now, HazardTypeId = 3, CreatedBy = user.Id },
                        new HazardKey { HazardKeyId = Guid.NewGuid(), Name = "Rock Heads", Color = "#C62828", CreatedDate = DateTime.Now, HazardTypeId = 3, CreatedBy = user.Id },
                        new HazardKey { HazardKeyId = Guid.NewGuid(), Name = "Steep Gradients", Color = "#F57C00", CreatedDate = DateTime.Now, HazardTypeId = 3, CreatedBy = user.Id },
                        new HazardKey { HazardKeyId = Guid.NewGuid(), Name = "Underground", Color = "#8B5A2B", CreatedDate = DateTime.Now, HazardTypeId = 3, CreatedBy = user.Id },
                        new HazardKey { HazardKeyId = Guid.NewGuid(), Name = "Underground", Color = "#4E8275", CreatedDate = DateTime.Now, HazardTypeId = 1, CreatedBy = user.Id },
                        new HazardKey { HazardKeyId = Guid.NewGuid(), Name = "Underground Phone Cable", Color = "#7A7326", CreatedDate = DateTime.Now, HazardTypeId = 2, CreatedBy = user.Id },
                        new HazardKey { HazardKeyId = Guid.NewGuid(), Name = "Woodland", Color = "#FFEB3B", CreatedDate = DateTime.Now, HazardTypeId = 3, CreatedBy = user.Id },
                        new HazardKey { HazardKeyId = Guid.NewGuid(), Name = "Other", Color = "#9E9E9E", CreatedDate = DateTime.Now, HazardTypeId = 3, CreatedBy = user.Id }
                    };

                _dbContext.HazardKeys.AddRange(hazardKeys);
                await _dbContext.SaveChangesAsync();
            }
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
        ///get farm list for app
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize (Roles = "Operator,Both")]
        [HttpGet]
        [Route("get-farms")]
        public IActionResult GetFarmList(string SearchKey)
        {
            var LogInUser = User.GetUserId();
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(SearchKey);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var loginInUserEmail = User.GetUserEmail();
            var response = _farmService.GetFarmListBySearch(LogInUser, SearchKey, loginInUserEmail);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        /// Assign farm
        /// </summary>
        /// <param name="farmId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize (Roles = "Operator,Both")]
        [HttpPost]
        [Route("switch-farm/{farmId}")]
        public async Task<IActionResult> GetFarmDetails(Guid farmId)
        {
            var email = User.GetUserEmail();
            FarmUserRequestViewModel farmuserViewModel = new() { FarmId = farmId, Email = email };
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(farmuserViewModel);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            
            if (!_farmService.IsFarmExist(farmId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FarmExistanceError + farmId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FarmExistanceError, data = farmId });
            }
            if (!_farmService.IsFarmAssignedToUser(farmId, email))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FarmSwitchUnsucessful + email);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FarmSwitchUnsucessful, data = email });
            }
            var userId = User.GetUserId();
            var machines = _machineService.GetOperatorActiveMachineCounts(userId);
            if (machines > 0)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.ActiveOperatingMachinesError + userId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.ActiveOperatingMachinesError, data = userId });
            }
            var response = _farmService.SwitchFarm(farmId, email);
            if (response == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FarmSwitchUnsucessful + email);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FarmSwitchUnsucessful, data = email });
            }

            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.FarmSwitchSucessfully, data = response });
        }

        ///// <summary>
        /////update Farm details
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        ///// <response code="400">If the item is null</response>
        //[HttpPut]
        //[Route("update-farm-detail/{farmId}")]
        //public IActionResult UpdateFarmDetail(Guid farmId, FarmRequestViewModel model)
        //{
        //    FarmResponseViewModel farmViewModel = new() { FarmId = farmId, FarmName = model.FarmName,  = model.Location, Area = model.Area };
        //    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + farmId);
        //    if (!_farmService.IsFarmExist(farmId))
        //    {
        //        NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FarmExistanceError + farmId);
        //        return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FarmExistanceError, data = farmId });
        //    }
        //    var response = _farmService.UpdateFarmDetails(farmViewModel);
        //    var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
        //    return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = response });
        //}

        /// <summary>
        /// add / update alarm action into system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPut]
        [Route("add-update-alarm-action")]
        public async Task<IActionResult> AddAlarmAction(AddAlarmActionRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var CreatedBy = User.GetUserId();

            if (model.AlarmActionId != null)
            {
                if (!_farmService.IsAlarmActionExist(model.AlarmActionId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.AlarmActionExistenceError + model.AlarmActionId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.AlarmActionExistenceError, data = model.AlarmActionId });
                }
            }

            if (!_masterService.IsMobileActionTypeExist(model.MobileActionTypeId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MobileActionTypeExistenceError + model.MobileActionTypeId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MobileActionTypeExistenceError, data = model.MobileActionTypeId });
            }
            var smsNumbersJson = Newtonsoft.Json.JsonConvert.SerializeObject(model.SmsNumbers);

            var response = _farmService.AddAlarmAction(CreatedBy, model, smsNumbersJson);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
        }




        /// <summary>
        ///get alarm Action detail
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-alarm-action-details")]
        public IActionResult GetAlarmActionDetails()
        {
            string loginUserId = User.GetUserId();
            var masterAdminId = _userService.GetMasterAdminId(loginUserId);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(masterAdminId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            
            var response = _farmService.GetAlarmActionDetails(masterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }



        /// <summary>
        ///get farm list by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [Route("get-farm-list-by-search-with-pagination")]
        public IActionResult GetFarmListBySearchWithPagination(FarmSearchRequestViewModel model)
        {
            if (model.PageSize <= 0) model.PageSize = 1;
            if (model.PageNumber <= 0) model.PageNumber = 1;
            string loginUserId = User.GetUserId();
            //var masterAdminId = _userService.GetMasterAdminId(loginUserId);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var response = _farmService.GetFarmListBySearchWithPagination(model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }


        /// <summary>
        ///get farm details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        [Route("get-farm-details/{farmId}")]
        public IActionResult GetFarmDetail(Guid farmId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(farmId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (!_farmService.IsFarmExist(farmId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FarmExistanceError + farmId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FarmExistanceError, data = farmId });
            }
            var response = _farmService.GetFarmDetail(farmId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }



        /// <summary>
        ///update farm details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut]
        [Route("update-farm-detail/{farmId}")]
        public IActionResult UpdateFarmDetail(Guid farmId, UpdateFarmRequestViewModel model)
        {
            FarmDetailViewModel farmViewModel = new() { FarmId = farmId, FarmName = model.FarmName, MasterAdminFirstName = model.MasterAdminFirstName, MasterAdminLastName = model.MasterAdminLastName };
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + farmId);
            if (!_farmService.IsFarmExist(farmId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FarmExistanceError + farmId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FarmExistanceError, data = farmId });
            }
            var response = _farmService.UpdateFarmDetail(farmViewModel);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = response });
        }


        /// <summary>
        ///get all farm list
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize(Roles = "Unicorn")]
        [HttpGet]
        [Route("get-all-farm-list")]
        public IActionResult GetAllFarmList()
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(null);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            //var LogInUser = User.GetUserId();
            //var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _farmService.GetAllFarmList();
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }


        /// <summary>
        /// Assign farm
        /// </summary>
        /// <param name="farmId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize(Roles = "Unicorn")]
        [HttpPost]
        [Route("access-farm/{farmId}")]
        public async Task<IActionResult> AccessFarm(Guid farmId)
        {
            var email = User.GetUserEmail();
            AccessFarmUserRequestViewModel farmuserViewModel = new() { FarmId = farmId, Email = email };
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(farmuserViewModel);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);

            if (!_farmService.IsFarmExist(farmId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FarmExistanceError + farmId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FarmExistanceError, data = farmId });
            }
            if (!_farmService.IsFarmAssigned(farmId, email))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FarmSwitchUnsucessful + email);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FarmSwitchUnsucessful, data = email });
            }
            //var userId = User.GetUserId();
            //var machines = _machineService.GetOperatorActiveMachineCounts(userId);
            //if (machines > 0)
            //{
            //    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.ActiveOperatingMachinesError + userId);
            //    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.ActiveOperatingMachinesError, data = userId });
            //}
            var response = _farmService.AccessFarm(farmId, email);
            if (response == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FarmSwitchUnsucessful + email);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FarmSwitchUnsucessful, data = email });
            }

            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.FarmSwitchSucessfully, data = response });
        }
    }
}
