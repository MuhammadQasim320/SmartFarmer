using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;
using static SmartFarmer.Core.Common.Enums;
using static SmartFarmer.Core.ViewModel.MasterViewModel;

namespace SmartFarmer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly IMasterService _masterService;
        public MasterController(IMasterService masterService)
        {
            _masterService = masterService;
        }

        ///// <summary>
        ///// Get Action Type
        ///// </summary>
        ///// <returns></returns>
        ///// <response code="400">If the item is null</response>
        //[HttpGet]
        //[Route("get-action-types")]
        //public IActionResult GetActionTypes()
        //{
        //    NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request);
        //    var response = _masterService.GetActionTypes();
        //    ActionTypeListViewModel listViewModel = new() { List = response };
        //    var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(listViewModel);
        //    NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
        //    return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        //}

        /// <summary>
        /// Get User Status
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-user-statuses")]
        public IActionResult GetUserStatuses()
        {
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request);
            var response = _masterService.GetUserStatuses();
            UserStatusListViewModel listViewModel = new() { List = response };
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(listViewModel);
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        /// Get Operator Status
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-operator-statuses")]
        public IActionResult GetOperatorStatuses()
        {
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request);
            var response = _masterService.GetOperatorStatuses();
            OperatorStatusListViewModel listViewModel = new() { List = response };
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(listViewModel);
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        /// Get User Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-user-types")]
        public IActionResult GetUserTypes()
        {
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request);
            var response = _masterService.GetUserTypes();
            UserTypeListViewModel listViewModel = new() { List = response };
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(listViewModel);
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        /// Get Check Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-check-types")]
        public IActionResult GetCheckTypes()
        {
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request);
            var response = _masterService.GetCheckTypes();
            CheckTypeListViewModel listViewModel = new() { List = response };
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(listViewModel);
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        /// Get Event Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-event-types")]
        public IActionResult GetEventTypes()
        {
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request);
            var response = _masterService.GetEventTypes();
            EventTypeListViewModel listViewModel = new() { List = response };
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(listViewModel);
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        /// Get Frequency Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-frequency-types")]
        public IActionResult GetFrequencyTypes()
        {
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request);
            var response = _masterService.GetFrequencyTypes();
            FrequencyTypeListViewModel listViewModel = new() { List = response };
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(listViewModel);
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        /// Get InitialRiskAndAdjustedRisk Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-initialRiskAndAdjustedRisks")]
        public IActionResult GetInitialRiskAndAdjustedRisks()
        {
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request);
            var response = _masterService.GetInitialRiskAndAdjustedRisks();
            InitialRiskAndAdjustedRiskListViewModel listViewModel = new() { List = response };
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(listViewModel);
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        /// Get Training Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-training-types")]
        public IActionResult GetTrainingTypes()
        {
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request);
            var response = _masterService.GetTrainingTypes();
            TrainingTypeListViewModel listViewModel = new() { List = response };
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(listViewModel);
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        /// Get Units Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-units-types")]
        public IActionResult GetUnitsTypes()
        {
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request);
            var response = _masterService.GetUnitsTypes();
            UnitsTypeListViewModel listViewModel = new() { List = response };
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(listViewModel);
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }
        
        /// <summary>
        /// Get Machine Statuses
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-machine-statuses")]
        public IActionResult GetMachineStatuses()
        {
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request);
            var response = _masterService.GetMachineStatuses();
            MachineStatusListViewModel listViewModel = new() { List = response };
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(listViewModel);
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }
        
        /// <summary>
        /// Get Issue Types
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-issue-types")]
        public IActionResult GetIssueTypes()
        {
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request);
            var response = _masterService.GetIssueTypes();
            IssueTypeListViewModel listViewModel = new() { List = response };
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(listViewModel);
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }
        
        /// <summary>
        /// Get Issue Statuses
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-issue-statuses")]
        public IActionResult GetIssueStatuses()
        {
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request);
            var response = _masterService.GetIssueStatuses();
            IssueStatusListViewModel listViewModel = new() { List = response };
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(listViewModel);
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        /// Get Alarm Types
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-alarm-types")]
        public IActionResult GetAlarmTypes()
        {
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request);
            var response = _masterService.GetAlarmTypes();
            EventTypeListViewModel listViewModel = new() { List = response };
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(listViewModel);
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }
        
        /// <summary>
        /// Get hazard Types
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-hazard-types")]
        public IActionResult GetHazardTypes()
        {
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request);
            var response = _masterService.GetHazardTypes();
            HazardTypeListViewModel listViewModel = new() { List = response };
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(listViewModel);
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }




        /// <summary>
        /// Get mobileAction types
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-mobileAction-types")]
        public IActionResult GetMobileActionTypes()
        {
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request);
            var response = _masterService.GetMobileActionTypes();
            MobileActionTypeListViewModel listViewModel = new() { List = response };
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(listViewModel);
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }


        /// <summary>
        /// Get checkResults
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-checkResults")]
        public IActionResult GetCheckResults()
        {
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request);
            var response = _masterService.GetCheckResults();
            CheckResultListViewModel listViewModel = new() { List = response };
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(listViewModel);
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }
    }
}
