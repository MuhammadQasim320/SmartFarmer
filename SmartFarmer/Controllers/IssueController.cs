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

namespace SmartFarmer.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private static Logger _logger;
        private readonly IIssueService _issueService;
        private readonly IMasterService _masterService;
        private readonly IMachineService _machineService;
        private readonly IIssueCategoryService _issueCategoryService;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        private readonly IRiskAssessmentService _riskAssessmentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IssueController(IIssueService issueService, IMasterService masterService, IMachineService machineService, IIssueCategoryService issueCategoryService, IFileService fileService, IUserService userService, UserManager<ApplicationUser> userManager, IRiskAssessmentService riskAssessmentService)
        {
            _issueService = issueService;
            _masterService = masterService;
            _machineService = machineService;
            _issueCategoryService = issueCategoryService;
            _fileService = fileService;
            _userService = userService;
            _userManager = userManager;
            _riskAssessmentService = riskAssessmentService;
        }

        /// <summary>
        ///add issue into system 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        //[Authorize (Roles = "Operator")]
        [HttpPost]
        [Route("add-issue")]
        public IActionResult AddIssue(IssueRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (_masterService.IsIssueTypeExist(model.IssueTypeId) == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.IssueTypeExistanceError + model.IssueTypeId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.IssueTypeExistanceError, data = model.IssueTypeId });
            }
            if (model.MachineId != null)
            {
                if (_machineService.IsMachineExist(model.MachineId.Value) == false)
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineExistanceError + model.MachineId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineExistanceError, data = model.MachineId });
                }
            }
            if (model.RiskAssessmentLogId != null)
            {
                if (_riskAssessmentService.IsRiskAssessmentLogExist(model.RiskAssessmentLogId.Value) == false)
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RiskAssessmentLogExistenceError + model.RiskAssessmentLogId.Value);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RiskAssessmentLogExistenceError, data = model.RiskAssessmentLogId.Value });
                }
            }
            if (model.IssueCategoryId != null)
            {
                if (_issueCategoryService.IsIssueCategoryExist(model.IssueCategoryId.Value) == false)
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.IssueCategoryExistanceError + model.IssueCategoryId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.IssueCategoryExistanceError, data = model.IssueCategoryId });
                }
            }
            var CreatedBy = User.GetUserId();
            var response = _issueService.AddIssue(CreatedBy, model);
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
        ///get Issue details
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-issue-details/{issueId}")]
        public IActionResult GetIssueDetails(Guid issueId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(issueId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (_issueService.IsIssueExist(issueId) == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.IssueExistanceError + issueId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.IssueExistanceError, data = issueId });
            }
            var response = _issueService.GetIssueDetails(issueId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///update issue details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPut]
        [Route("update-issue-details")]
        public IActionResult UpdateIssueDetail(IssueUpdateRequestViewModel model)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + model.IssueId);
            if (!_issueService.IsIssueExist(model.IssueId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.IssueExistanceError + model.IssueId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.IssueExistanceError, data = model.IssueId });
            }
            if (!_masterService.IsIssueTypeExist(model.IssueTypeId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.IssueTypeExistanceError + model.IssueTypeId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.IssueTypeExistanceError, data = model.IssueTypeId });
            }
            if (model.MachineId != null)
            {
                if (!_machineService.IsMachineExist(model.MachineId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineExistanceError + model.MachineId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineExistanceError, data = model.MachineId });
                }
            }

            if (model.IssueCategoryId != null)
            {
                if (!_issueCategoryService.IsIssueCategoryExist(model.IssueCategoryId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.IssueCategoryExistanceError + model.IssueCategoryId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.IssueCategoryExistanceError, data = model.IssueCategoryId });
                }
            }
            if (model.RiskAssessmentLogId != null)
            {
                if (!_riskAssessmentService.IsRiskAssessmentLogExist(model.RiskAssessmentLogId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RiskAssessmentExistenceError + model.RiskAssessmentLogId.Value);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RiskAssessmentExistenceError, data = model.RiskAssessmentLogId.Value });
                }
            }
            if (!_masterService.IsIssueStatusExist(model.IssueStatusId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.IssueStatusExistanceError + model.IssueStatusId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.IssueStatusExistanceError, data = model.IssueStatusId });
            }
            if (model.ResolvedBy != null)
            {
                if (!_userService.IsUserExist(model.ResolvedBy))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.ResolvedByUserExistanceError + model.ResolvedBy);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.ResolvedByUserExistanceError, data = model.ResolvedBy });
                }
            }
            IssueResponseViewModel issueResponseViewModel = new IssueResponseViewModel()
            {
                IssueId = model.IssueId,
                IssueTitle = model.IssueTitle,
                Description = model.Description,
                IssueCategoryId = model.IssueCategoryId,
                IssueTypeId = model.IssueTypeId,
                MachineId = model.MachineId,
                IsTargetDateExist = model.IsTargetDateExist,
                TargetDate = model.TargetDate,
                ResolvedBy = model.ResolvedBy,
                RiskAssessmentLogId = model?.RiskAssessmentLogId,
                ResolvedDate = model?.ResolvedDate,
                IssueStatusId = model.IssueStatusId,
                Note = model?.Note,
            };
            var response = _issueService.UpdateIssueDetails(issueResponseViewModel);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = response });
        }

        /// <summary>
        ///get machine issues list
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-machine-issues-list/{machineId}")]
        public IActionResult GetMachineIssuesList(Guid machineId)
        {
            string loginUserId = User.GetUserId();
            var masterAdminId = _userService.GetMasterAdminId(loginUserId);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(machineId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (_machineService.IsMachineExist(machineId) == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineExistanceError + machineId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineExistanceError, data = machineId });
            }
            var response = _issueService.GetMachineIssuesList(machineId, masterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///upload issue images
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("upload-issue-images/{issueId}")]
        public IActionResult UploadIssueImages(Guid issueId, List<IFormFile> Images)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(issueId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (_issueService.IsIssueExist(issueId) == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.IssueExistanceError + issueId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.IssueExistanceError, data = issueId });
            }
            if(Images.Count <= 0)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.ImageCountError + null);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.ImageCountError, data = null });
            }
            IssueFileListViewModel issueFileListViewModel = new IssueFileListViewModel();
            foreach (var image in Images)
            {

                var uploadedFile = _fileService.UploadFile(image);
                if (uploadedFile != null)
                {
                    IssueFileViewModel issueFileViewModel = new IssueFileViewModel
                    {
                        IssueId = issueId,
                        FileUniqueName = uploadedFile.Name,
                        FileURL = uploadedFile.FileLink
                    };
                    var upload = _issueService.UploadIssueFile(issueFileViewModel);

                    issueFileListViewModel.issueFiles.Add(upload);
                }
                else
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.ImageUploadUnSuccessful + image.Name);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.ImageUploadUnSuccessful, data = image.Name });
                }
            }
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + issueFileListViewModel);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = issueFileListViewModel });
        }

        /// <summary>
        ///get issue images
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-issue-images/{issueId}")]
        public IActionResult GetIssueImages(Guid issueId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(issueId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (_issueService.IsIssueExist(issueId) == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.IssueExistanceError + issueId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.IssueExistanceError, data = issueId });
            }
            var response = _issueService.GetIssueFilesList(issueId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + response);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get issue list by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-issue-list-by-search-with-pagination")]
        public IActionResult GetIssueListBySearchWithPagination(SearchIssueRequestViewModel model)
        {
            if (model.PageSize <= 0) model.PageSize = 1;
            if (model.PageNumber <= 0) model.PageNumber = 1;
            string loginUserId = User.GetUserId();
            var masterAdminId = _userService.GetMasterAdminId(loginUserId);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var response = _issueService.GetIssueListBySearchWithPagination(model, masterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get machine existing issues list
        /// </summary>
        /// <param name="MachineId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-machine-existing-issues")]
        public IActionResult GetMachineExistingIssuesList(Guid MachineId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(MachineId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var response = _issueService.GetMachineExistingIssuesList(MachineId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        ///// <summary>
        /////add issue comment into system 
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        ///// <response code="400">If the item is null</response>
        //[Authorize(Roles = "Operator")]
        //[HttpPost]
        //[Route("add-issue-comment")]
        //public IActionResult AddIssueComment(IssueCommentRequestViewModel model)
        //{
        //    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
        //    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
        //    if (!_issueService.IsIssueExist(model.IssueId))
        //    {
        //        NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.IssueExistanceError + model.IssueId);
        //        return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.IssueExistanceError, data = model.IssueId });
        //    }
        //    var CreatedBy = User.GetUserId();
        //    var response = _issueService.AddIssueComment(CreatedBy, model);
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
        /// add issue comment into system 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize(Roles = "Operator,Both")]
        [HttpPost]
        [Route("add-issue-comment")]
        public IActionResult AddIssueComment(IssueCommentRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (!_issueService.IsIssueExist(model.IssueId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.IssueExistanceError + model.IssueId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.IssueExistanceError, data = model.IssueId });
            }
            var response = _issueService.AddIssueComment(model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
        }




        /// <summary>
        ///delete issueFile  by issueFileId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpDelete]
        [Route("delete-issue-file/{issueFileId}")]
        public IActionResult DeleteIssueImage(Guid issueFileId)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + issueFileId);
            if (!_issueService.IsIssueFileExist(issueFileId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.IssueFileExistanceError + issueFileId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.IssueFileExistanceError, data = issueFileId });
            }
            var res = _issueService.GetIssueFile(issueFileId);
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
            var response = _issueService.DeleteIssueFile(issueFileId);
            if (response == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FileDeleteUnSuccessfull + response);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FileDeleteUnSuccessfull, data = response });
            }
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.FileDeleteSuccessfull, data = response });
        }
    }
}
