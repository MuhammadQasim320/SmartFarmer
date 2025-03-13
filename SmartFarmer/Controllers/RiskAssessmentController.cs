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
    public class RiskAssessmentController : ControllerBase
    {
        private IRiskAssessmentService _riskAssessmentService;
        private IFileService _fileService;
        private readonly IUserService _userService;

        public RiskAssessmentController(IRiskAssessmentService riskAssessmentService, IFileService fileService, IUserService userService)
        {
            _riskAssessmentService = riskAssessmentService;
            _fileService = fileService;
            _userService = userService;
        }

        /// <summary>
        ///add riskAssessment into system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("add-riskAssessment")]
        public IActionResult AddRiskAssessment(RiskAssessmentRequestViewModel model)
        {

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var CreatedBy = User.GetUserId();
            var response = _riskAssessmentService.AddRiskAssessment(CreatedBy, model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
        }

        /// <summary>
        ///get riskAssessment list by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-riskAssessment-list-by-search-with-pagination")]
        public IActionResult GetRiskAssessmentListBySearchWithPagination(RiskAssessmentSearchRequestViewModel model)
        {
            if (model.PageSize <= 0) model.PageSize = 1;
            if (model.PageNumber <= 0) model.PageNumber = 1;
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _riskAssessmentService.GetRiskAssessmentListBySearchWithPagination(UserMasterAdminId, model);
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
        [Route("get-riskAssessment-detail/{riskAssessmentId}")]
        public IActionResult GetRiskAssessmentDetail(Guid riskAssessmentId)
        {

            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + riskAssessmentId);
            if (!_riskAssessmentService.IsRiskAssessmentExist(riskAssessmentId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.SmartCourseExistenceError + riskAssessmentId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RiskAssessmentExistenceError, data = riskAssessmentId });
            }
            var response = _riskAssessmentService.GetRiskAssessmentDetails(riskAssessmentId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get riskAssessment files by riskAssessmentId
        /// </summary>
        /// <param name="riskAssessmentId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-riskAssessment-files/{riskAssessmentId}")]
        public IActionResult GetRiskAssessmentFiles(Guid riskAssessmentId)
        {

            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + riskAssessmentId);
            if (!_riskAssessmentService.IsRiskAssessmentExist(riskAssessmentId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.SmartCourseExistenceError + riskAssessmentId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RiskAssessmentExistenceError, data = riskAssessmentId });
            }
            var response = _riskAssessmentService.GetRiskAssessmentFiles(riskAssessmentId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///update riskAssessment details by riskAssessmentId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPut]
        [Route("update-riskAssessment-detail/{riskAssessmentId}")]
        public IActionResult UpdateRiskAssessmentDetail(Guid riskAssessmentId, RiskAssessmentRequestViewModel model)
        {
            RiskAssessmentViewModel riskAssessmentView = new() { Name = model.Name, RiskAssessmentId = riskAssessmentId, Validity = model.Validity };
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + riskAssessmentId);
            if (!_riskAssessmentService.IsRiskAssessmentExist(riskAssessmentId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RiskAssessmentExistenceError + riskAssessmentId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RiskAssessmentExistenceError, data = riskAssessmentId });
            }
            var response = _riskAssessmentService.UpdateRiskAssessmentDetails(riskAssessmentView);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = response });
        }

        /// <summary>
        ///upload riskAssessment file by riskAssessmentId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("upload-riskAssessment-file/{riskAssessmentId}")]
        public IActionResult UploadRiskAssessmentFile(Guid riskAssessmentId, [FromForm] AddFileViewModel model)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + riskAssessmentId);
            if (!_riskAssessmentService.IsRiskAssessmentExist(riskAssessmentId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RiskAssessmentExistenceError + riskAssessmentId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RiskAssessmentExistenceError, data = riskAssessmentId });
            }
            var fileResponse = _fileService.UploadFile(model.File);
            if (fileResponse != null)
            {
                RiskAssessmentFileViewModel riskAssessmentFile = new() { RiskAssessmentId = riskAssessmentId, FileUrl = fileResponse.FileLink, FileUniqueName = fileResponse.Name, FileName = model.FileName };
                var response = _riskAssessmentService.UploadRiskAssessmentFile(riskAssessmentFile);
                if (response == null)
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RecordUpdateUnSuccessfull + response);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RecordUpdateUnSuccessfull, data = response });
                }
                var jsonResponse1 = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FileUploadSuccessfull + jsonResponse1);
                return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.FileUploadSuccessfull, data = response });
            }
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(fileResponse);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FileUploadUnSuccessfull + jsonResponse);
            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FileUploadUnSuccessfull, data = fileResponse });
        }

        /// <summary>
        ///delete riskAssessment file by riskAssessmentId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpDelete]
        [Route("delete-riskAssessment-file/{riskAssessmentFileId}")]
        public IActionResult DeleteRiskAssessmentFile(Guid riskAssessmentFileId)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + riskAssessmentFileId);
            if (!_riskAssessmentService.IsRiskAssessmentFileExist(riskAssessmentFileId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RiskAssessmentFileExistenceError + riskAssessmentFileId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RiskAssessmentFileExistenceError, data = riskAssessmentFileId });
            }
            var res = _riskAssessmentService.GetRiskAssessmentFile(riskAssessmentFileId);
            if (res == null)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FileDeleteUnSuccessfull + res);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FileDeleteUnSuccessfull, data = res });
            }
            var deletingServerFile = _fileService.DeleteUploadFile(res.FileUniqueName);
            if (deletingServerFile == null)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FileDeleteUnSuccessfull + deletingServerFile);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FileDeleteUnSuccessfull, data = deletingServerFile });
            }
            var response = _riskAssessmentService.DeleteRiskAssessmentFile(riskAssessmentFileId);
            if (response == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FileDeleteUnSuccessfull + response);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FileDeleteUnSuccessfull, data = response });
            }
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.FileDeleteSuccessfull, data = response });
        }

        /// <summary>
        ///get riskAssessment Name List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-riskAssessment-name-list")]
        public IActionResult GetRiskAssessmentNameList()
        {
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(UserMasterAdminId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);


            var response = _riskAssessmentService.GetRiskAssessmentNameList(UserMasterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///add riskAssessmentLog into system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("add-riskAssessmentLog")]
        public IActionResult AddRiskAssessmentLog(RiskAssessmentLogRequestViewModel model)
        {

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (model.RiskAssessmentId != null)
            {
                if (!_riskAssessmentService.IsRiskAssessmentExist(model.RiskAssessmentId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.SmartCourseExistenceError + model.RiskAssessmentId.Value);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RiskAssessmentExistenceError, data = model.RiskAssessmentId.Value });
                }
            }
            if (model.InitialRiskId != null)
            {
                if (!_riskAssessmentService.IsInitialRiskExist(model.InitialRiskId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.InitialRiskExistenceError + model.InitialRiskId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.InitialRiskExistenceError, data = model.InitialRiskId });
                }
            }
            if (model.AdjustedRiskId != null)
            {
                if (!_riskAssessmentService.IsInitialRiskExist(model.AdjustedRiskId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.AdjustedRiskExistenceError + model.AdjustedRiskId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.AdjustedRiskExistenceError, data = model.AdjustedRiskId });
                }
            }
            //if (model.ActionId != null)
            //{
            //    if (!_riskAssessmentService.IsActionExist(model.ActionId.Value))
            //    {
            //        NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.ActionExistenceError + model.ActionId);
            //        return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.ActionExistenceError, data = model.ActionId });
            //    }
            //}
            var CreatedBy = User.GetUserId();
            var response = _riskAssessmentService.AddRiskAssessmentLog(CreatedBy, model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
        }

        /// <summary>
        ///get riskAssessmentLog list by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-riskAssessmentLog-list-by-search-with-pagination")]
        public IActionResult GetRiskAssessmentLogListBySearchWithPagination(RiskAssessmentLogSearchRequestViewModel model)
        {
            if (model.PageSize <= 0) model.PageSize = 1;
            if (model.PageNumber <= 0) model.PageNumber = 1;
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _riskAssessmentService.GetRiskAssessmentLogListBySearchWithPagination(UserMasterAdminId, model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get riskAssessmentLog details by riskAssessmentLogId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-riskAssessmentLog-detail/{riskAssessmentLogId}")]
        public IActionResult GetRiskAssessmentLogDetail(Guid riskAssessmentLogId)
        {

            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + riskAssessmentLogId);
            if (!_riskAssessmentService.IsRiskAssessmentLogExist(riskAssessmentLogId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RiskAssessmentLogExistenceError + riskAssessmentLogId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RiskAssessmentLogExistenceError, data = riskAssessmentLogId });
            }
            var response = _riskAssessmentService.GetRiskAssessmentLogDetails(riskAssessmentLogId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///update riskAssessmentLog details by riskAssessmentLogId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPut]
        [Route("update-riskAssessmentLog-detail/{riskAssessmentLogId}")]
        public IActionResult UpdateRiskAssessmentLogDetail(Guid riskAssessmentLogId, RiskAssessmentLogRequestViewModel model)
        {
            RiskAssessmentLogViewModel riskAssessmentLogView = new() { Name = model.Name, RiskAssessmentLogId = riskAssessmentLogId, /*CompletedDate = model.CompletedDate,*/ Expires = model.Expires, Archived = model.Archived, InitialRiskId = model.InitialRiskId, AdjustedRiskId = model.AdjustedRiskId, RiskAssessmentId = model.RiskAssessmentId };
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + riskAssessmentLogId);
            if (!_riskAssessmentService.IsRiskAssessmentLogExist(riskAssessmentLogId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RiskAssessmentLogExistenceError + riskAssessmentLogId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RiskAssessmentLogExistenceError, data = riskAssessmentLogId });
            }
            if (model.RiskAssessmentId != null)
            {
                if (!_riskAssessmentService.IsRiskAssessmentExist(model.RiskAssessmentId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RiskAssessmentExistenceError + model.RiskAssessmentId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RiskAssessmentExistenceError, data = model.RiskAssessmentId });
                }
            }
            if (model.InitialRiskId != null)
            {
                if (!_riskAssessmentService.IsInitialRiskExist(model.InitialRiskId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.InitialRiskExistenceError + model.InitialRiskId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.InitialRiskExistenceError, data = model.InitialRiskId });
                }
            }
            if (model.AdjustedRiskId != null)
            {
                if (!_riskAssessmentService.IsInitialRiskExist(model.AdjustedRiskId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.AdjustedRiskExistenceError + model.AdjustedRiskId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.AdjustedRiskExistenceError, data = model.AdjustedRiskId });
                }
            }
            //if (model.ActionId != null)
            //{
            //    if (!_riskAssessmentService.IsActionExist(model.ActionId.Value))
            //    {
            //        NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.ActionExistenceError + model.ActionId);
            //        return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.ActionExistenceError, data = model.ActionId });
            //    }
            //}
            var response = _riskAssessmentService.UpdateRiskAssessmentLogDetails(riskAssessmentLogView);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = response });
        }

        /// <summary>
        ///get corrective issue against RiskAssessmentLogId
        /// </summary>
        /// <param name="riskAssessmentLogId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-corrective-issue-list/{riskAssessmentLogId}")]
        public IActionResult GetCorrectiveIssueList(Guid riskAssessmentLogId)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + riskAssessmentLogId);
            if (!_riskAssessmentService.IsRiskAssessmentLogExist(riskAssessmentLogId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RiskAssessmentLogExistenceError + riskAssessmentLogId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RiskAssessmentLogExistenceError, data = riskAssessmentLogId });
            }
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _riskAssessmentService.GetCorrectiveIssueList(riskAssessmentLogId, UserMasterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }




        /// <summary>
        ///get riskAssessmentLog Name List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-riskAssessmentLog-name-list")]
        public IActionResult GetRiskAssessmentLogNameList()
        {
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(UserMasterAdminId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);


            var response = _riskAssessmentService.GetRiskAssessmentLogNameList(UserMasterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }



        /// <summary>
        ///get riskAssessment with riskAssessmentLog  details by riskAssessmentId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-riskAssessment-with-Log-detail/{riskAssessmentId}")]
        public IActionResult GetRiskAssessmentWithLogDetail(Guid riskAssessmentId)
        {

            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + riskAssessmentId);
            if (!_riskAssessmentService.IsRiskAssessmentExist(riskAssessmentId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.SmartCourseExistenceError + riskAssessmentId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RiskAssessmentExistenceError, data = riskAssessmentId });
            }
            var response = _riskAssessmentService.GetRiskAssessmentWithLogDetail(riskAssessmentId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

    }
}
