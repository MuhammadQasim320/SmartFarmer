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
    public class IssueCategoryController : ControllerBase
    {
        private static Logger _logger;
        private readonly IIssueCategoryService _issueCategoryService;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IssueCategoryController(IIssueCategoryService issueCategoryService, IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _issueCategoryService = issueCategoryService;
            _userService = userService;
            _userManager = userManager;
        }

        /// <summary>
        ///add issueCategory into system 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("add-issueCategory")]
        public IActionResult AddIssueCategory(IssueCategoryRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var CreatedBy = User.GetUserId();
            var response = _issueCategoryService.AddIssueCategory(CreatedBy, model);
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
        ///get issueCategory list by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-issueCategory-list-by-search-with-pagination")]
        public IActionResult GetIssueCategoryListBySearchWithPagination(SearchIssueCategoryRequestViewModel model)
        {
            if (model.PageSize <= 0) model.PageSize = 1;
            if (model.PageNumber <= 0) model.PageNumber = 1;
            string loginUserId = User.GetUserId();
            var masterAdminId = _userService.GetMasterAdminId(loginUserId);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var response = _issueCategoryService.GetIssueCategoryListBySearchWithPagination(model, masterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get issue category details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-issuecategory-details/{issueCategoryId}")]
        public IActionResult GetIssueCategoryDetails(Guid issueCategoryId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(issueCategoryId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (_issueCategoryService.IsIssueCategoryExist(issueCategoryId) == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.IssueCategoryExistanceError + issueCategoryId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.IssueCategoryExistanceError, data = issueCategoryId });
            }
            var response = _issueCategoryService.GetIssueCategoryDetails(issueCategoryId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///update issue category details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPut]
        [Route("update-issue-category-detail/{issueCategoryId}")]
        public IActionResult UpdateIssueCategoryDetail(Guid issueCategoryId, IssueCategoryRequestViewModel model)
        {
            IssueCategoryResponseViewModel issueCategoryViewModel = new() { IssueCategoryId = issueCategoryId, Category = model.Category };
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + issueCategoryId);
            if (!_issueCategoryService.IsIssueCategoryExist(issueCategoryId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.IssueCategoryExistanceError + issueCategoryId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.IssueCategoryExistanceError, data = issueCategoryId });
            }
            var response = _issueCategoryService.UpdateIssueCategoryDetails(issueCategoryViewModel);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = response });
        }

        /// <summary>
        ///get IssueCategory Name List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-issueCategory-name-list")]
        public IActionResult GetissueCategoryNameList()
        {
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(UserMasterAdminId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var response = _issueCategoryService.GetIssueCategoryNameList(UserMasterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///delete IssueCategory
        /// </summary>
        /// <param name="issueCategoryId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpDelete]
        [Route("delete-issueCategory/{issueCategoryId}")]
        public IActionResult DeleteIssueCategory(Guid issueCategoryId)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + issueCategoryId);
            if (_issueCategoryService.IsIssueCategoryExist(issueCategoryId) == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.IssueCategoryExistanceError + issueCategoryId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.IssueCategoryExistanceError, data = issueCategoryId });
            }
            var response = _issueCategoryService.DeleteIssueCategory(issueCategoryId);
            if(response == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.IssueCategoryInUseError + issueCategoryId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.IssueCategoryInUseError, data = issueCategoryId });
            }
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordDeleteSuccess, data = response });
        }
    }
}
