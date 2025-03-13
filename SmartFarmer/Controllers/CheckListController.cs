using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFarmer.API.Extension;
using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.Service;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Model;
using static SmartFarmer.Core.Common.Enums;

namespace SmartFarmer.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CheckListController : ControllerBase
    {
        private ICheckListService _checkListService;
        private IMachineTypeService _machineTypeService;
        private readonly IMasterService _masterService;
        private readonly IUserService _userService;
        private readonly IMachineService _machineService;

        public CheckListController(ICheckListService checkListService, IMachineTypeService machineTypeService, IMasterService masterService, IUserService userService, IMachineService machineService)
        {
            _checkListService = checkListService;
            _machineTypeService = machineTypeService;
            _masterService = masterService;
            _userService = userService;
            _machineService = machineService;
        }

        /// <summary>
        /// add CheckList into system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("add-checkList")]
        public async Task<IActionResult> AddCheckList(CheckListRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var CreatedBy = User.GetUserId();
            if (!_machineTypeService.IsMachineTypeExist(model.MachineTypeId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineTypeExistenceError + model.MachineTypeId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineTypeExistenceError, data = model.MachineTypeId });
            }
            if (!_masterService.IsCheckTypeExist(model.CheckTypeId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.CheckTypeExistenceError + model.CheckTypeId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.CheckTypeExistenceError, data = model.CheckTypeId });
            }
            if (!_masterService.IsFrequencyTypeExist(model.FrequencyTypeId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FrequencyTypeExistenceError + model.FrequencyTypeId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FrequencyTypeExistenceError, data = model.FrequencyTypeId });
            }
            var response = _checkListService.AddCheckList(CreatedBy, model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
        }

        /// <summary>
        ///get CheckList by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-checkList-list-by-search-with-pagination")]
        public IActionResult GetCheckListListBySearchWithPagination(CheckListSearchRequestViewModel model)
        {
            if (model.PageSize <= 0) model.PageSize = 1;
            if (model.PageNumber <= 0) model.PageNumber = 1;
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (model.MachineTypeId != null)
            {
                if (!_machineTypeService.IsMachineTypeExist(model.MachineTypeId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineTypeExistenceError + model.MachineTypeId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineTypeExistenceError, data = model.MachineTypeId });
                }
            }
            if (model.CheckTypeId != null)
            {
                if (!_masterService.IsCheckTypeExist(model.CheckTypeId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.CheckTypeExistenceError + model.CheckTypeId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.CheckTypeExistenceError, data = model.CheckTypeId });
                }
            }
            if (model.FrequencyTypeId != null)
            {
                if (!_masterService.IsFrequencyTypeExist(model.FrequencyTypeId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FrequencyTypeExistenceError + model.FrequencyTypeId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FrequencyTypeExistenceError, data = model.FrequencyTypeId });
                }
            }
            var response = _checkListService.GetCheckListListBySearchWithPagination(UserMasterAdminId, model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get CheckList list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-checkList-list")]
        public IActionResult GetCheckListList()
        {
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _checkListService.GetCheckListList(UserMasterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get CheckList details by checkListId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-checkList-detail/{checkListId}")]
        public IActionResult GetCheckListDetail(Guid checkListId)
        {

            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + checkListId);
            if (!_checkListService.IsCheckListExist(checkListId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.CheckListExistenceError + checkListId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.CheckListExistenceError, data = checkListId });
            }
            var response = _checkListService.GetCheckListDetails(checkListId);

            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///update CheckList details by checkListId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPut]
        [Route("update-checkList-detail/{checkListId}")]
        public IActionResult UpdateCheckListDetail(Guid checkListId, CheckListRequestViewModel model)
        {
            CheckListViewModel checkListView = new() { Name = model.Name, CheckListId = checkListId, Frequency = model.Frequency, MachineTypeId = model.MachineTypeId, CheckTypeId = model.CheckTypeId, FrequencyTypeId = model.FrequencyTypeId };
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + checkListId);
            if (!_checkListService.IsCheckListExist(checkListId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.CheckListExistenceError + checkListId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.CheckListExistenceError, data = checkListId });
            }
            if (!_machineTypeService.IsMachineTypeExist(model.MachineTypeId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineTypeExistenceError + model.MachineTypeId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineTypeExistenceError, data = model.MachineTypeId });
            }
            if (!_masterService.IsCheckTypeExist(model.CheckTypeId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.CheckTypeExistenceError + model.CheckTypeId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.CheckTypeExistenceError, data = model.CheckTypeId });
            }
            if (!_masterService.IsFrequencyTypeExist(model.FrequencyTypeId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FrequencyTypeExistenceError + model.FrequencyTypeId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FrequencyTypeExistenceError, data = model.FrequencyTypeId });
            }
            var response = _checkListService.UpdateCheckListDetails(checkListView);
            if (response == null)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RecordUpdateUnSuccessfull + response);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RecordUpdateUnSuccessfull, data = response });
            }
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = response });
        }

        /// <summary>
        ///update CheckList details by OperatorId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize(Roles = "Operator,Both")]
        [HttpPut]
        [Route("update-checkList-detail-by-operatorId/{checkListId}")]
        public IActionResult UpdateCheckListDetailByOperatorId(Guid checkListId, OperatorCheckListRequestViewModel model)
        {
            var operatorId = User.GetUserId();
            OperatorCheckListResponseViewModel checkListView = new() { CheckListId = checkListId, OperatorId = operatorId, LastCheckDate = model.LastCheckDate.Value, NextDueDate = model.NextDueDate.Value };
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + checkListId);
            if (!_checkListService.IsCheckListExist(checkListId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.CheckListExistenceError + checkListId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.CheckListExistenceError, data = checkListId });
            }
            var response = _checkListService.UpdateCheckListDetailByOperatorId(checkListView);
            if (response == null)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RecordUpdateUnSuccessfull + response);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RecordUpdateUnSuccessfull, data = response });
            }
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = response });
        }

        /// <summary>
        ///delete CheckList
        /// </summary>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpDelete]
        [Route("delete-checkList/{checkListId}")]
        public IActionResult DeleteCheckList(Guid checkListId)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + checkListId);
            if (!_checkListService.IsCheckListExist(checkListId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.CheckListExistenceError + checkListId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.CheckListExistenceError, data = checkListId });
            }
            var response = _checkListService.DeleteCheckList(checkListId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordDeleteSuccess, data = response });
        }

        //CheckListItem API's

        /// <summary>
        ///add CheckListItem by checkListId into system
        /// </summary>
        /// <param name = "model" ></ param >
        /// < returns ></ returns >
        /// < response code="400">If the item is null</response>
        [HttpPost]
        [Route("add-checkListItem/{checkListId}")]
        public async Task<IActionResult> AddCheckListItem(Guid checkListId, List<CheckListItemsListViewModel> model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (!_checkListService.IsCheckListExist(checkListId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.CheckListExistenceError + checkListId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.CheckListExistenceError, data = checkListId });
            }
            var response = _checkListService.AddCheckListItems(checkListId, model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
        }

        /// <summary>
        ///get CheckListItem details by checkListId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-checkListItems-by-checkListId/{checkListId}")]
        public IActionResult GetCheckListItems(Guid checkListId)
        {

            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + checkListId);
            if (!_checkListService.IsCheckListExist(checkListId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.CheckListExistenceError + checkListId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.CheckListExistenceError, data = checkListId });
            }
            var response = _checkListService.GetCheckListItems(checkListId);

            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }
        /// <summary>
        /// Start CheckList 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize(Roles = "Operator,Both")]
        [HttpPost]
        [Route("start-checkList")]
        public ActionResult StartCheckList(StartCheckListViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var OperatorId = User.GetUserId();
            var UserMasterAdminId = User.GetUserMasterAdminId();
            if (!_machineService.IsMachineExist(model.MachineId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineExistanceError + model.MachineId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineExistanceError, data = model.MachineId });
            }
            if (!_checkListService.IsCheckListExist(model.CheckListId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.CheckListExistenceError + model.CheckListId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.CheckListExistenceError, data = model.CheckListId });
            }
            foreach (var item in model.Items)
            {
                var check = _checkListService.IsCheckListItemExists(model.CheckListId, item.CheckListItemId);
                if (check == false)
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.CheckListItemExistenceError + item.CheckListItemId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.CheckListItemExistenceError, data = item.CheckListItemId });
                }
            }
            var response = _checkListService.StartCheckList(OperatorId, model, UserMasterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
        }

        /// <summary>
        /// Get last checkList aginst machineId
        /// </summary>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize(Roles = "Operator,Both")]
        [HttpGet]
        [Route("get-last-checkList/{checkListId}/{machineId}")]
        public ActionResult GetLastCheckList(Guid checkListId, Guid machineId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(checkListId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (!_machineService.IsMachineExist(machineId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineExistanceError + machineId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineExistanceError, data = machineId });
            }
            if (!_checkListService.IsCheckListExist(checkListId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.CheckListExistenceError + checkListId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.CheckListExistenceError, data = checkListId });
            }
            var response = _checkListService.GetLastCheckList(checkListId, machineId);
            if (response == null)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + response);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RecordGetUnSuccess, data = response });
            }
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        ///// <summary>
        /////get Pre-Check history by search with pagination
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        ///// <response code="400">If the item is null</response>
        //[HttpPost]
        //[Route("get-all-machine-pre-check-history")]
        //public IActionResult GetAllPreCheckLogsBySearchWithPagination(PreCheckLogsRequestViewModel model)
        //{
        //    if (model.PageSize <= 0) model.PageSize = 1;
        //    if (model.PageNumber <= 0) model.PageNumber = 1;
        //    string UserMasterAdminId = User.GetUserMasterAdminId();
        //    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
        //    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
        //    if (model.MachineId != null)
        //    {
        //        if (!_machineService.IsMachineExist(model.MachineId.Value))
        //        {
        //            NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineExistanceError + model.MachineId);
        //            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineExistanceError, data = model.MachineId });
        //        }
        //    }
        //    if (model.FrequencyTypeId != null)
        //    {
        //        if (!_masterService.IsFrequencyTypeExist(model.FrequencyTypeId.Value))
        //        {
        //            NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FrequencyTypeExistenceError + model.FrequencyTypeId);
        //            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FrequencyTypeExistenceError, data = model.FrequencyTypeId });
        //        }
        //    }
        //    var response = _checkListService.GetPreCheckLogsBySearchWithPagination(UserMasterAdminId, model);
        //    var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
        //    return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        //}

        /// <summary>
        ///get Pre-Check logs by search 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-pre-check-logs")]
        public IActionResult GetPreCheckLogsBySearchWithPagination(PreCheckLogsRequestViewModel model)
        {
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
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
            if (model.FrequencyTypeId != null)
            {
                if (!_masterService.IsFrequencyTypeExist(model.FrequencyTypeId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FrequencyTypeExistenceError + model.FrequencyTypeId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FrequencyTypeExistenceError, data = model.FrequencyTypeId });
                }
            }
            var response = _checkListService.GetPreCheckLogsBySearch(UserMasterAdminId, model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get checklist  list by machineId
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-checkList-list-by/{machineId}")]
        public IActionResult GetCheckListList(Guid machineId)
        {
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _checkListService.GetCheckListList(machineId ,UserMasterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }



        /// <summary>
        ///delete CheckListItem
        /// </summary>
        /// <param name="checkListItemId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpDelete]
        [Route("delete-checkListItem/{checkListItemId}")]
        public IActionResult DeleteCheckListItem(Guid checkListItemId)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + checkListItemId);
            if (!_checkListService.IsCheckListItemExist(checkListItemId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.CheckListItemExistenceError + checkListItemId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.CheckListItemExistenceError, data = checkListItemId });
            }
            var response = _checkListService.DeleteCheckListItem(checkListItemId);
            if (response == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.CheckListItemError);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.CheckListItemError, data = response });
            }
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordDeleteSuccess, data = response });
        }

    }
}
