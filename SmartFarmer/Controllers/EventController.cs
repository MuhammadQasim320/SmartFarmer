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
    public class EventController : ControllerBase
    {
        private static Logger _logger;
        private readonly IEventService _eventService;
        private readonly IMasterService _masterService;
        private readonly IMachineService _machineService;
        private readonly IUserService _userService;

        public EventController(IEventService eventService, IMasterService masterService, IMachineService machineService, IUserService userService)
        {
            _eventService = eventService;
            _masterService = masterService;
            _machineService = machineService;
            _userService = userService;
        }

        ///// <summary>
        /////add event into system 
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        ///// <response code="400">If the item is null</response>
        //[HttpPost]
        //[Route("add-event")]
        //public IActionResult AddEvent(EventRequestViewModel model)
        //{
        //    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
        //    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
        //    if (_masterService.IsEventTypeExist(model.EventTypeId) == false)
        //    {
        //        NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.EventTypeExistanceError + model.EventTypeId);
        //        return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.EventTypeExistanceError, data = model.EventTypeId });
        //    }
        //    if (model.MachineId != null)
        //    {
        //        if (_machineService.IsMachineExist(model.MachineId.Value) == false)
        //        {
        //            NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineExistanceError + model.MachineId);
        //            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineExistanceError, data = model.MachineId });
        //        }
        //    }
        //    var CreatedBy = User.GetUserId();
        //    var response = _eventService.AddEvent(CreatedBy,model);
        //    var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //    if (response != null)
        //    {
        //        NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
        //        return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
        //    }
        //    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
        //    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RecordAddError, data = response });

        //}

        /// <summary>
        ///get Event list by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-event-list-by-search-with-pagination")]
        public IActionResult GeEventListBySearchWithPagination(SearchEventRequestViewModel model)
        {
            if (model.PageSize <= 0) model.PageSize = 1;
            if (model.PageNumber <= 0) model.PageNumber = 1;
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (model.EventTypeId != null)
            {
                if (_masterService.IsEventTypeExist(model.EventTypeId.Value) == false)
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.EventTypeExistanceError + model.EventTypeId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.EventTypeExistanceError, data = model.EventTypeId });
                }
            }
            var response = _eventService.GetEventListBySearchWithPagination(UserMasterAdminId,model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get Event details
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-event-details/{eventId}")]
        public IActionResult GetEventDetails(Guid eventId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(eventId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (_eventService.IsEventExist(eventId) == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.EventExistanceError + eventId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.EventExistanceError, data = eventId });
            }
            var response = _eventService.GetEventDetails(eventId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///update event details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPut]
        [Route("update-event-detail/{eventId}")]
        public IActionResult UpdateEventDetail(Guid eventId, EventRequestViewModel model)
        {
            EventResponseViewModel eventViewModel = new() { EventId = eventId, Location = model.Location };
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + eventId);
            if (!_eventService.IsEventExist(eventId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.EventExistanceError + eventId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.EventExistanceError, data = eventId });
            }
            //if (_masterService.IsEventTypeExist(model.EventTypeId) == false)
            //{
            //    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.EventTypeExistanceError + model.EventTypeId);
            //    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.EventTypeExistanceError, data = model.EventTypeId });
            //}
            //if (model.MachineId != null)
            //{
            //    if (_machineService.IsMachineExist(model.MachineId.Value) == false)
            //    {
            //        NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineExistanceError + model.MachineId);
            //        return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineExistanceError, data = model.MachineId });
            //    }
            //}
            var response = _eventService.UpdateEventDetails(eventViewModel);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = response });
        }
    }
}
