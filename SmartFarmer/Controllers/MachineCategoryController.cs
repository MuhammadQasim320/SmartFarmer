using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFarmer.API.Extension;
using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.Service;
using SmartFarmer.Core.ViewModel;
using static SmartFarmer.Core.Common.Enums;

namespace SmartFarmer.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MachineCategoryController : ControllerBase
    {
        private readonly IMachineCategoryService _machineCategoryService;
        private readonly IUserService _userService;

        public MachineCategoryController(IMachineCategoryService machineCategoryService, IUserService userService)
        {
            _machineCategoryService = machineCategoryService;
            _userService = userService;
        }

        /// <summary>
        ///add MachineCategory into system 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("add-machineCategory")]
        public IActionResult AddMachineCategory(MachineCategoryRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var CreatedBy = User.GetUserId();
            var response = _machineCategoryService.AddMachineCategory(CreatedBy, model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            if (response != null)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
                return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
            }
            NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RecordAddError, data = null });

        }

        /// <summary>
        ///get MachineCategory list by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-machineCategory-list-by-search-with-pagination")]
        public IActionResult GetMachineCategoryListBySearchWithPagination(SearchMachineCategoryRequestViewModel model)
        {
            if (model.PageSize <= 0) model.PageSize = 1;
            if (model.PageNumber <= 0) model.PageNumber = 1;
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            //string UserMasterAdminId = User.GetUserMasterAdminId(); 
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _machineCategoryService.GetMachineCategoryListBySearchWithPagination(UserMasterAdminId,model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get MachineCategory details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-machineCategory-details/{machineCategoryId}")]
        public IActionResult GetmachineCategoryDetails(Guid machineCategoryId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(machineCategoryId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (_machineCategoryService.IsMachineCategoryExist(machineCategoryId) == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineCategoryExistanceError + machineCategoryId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineCategoryExistanceError, data = machineCategoryId });
            }
            var response = _machineCategoryService.GetMachineCategoryDetails(machineCategoryId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///update machineCategory details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPut]
        [Route("update-machineCategory-detail/{machineCategoryId}")]
        public IActionResult UpdateMachineCategoryDetail(Guid machineCategoryId, MachineCategoryRequestViewModel model)
        {
            MachineCategoryResponseViewModel machineCategoryViewModel = new()
            {
                MachineCategoryId = machineCategoryId,
                Name = model?.Name,
                Description = model.Description,
            };
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + machineCategoryId);
            if (!_machineCategoryService.IsMachineCategoryExist(machineCategoryId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineCategoryExistanceError + machineCategoryId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineCategoryExistanceError, data = machineCategoryId });
            }
            var response = _machineCategoryService.UpdateMachineCategoryDetails(machineCategoryViewModel);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = response });
        }

        /// <summary>
        ///delete machineCategory
        /// </summary>
        /// <param name="machineCategoryId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpDelete]
        [Route("delete-machineCategory/{machineCategoryId}")]
        public IActionResult DeleteSmartCourse(Guid machineCategoryId)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + machineCategoryId);
            if (!_machineCategoryService.IsMachineCategoryExist(machineCategoryId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineCategoryExistanceError + machineCategoryId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineCategoryExistanceError, data = machineCategoryId });
            }
            if (!_machineCategoryService.IsMachineCategoryAssign(machineCategoryId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineCategoryAssignError + machineCategoryId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineCategoryAssignError, data = machineCategoryId });
            }
            var response = _machineCategoryService.DeleteMachineCategory(machineCategoryId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordDeleteSuccess, data = response });
        }

        /// <summary>
        ///get MachineCategory Name List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-machineCategory-name-list")]
        public IActionResult GetmachineCategoryNameList()
        {
            //var createdBy = User.GetUserId();
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(null);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);

            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _machineCategoryService.GetMachineCategoryNameList(UserMasterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }
    }
}
