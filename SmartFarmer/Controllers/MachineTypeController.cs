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
    public class MachineTypeController : ControllerBase
    {
        private IMachineTypeService _machineTypeService;
        private ITrainingService _trainingService;
        private IRiskAssessmentService _riskAssessmentService;
        private readonly IUserService _userService;
        public MachineTypeController(IMachineTypeService machineTypeService, ITrainingService trainingService, IRiskAssessmentService riskAssessmentService, IUserService userService)
        {
            _machineTypeService = machineTypeService;
            _trainingService = trainingService;
            _riskAssessmentService = riskAssessmentService;
            _userService = userService;
        }

        /// <summary>
        ///add MachineType into system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("add-machineType")]
        public IActionResult AddMachineType(MachineTypeRequestViewModel model)
        {

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (model.TrainingId != null)
            {
                if (!_trainingService.IsTrainingExist(model.TrainingId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingExistenceError + model.TrainingId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingExistenceError, data = model.TrainingId });
                }
            }
            if(model.RiskAssessmentId != null)
            {
                if (!_riskAssessmentService.IsRiskAssessmentExist(model.RiskAssessmentId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.SmartCourseExistenceError + model.RiskAssessmentId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RiskAssessmentExistenceError, data = model.RiskAssessmentId });
                }
            }
           
            if (!_machineTypeService.IsUnitsTypeExist(model.UnitsTypeId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.UnitsTypeExistenceError + model.UnitsTypeId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UnitsTypeExistenceError, data = model.UnitsTypeId });
            }
            var CreatedBy = User.GetUserId();
            var response = _machineTypeService.AddMachineType(CreatedBy,model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
        }

        /// <summary>
        ///get MachineType list by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-machineType-list-by-search-with-pagination")]
        public IActionResult GetMachineTypeListBySearchWithPagination(MachineTypeSearchRequestViewModel model)
        {
            if (model.PageSize <= 0) model.PageSize = 1;
            if (model.PageNumber <= 0) model.PageNumber = 1;
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (model.RiskAssessmentId != null)
            {
                if (!_riskAssessmentService.IsRiskAssessmentExist(model.RiskAssessmentId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.SmartCourseExistenceError + model.RiskAssessmentId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RiskAssessmentExistenceError, data = model.RiskAssessmentId });
                }
            }
            var response = _machineTypeService.GetMachineTypeListBySearchWithPagination(UserMasterAdminId,model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get riskAssessment details by riskAssessmentId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-machineType-detail/{machineTypeId}")]
        public IActionResult GetMachineTypeDetail(Guid machineTypeId)
        {

            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + machineTypeId);
            if (!_machineTypeService.IsMachineTypeExist(machineTypeId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineTypeExistenceError + machineTypeId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineTypeExistenceError, data = machineTypeId });
            }
            var response = _machineTypeService.GetMachineTypeDetails(machineTypeId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///update MachineType details by machineTypeId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPut]
        [Route("update-machineType-detail/{machineTypeId}")]
        public IActionResult UpdateMachineTypeDetail(Guid machineTypeId, MachineTypeRequestViewModel model)
        {
            MachineTypeViewModel machineTypeView = new() { Name = model.Name, MachineTypeId = machineTypeId, NeedsTraining = model.NeedsTraining, TrainingId = model?.TrainingId, UnitsTypeId = model.UnitsTypeId,RiskAssessmentId=model.RiskAssessmentId };
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + machineTypeId);
            if(model.TrainingId != null)
            {
                if (!_trainingService.IsTrainingExist(model.TrainingId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingExistenceError + model.TrainingId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingExistenceError, data = model.TrainingId });
                }
            }
            if (!_machineTypeService.IsMachineTypeExist(machineTypeId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineTypeExistenceError + machineTypeId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineTypeExistenceError, data = machineTypeId });
            }
            if (model.RiskAssessmentId != null)
            {
                if (!_riskAssessmentService.IsRiskAssessmentExist(model.RiskAssessmentId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.SmartCourseExistenceError + model.RiskAssessmentId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RiskAssessmentExistenceError, data = model.RiskAssessmentId });
                }
            }
            if (!_machineTypeService.IsUnitsTypeExist(model.UnitsTypeId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.UnitsTypeExistenceError + model.UnitsTypeId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UnitsTypeExistenceError, data = model.UnitsTypeId });
            }
            var response = _machineTypeService.UpdateMachineTypeDetails(machineTypeView);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = response });
        }

        /// <summary>
        ///get machine type name list
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-machinetype-name-list")]
        public IActionResult GetMachineTypeNameList()
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(null);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _machineTypeService.GetMachineTypeNameList(UserMasterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }
    }
}
