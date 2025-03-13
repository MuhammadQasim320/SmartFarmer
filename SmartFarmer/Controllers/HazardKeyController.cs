using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class HazardKeyController : ControllerBase
    {
        private readonly IHazardKeyService _hazardKeyService;
        private readonly IFieldService _fieldService;
        private readonly IMasterService _masterService;
        private readonly IUserService _userService;

        public HazardKeyController(IHazardKeyService hazardKeyService, IFieldService fieldService, IMasterService masterService, IUserService userService)
        {
            _hazardKeyService = hazardKeyService;
            _fieldService = fieldService;
            _masterService = masterService;
            _userService = userService;
        }

        /// <summary>
        ///add hazardKey into system 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("add-hazardKey")]
        public IActionResult AddHazardKey(HazardKeyRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var CreatedBy = User.GetUserId();
                if (!_masterService.IsHazardTypeExist(model.HazardTypeId))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.HazardTypeExistenceError + model.HazardTypeId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.HazardTypeExistenceError, data = model.HazardTypeId });
                }
            var response = _hazardKeyService.AddHazardKey(CreatedBy, model);
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
        ///update hazardKey details by hazardKeyId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPut]
        [Route("update-hazardKey-detail/{hazardKeyId}")]
        public IActionResult UpdateHazardKeyDetail(Guid hazardKeyId, HazardKeyRequestViewModel model)
        {
            HazardKeyResponseViewModel hazardKey = new() { HazardKeyId = hazardKeyId, Name = model.Name, Color = model.Color, HazardTypeId = model.HazardTypeId };
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + hazardKeyId);
            if (!_hazardKeyService.IsHazardKeyExist(hazardKeyId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.HazardKeyExistenceError + hazardKeyId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.HazardKeyExistenceError, data = hazardKeyId });
            }
            if (!_masterService.IsHazardTypeExist(model.HazardTypeId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.HazardTypeExistenceError + model.HazardTypeId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.HazardTypeExistenceError, data = model.HazardTypeId });
            }
            var response = _hazardKeyService.UpdateHazardKeyDetails(hazardKey);
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
        ///delete hazardKey
        /// </summary>
        /// <param name="hazardKeyId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpDelete]
        [Route("delete-hazardKey/{hazardKeyId}")]
        public IActionResult DeleteHazardKey(Guid hazardKeyId)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + hazardKeyId);
            if (!_hazardKeyService.IsHazardKeyExist(hazardKeyId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.HazardKeyExistenceError + hazardKeyId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.HazardKeyExistenceError, data = hazardKeyId });
            } 
            var response = _hazardKeyService.DeleteHazardKey(hazardKeyId);
            if (response == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.HazardKeyDeleteError);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.HazardKeyDeleteError, data = response });
            }
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordDeleteSuccess, data = response });
        }


        /// <summary>
        ///get hazardKey list by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-hazardKey-list-by-search-with-pagination")]
        public IActionResult GetUserHazardKeyBySearchWithPagination(SearchHazardKeyRequestViewModel model)
        {
            if (model.PageSize <= 0) model.PageSize = 1;
            if (model.PageNumber <= 0) model.PageNumber = 1;
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var response = _hazardKeyService.GetHazardKeyListBySearchWithPagination(UserMasterAdminId, model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }



        /// <summary>
        ///get hazardKey details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-hazardKey-details/{hazardKeyId}")]
        public IActionResult GetHazardKeyDetails(Guid hazardKeyId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(hazardKeyId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (!_hazardKeyService.IsHazardKeyExist(hazardKeyId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.HazardKeyExistenceError + hazardKeyId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.HazardKeyExistenceError, data = hazardKeyId });
            }
            var response = _hazardKeyService.GetHazardKeyDetails(hazardKeyId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }


        [HttpPost]
        [Route("add-hazardKey-field-locations")]
        public IActionResult AddHazardKeyFieldLocations(HazardKeyFieldViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);

            if (!_hazardKeyService.IsHazardKeyExist(model.HazardKeyId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.HazardKeyExistenceError + model.HazardKeyId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.HazardKeyExistenceError, data = model.HazardKeyId });
            }

            if (!_fieldService.IsFieldExist(model.FieldId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FieldExistanceError + model.FieldId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FieldExistanceError, data = model.FieldId });
            }

            var locationJson = Newtonsoft.Json.JsonConvert.SerializeObject(model.Locations);

            var response = _hazardKeyService.AddHazardKeyFieldLocations(model, locationJson);

            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
        }





        /// <summary>
        /// get hazardKey name list by search
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-hazards-list")]
        public IActionResult GetHazardKeyNameListWithSearch(string SearchKey)
        {

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(SearchKey);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _hazardKeyService.GetHazardKeyNameListWithSearch(SearchKey, UserMasterAdminId);
            var jsonStringResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonStringResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }



        [HttpGet]
        [Route("get-field-all-hazards/{fieldId}")]
        public IActionResult GetFieldHazardKeys(Guid fieldId)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + fieldId);

            if (!_fieldService.IsFieldExist(fieldId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FieldExistanceError + fieldId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FieldExistanceError, data = " " });
            }
            var response = _hazardKeyService.GetFieldHazardKeys(fieldId);
            FieldHazardListViewModel model = new() { List = response };
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + model);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }



        /// <summary>
        /// remove hazardkey from field from system
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Client</returns>
        /// <response code="201">Returns the newly created item </response>
        /// <response code="400">If the item is null</response>  
        [HttpDelete]
        [Route("remove-hazardKey-from-field")]
        public async Task<IActionResult> RemoveHazardKeyField(Guid hazardKeyFieldMappingId)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + hazardKeyFieldMappingId);

                if (!_hazardKeyService.IsHazardKeyFieldMappingExist(hazardKeyFieldMappingId))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + hazardKeyFieldMappingId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.HazardKeyFieldMappingExistError, data = "" });
                }

                var response = _hazardKeyService.RemoveHazardKeyField(hazardKeyFieldMappingId);
                if (response == false)
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.HazardKeyFieldMappingExistError);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.HazardKeyFieldMappingExistError });
                }

            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RecordDeleteSuccess);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordDeleteSuccess });
        }





        ///delete hazardKey field location
        /// </summary>
        /// <param name="hazardKeyFieldMappingId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpDelete]
        [Route("remove-hazardKey-field-location/{hazardKeyFieldMappingId}")]
        public IActionResult RemoveHazardKeyFieldLocation(Guid hazardKeyFieldMappingId)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + hazardKeyFieldMappingId);
            if (!_hazardKeyService.IsHazardKeyFieldMappingExist(hazardKeyFieldMappingId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + hazardKeyFieldMappingId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.HazardKeyFieldMappingExistError, data = "" });
            }
            var response = _hazardKeyService.RemoveHazardKeyFieldLocation(hazardKeyFieldMappingId);
            if (response == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.HazardKeyFieldDeleteError);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.HazardKeyFieldDeleteError, data = response });
            }
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordDeleteSuccess, data = response });
        }


        ///delete field all locations
        /// </summary>
        /// <param name="hazardKeyFieldMappingId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpDelete]
        [Route("remove-field-locations/{fieldId}")]
        public IActionResult RemoveFieldLocation(Guid fieldId)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + fieldId);
            if (!_fieldService.IsFieldExist(fieldId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FieldExistanceError + fieldId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FieldExistanceError, data = " " });
            }
            var response = _hazardKeyService.RemoveFieldLocation(fieldId);
            if (response == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.HazardKeyFieldDeleteError);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.HazardKeyFieldDeleteError, data = response });
            }
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordDeleteSuccess, data = response });
        }



        /// <summary>
        ///add hazardKey locations by fieldId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("add-hazardKey-locations-by/{fieldId}")]
        public IActionResult AddHazardKeyLocationsByField(Guid fieldId, HazardKeyLocationListViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var responses = new List<HazardFieldResponseViewModel>();

            foreach (var hazardKeyLocation in model.List)
            {
                if (!_hazardKeyService.IsHazardKeyExist(hazardKeyLocation.HazardKeyId))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.HazardKeyExistenceError + hazardKeyLocation.HazardKeyId);
                    return BadRequest(new ResponseViewModel
                    {
                        Status = ResponseStatusType.Error.ToString(),
                        Message = ResponseMessageConstants.HazardKeyExistenceError,
                        data = hazardKeyLocation.HazardKeyId
                    });
                }

                var locationJson = Newtonsoft.Json.JsonConvert.SerializeObject(hazardKeyLocation.Locations);

                var response = _hazardKeyService.AddHazardKeyLocationsByField(fieldId, hazardKeyLocation.HazardKeyId, locationJson);

                responses.Add(response);
                var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);

            }
            return Ok(new ResponseViewModel
            {
                Status = ResponseStatusType.Success.ToString(),
                Message = ResponseMessageConstants.RecordAddSuccess,
                data = responses
            });

        }


    }
}
