using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using SmartFarmer.API.Extension;
using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;
using static SmartFarmer.Core.Common.Enums;

namespace SmartFarmer.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WelfareRoutineController : ControllerBase
    {
        private static Logger _logger;
        private readonly IWelfareRoutineService _welfareRoutineService;
        private readonly IUserGroupService _userGroupService;
        private readonly IUserService _userService;

        public WelfareRoutineController(IWelfareRoutineService welfareRoutineService, IUserGroupService userGroupService, IUserService userService)
        {
            _welfareRoutineService = welfareRoutineService;
            _userGroupService = userGroupService;
            _userService = userService;
        }

        /// <summary>
        ///add welfareRoutine by UserGroupId into system 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("add-welfareRoutine")]
        public IActionResult AddWelfareRoutine(WelfareRoutineRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var CreatedBy = User.GetUserId();
            if(model.UserGroupId != null)
            {
                if (!_userGroupService.IsUserGroupExist(model.UserGroupId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + model.UserGroupId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserGroupIdIncorrectError, data = model.UserGroupId });
                }

                if (_welfareRoutineService.IsGroupAssignedToOtherWelfareRoutine(model.UserGroupId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + model.UserGroupId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.GroupAssignedToWelfareRoutineError, data = model.UserGroupId });
                }
            } 
               
            
            var response = _welfareRoutineService.AddWelfareRoutine(CreatedBy,model);
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
        ///get welfareRoutine list by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-welfareRoutine-list-by-search-with-pagination")]
        public IActionResult GetWelfareRoutineListBySearchWithPagination(SearchWelfareRoutineRequestViewModel model)
        {
            if (model.PageSize <= 0) model.PageSize = 1;
            if (model.PageNumber <= 0) model.PageNumber = 1;
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var response = _welfareRoutineService.GetWelfareRoutineListBySearchWithPagination(UserMasterAdminId,model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get welfareRoutine details
        /// </summary>
        /// <param name="welfareRoutineId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-welfareRoutine-detail/{welfareRoutineId}")]
        public IActionResult GetWelfareRoutineDetail(Guid welfareRoutineId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(welfareRoutineId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (_welfareRoutineService.IsWelfareRoutineExists(welfareRoutineId) == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.WelfareRoutineExistanceError + welfareRoutineId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.WelfareRoutineExistanceError, data = welfareRoutineId });
            }
            var response = _welfareRoutineService.GetWelfareRoutineDetails(welfareRoutineId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///update welfareRoutine details
        /// </summary>
        /// <param name="welfareRoutineId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPut]
        [Route("update-welfareRoutine-detail/{welfareRoutineId}")]
        public IActionResult UpdateWelfareRoutineDetail(Guid welfareRoutineId, WelfareRoutineRequestViewModel model)
        {
            WelfareRoutineResponseViewModel welfareRoutineViewModel = new() { Name = model.Name, UserGroupId = model.UserGroupId, Minutes = model.Minutes, WelfareRoutineId = welfareRoutineId };
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + welfareRoutineId);
            if (!_welfareRoutineService.IsWelfareRoutineExists(welfareRoutineId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.WelfareRoutineExistanceError + welfareRoutineId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.WelfareRoutineExistanceError, data = welfareRoutineId });
            }
            if (model.UserGroupId != null)
            {
                if (!_userGroupService.IsUserGroupExist(model.UserGroupId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + model.UserGroupId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserGroupIdIncorrectError, data = model.UserGroupId });
                }

                if (_welfareRoutineService.IsGroupAssignedToWelfareRoutine(model.UserGroupId.Value,welfareRoutineId))
                {
                    if (_welfareRoutineService.IsGroupAssigned(model.UserGroupId.Value, welfareRoutineId))
                    {

                        NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + model.UserGroupId);
                        return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.GroupAssignedToWelfareRoutineError, data = model.UserGroupId });
                    }
                }

                if (_welfareRoutineService.IsGroupAssigned(model.UserGroupId.Value, welfareRoutineId))
                {

                    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + model.UserGroupId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.GroupAssignedToWelfareRoutineError, data = model.UserGroupId });
                }








            }

            var response = _welfareRoutineService.UpdateWelfareRoutineDetail(welfareRoutineViewModel);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = response });
        }
    }
}
