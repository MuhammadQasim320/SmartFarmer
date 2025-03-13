using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using SmartFarmer.API.Extension;
using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.Service;
using SmartFarmer.Core.ViewModel;
using static SmartFarmer.Core.Common.Enums;
using static SmartFarmer.Core.ViewModel.SearchEquipmentHistoryRequestViewModel;

namespace SmartFarmer.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : Controller
    {
        private static Logger _logger;
        private readonly IEquipmentService _equipmentService;
        private readonly IUserService _userService;
        private readonly IMasterService _masterService;
        private readonly IMachineService _machineService;
        public EquipmentController(IEquipmentService equipmentService, IUserService userService, IMasterService masterService, IMachineService machineService)
        {
            _equipmentService = equipmentService;
            _userService = userService;
            _masterService = masterService;
            _machineService = machineService;
        }

        /// <summary>
        ///get Equipment list by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-equipment-list-by-search-with-pagination")]
        public IActionResult GetEquipmentListBySearchWithPagination(SearchEquipmentRequestViewModel model)
        {
            if (model.PageSize <= 0) model.PageSize = 1;
            if (model.PageNumber <= 0) model.PageNumber = 1;
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            //var loginUserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _equipmentService.GetEquipmentListBySearchWithPagination(model, UserMasterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get equipment history by machineId
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-equipment-history-list-with-pagination/{machineId}")]
        public IActionResult GetEquipmentHistory(Guid machineId, SearchEquipmentHistoryRequestViewModel model)
        {
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            if (model.PageSize <= 0) model.PageSize = 1;
            if (model.PageNumber <= 0) model.PageNumber = 1;
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (model.EventTypeId != null)
            {
                if (!_masterService.IsEventTypeExist(model.EventTypeId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.EventTypeExistanceError + model.EventTypeId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.EventTypeExistanceError, data = model.EventTypeId });
                }
            }
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _equipmentService.GetEquipmentHistory(machineId, model, UserMasterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get equipment precheck history by machineId
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-equipment-precheck-history-list-by-search-with-pagination")]
        public IActionResult GetEquipmentPreCheckHistory(SearchEquipmentPreCheckHistoryRequestViewModel model)
        {

            //if (model.PageSize <= 0) model.PageSize = 1;
            //if (model.PageNumber <= 0) model.PageNumber = 1;
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (model.MachineId != null)
            {
                if (!_machineService.IsMachineExist(model.MachineId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineExistanceError + model.MachineId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineExistanceError, data = model.MachineId });
                }
            }
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _equipmentService.GetEquipmentPreCheckHistory(model, UserMasterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }
    }
}
