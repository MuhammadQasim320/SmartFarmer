using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public class FieldController : Controller
    {
        private static Logger _logger;
        private readonly IFieldService _fieldService;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public FieldController(IFieldService fieldService, IUserService userService,UserManager<ApplicationUser> userManager)
        {
            _fieldService = fieldService;
            _userService = userService;
            _userManager = userManager;
        }

        /// <summary>
        ///add field into system 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("add-field")]
        public IActionResult AddField(FieldRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var CreatedBy = User.GetUserId();
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _fieldService.AddField(CreatedBy, UserMasterAdminId, model);
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
        ///get field list by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-field-list-by-search-with-pagination")]
        public IActionResult GetFieldListBySearchWithPagination(SearchFieldRequestViewModel model)
        {
            if (model.PageSize <= 0) model.PageSize = 1;
            if (model.PageNumber <= 0) model.PageNumber = 1;
            string loginUserId = User.GetUserId();
            var masterAdminId = _userService.GetMasterAdminId(loginUserId);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var response = _fieldService.GetFieldListBySearchWithPagination(model, masterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get feild details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-feild-details/{fieldId}")]
        public IActionResult GetFieldDetails(Guid fieldId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(fieldId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (_fieldService.IsFieldExist(fieldId) == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FieldExistanceError + fieldId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FieldExistanceError, data = fieldId });
            }
            var response = _fieldService.GetFieldDetails(fieldId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///update Field details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPut]
        [Route("update-field-detail/{fieldId}")]
        public IActionResult UpdateFieldDetail(Guid fieldId, FieldRequestViewModel model)
        {
            FieldResponseViewModel fieldViewModel = new() { FieldId = fieldId, Name = model.Name};
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + fieldId);
            if (!_fieldService.IsFieldExist(fieldId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FieldExistanceError + fieldId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FieldExistanceError, data = fieldId });
            }
            var response = _fieldService.UpdateFieldDetails(fieldViewModel);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = response });
        }



       

        /// <summary>
        ///delete field
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpDelete]
        [Route("delete-field/{fieldId}")]
        public IActionResult DeleteField(Guid fieldId)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + fieldId);
            if (!_fieldService.IsFieldExist(fieldId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FieldExistanceError + fieldId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FieldExistanceError, data = fieldId });
            }
            //if (!_fieldService.IsFieldAssigned(fieldId))
            //{
            //    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FieldDeleteError + fieldId);
            //    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FieldDeleteError, data = fieldId });
            //}
            var response = _fieldService.DeleteField(fieldId);
            if (response == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FieldDeleteError);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FieldDeleteError, data = response });
            }
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordDeleteSuccess, data = response });
        }




        /// <summary>
        /// get fields name list by search
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-fields-list")]
        public IActionResult GetFieldNameListWithSearch(string SearchKey)
        {

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(SearchKey);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _fieldService.GetFieldNameListWithSearch(SearchKey, UserMasterAdminId);
            var jsonStringResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonStringResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }



        [HttpPost]
        [Route("add-field-center")]
        public IActionResult AddFieldCenter(FieldCenterViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);

            if (!_fieldService.IsFieldExist(model.FieldId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FieldExistanceError + model.FieldId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FieldExistanceError, data = model.FieldId });
            }

            var locationJson = Newtonsoft.Json.JsonConvert.SerializeObject(model.Center);

            var response = _fieldService.AddFieldCenter(model, locationJson);

            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
        }




        [HttpPost]
        [Route("add-field-boundaries")]
        public IActionResult AddFieldBoundary(FieldBoundaryViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);

            if (!_fieldService.IsFieldExist(model.FieldId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FieldExistanceError + model.FieldId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FieldExistanceError, data = model.FieldId });
            }

            var locationJson = Newtonsoft.Json.JsonConvert.SerializeObject(model.Boundaries);

            var response = _fieldService.AddFieldBoundaries(model, locationJson);

            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
        }



        /// <summary>
        ///delete field boundaries
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpDelete]
        [Route("remove-field-boundaries/{fieldId}")]
        public IActionResult RemoveFieldBoundaries(Guid fieldId)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + fieldId);
            if (!_fieldService.IsFieldExist(fieldId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FieldExistanceError + fieldId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FieldExistanceError, data = fieldId });
            }
            var response = _fieldService.RemoveFieldBoundaries(fieldId);
            if (response == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FieldDeleteError);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FieldDeleteError, data = response });
            }
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordDeleteSuccess, data = response });
        }
    }
}
