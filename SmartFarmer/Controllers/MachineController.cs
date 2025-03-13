using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public class MachineController : ControllerBase
    {
        private static Logger _logger;
        private readonly IMachineService _machineService;
        private readonly IMachineTypeService _machineTypeService;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        private readonly IMasterService _masterService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MachineController(IMachineService machineService, IMachineTypeService machineTypeService, IFileService fileService, IUserService userService, IMasterService masterService, UserManager<ApplicationUser> userManager)
        {
            _machineService = machineService;
            _machineTypeService = machineTypeService;
            _fileService = fileService;
            _userService = userService;
            _masterService = masterService;
            _userManager = userManager;
        }

        /// <summary>
        ///add Machine into system 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("add-machine")]
        public IActionResult AddMachine(MachineRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);

            if (_machineTypeService.IsMachineTypeExist(model.MachineTypeId) == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineTypeExistenceError + model.MachineTypeId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineTypeExistenceError, data = model.MachineTypeId });
            }
            var createdBy = User.GetUserId();
            var response = _machineService.AddMachine(createdBy, model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            if (response != null)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
                return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
            }
            NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RecordAddError, data = null });

        }

        /// <summary>
        ///get Machine list by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-machine-list-by-search-with-pagination")]
        public IActionResult GetMachineListBySearchWithPagination(SearchMachineRequestViewModel model)
        {
            if (model.PageSize <= 0) model.PageSize = 1;
            if (model.PageNumber <= 0) model.PageNumber = 1;
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _machineService.GetMachineListBySearchWithPagination(false, LogInUser, UserMasterAdminId, model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        } 
        
        /// <summary>
        ///get Machine list by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("Find-machine-by-search-with-pagination")]
        public IActionResult FindMachineListBySearchWithPagination(SearchMachineRequestViewModel model)
        {
            if (model.PageSize <= 0) model.PageSize = 1;
            if (model.PageNumber <= 0) model.PageNumber = 1;
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            //var LogInUser = User.GetUserId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _machineService.GetMachineListBySearchWithPagination(true, LogInUser,UserMasterAdminId, model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get Machine details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-machine-details/{machineId}")]
        public IActionResult GetmachineIdDetails(Guid machineId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(machineId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (_machineService.IsMachineExist(machineId) == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineExistanceError + machineId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineExistanceError, data = machineId });
            }
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _machineService.GetMachineDetails(LogInUser,machineId, UserMasterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///update machine details
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPut]
        [Route("update-machine-detail/{machineId}")]
        public IActionResult UpdateMachineDetail(Guid machineId, MachineRequestViewModel model)
        {
            MachineResponseViewModel machineViewModel = new()
            {
                MachineId = machineId,
                NickName = model.NickName,
                Make = model.Make,
                Model = model.Model,
                SerialNumber = model.SerialNumber,
                Name = model?.Name,
                Description = model.Description,
                ManufacturedDate = model.ManufacturedDate,
                PurchaseDate = model.PurchaseDate,
                LOLERDate = model.LOLERDate,
                ServiceInterval = model.ServiceInterval,
                MachineTypeId = model.MachineTypeId,
                MOTDate = model.MOTDate,
                WorkingIn = model.WorkingIn,
                //Location = model?.Location,
                MachineCategoryId = model.MachineCategoryId,
                InSeason = model.InSeason,
                Archived = model.Archived,
            };
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + machineId);
            if (!_machineService.IsMachineExist(machineId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineExistanceError + machineId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineExistanceError, data = machineId });
            }
            if (_machineTypeService.IsMachineTypeExist(model.MachineTypeId) == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineTypeExistenceError + model.MachineTypeId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineTypeExistenceError, data = model.MachineTypeId });
            }

            var response = _machineService.UpdateMachineDetails(machineViewModel);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = response });
        }

        /// <summary>
        ///upload Machine Image
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPut]
        [Route("upload-machine-image")]
        public IActionResult UpdateMachineIamge(Guid machineId, [FromForm] AddFileViewModel model)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + machineId);
            if (!_machineService.IsMachineExist(machineId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineExistanceError + machineId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineExistanceError, data = machineId });
            }
            var previousFileName = _machineService.GetMachineImageFile(machineId);
            if (previousFileName.Name != null)
            {
                var fileResponse = _fileService.DeleteUploadFile(previousFileName.Name);
                if (fileResponse == null)
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(fileResponse);
                    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FileDeleteUnSuccessfull + json);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FileDeleteUnSuccessfull, data = fileResponse });
                }
            }
            var response = _fileService.UploadFile(model.File);
            if (response != null)
            {
                var res = _machineService.UpdateMachineImageFile(machineId, response.Name, response.FileLink);
                if (res != false)
                {
                    var jsonResponse2 = Newtonsoft.Json.JsonConvert.SerializeObject(res);
                    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse2);
                    return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = res });
                }
                NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FileUploadUnSuccessfull + res);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FileUploadUnSuccessfull, data = res });
            }
            var jsonResponse1 = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FileUploadUnSuccessfull + jsonResponse1);
            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FileUploadUnSuccessfull, data = response });

        }

        /// <summary>
        ///upload QR Image
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPut]
        [Route("upload-QR-image")]
        public IActionResult UpdateQRIamge(Guid machineId, long? machineCode,[FromForm] AddFileViewModel model)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + machineId);
            if (!_machineService.IsMachineExist(machineId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineExistanceError + machineId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineExistanceError, data = machineId });
            }
            var previousFileName = _machineService.GetMachineQRFile(machineId);
            if (previousFileName.Name != null)
            {
                var fileResponse = _fileService.DeleteUploadFile(previousFileName.Name);
                if (fileResponse == null)
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(fileResponse);
                    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FileDeleteUnSuccessfull + json);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FileDeleteUnSuccessfull, data = fileResponse });
                }
            }
            var response = _fileService.UploadFile(model.File);
            if (response != null)
            {
                var res = _machineService.UpdateQRFile(machineId, response.Name, response.FileLink, machineCode);
                if (res != false)
                {
                    var jsonResponse2 = Newtonsoft.Json.JsonConvert.SerializeObject(res);
                    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse2);
                    return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = res });
                }
                NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FileUploadUnSuccessfull + res);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FileUploadUnSuccessfull, data = res });
            }
            var jsonResponse1 = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FileUploadUnSuccessfull + jsonResponse1);
            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FileUploadUnSuccessfull, data = response });

        }

        /// <summary>
        ///get machine name list
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-machine-name-list")]
        public IActionResult GetMachineNameList()
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(null);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            // string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _machineService.GetMachineNameList(UserMasterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        /// update machine status
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize]
        [HttpPut]
        [Route("update-machine-status")]
        public IActionResult UpdateMachineStatus(UpdateMachineStatusRequestlViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (model.operatorId != null)
            {
                if (!_userService.IsOperatorExist(model.operatorId) )
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.OperatorIdIncorrectError + model.operatorId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.OperatorIdIncorrectError, data = model.operatorId });
                }
            }
            if (!_machineTypeService.IsMachineStatusExist(model.machineStatusId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineStatusExistenceError + model.machineStatusId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineStatusExistenceError, data = model.machineStatusId });
            }
            if (!_machineService.IsMachineExist(model.machineId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineExistanceError + model.machineId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineExistanceError, data = model.machineId });
            }
            var response = _machineService.UpdateMachineStatus(model.operatorId, model.machineId, model.ReasonOfServiceRemoval,model.machineStatusId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        /// assign machine to operator rom system for APP
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize(Roles = "Operator,Both")]
        [HttpPost]
        [Route("start-operating/{machineId}")]
        public IActionResult StartOperating(Guid machineId, string location)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + machineId);
            if (!_machineService.IsMachineExist(machineId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineExistanceError + machineId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineExistanceError, data = machineId });
            }
            var operatorId = User.GetUserId();
            //var user = _userManager.FindByIdAsync(operatorId).Result;
            //if (user != null)
            //{
            //    user.Location = location;
            //    _userManager.UpdateAsync(user).Wait();
            //}
            var masterAdminId = User.GetUserMasterAdminId();
            var response = _machineService.StartOperating(machineId, operatorId, location, masterAdminId);
            if (response == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineOperatingError + operatorId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineOperatingError, data = operatorId });
            }
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = response });
        }

        /// <summary>
        /// unassign machine to operator rom system for APP
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize(Roles = "Operator,Both")]
        [HttpPost]
        [Route("stop-operating/{machineId}")]
        public IActionResult StopOperating(Guid machineId, string location)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + machineId);
            if (!_machineService.IsMachineExist(machineId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineExistanceError + machineId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineExistanceError, data = machineId });
            }
            var operatorId = User.GetUserId();
            var user = _userManager.FindByIdAsync(operatorId).Result;
            //if (user != null)
            //{
            //    user.Location = location;
            //    _userManager.UpdateAsync(user).Wait();
            //}
            var response = _machineService.StopOperating(machineId, operatorId, location);
            if (response == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.OperatorOperatingLimitExcedes + operatorId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.OperatorOperatingLimitExcedes, data = operatorId });
            }
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = response });
        }

        /// <summary>
        ///update machine Working details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPut]
        [Route("update-machine-working-detail/{machineId}")]
        public IActionResult UpdateMachineWorkingDetail(Guid machineId, string WorkingIn)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + machineId);
            if (!_machineService.IsMachineExist(machineId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.MachineExistanceError + machineId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MachineExistanceError, data = machineId });
            }
            var response = _machineService.UpdateMachineWorkingDetails(machineId, WorkingIn);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.MachineUsageUpdateSuccess, data = response });
        }

        /// <summary>
        ///get Recent Machines for App
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize(Roles = "Operator,Both")]
        [HttpPost]
        [Route("get-recent-machines")]
        public IActionResult GetRecentMachineDetails(SearchMachineRequestViewModel model)
        {
            if (model.PageSize <= 0) model.PageSize = 1;
            if (model.PageNumber <= 0) model.PageNumber = 1;
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            // var UserId = User.GetUserId();
            // string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _machineService.GetRecentMachineDetails(LogInUser, UserMasterAdminId, model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get operator active Machines for App
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize(Roles = "Operator,Both")]
        [HttpPost]
        [Route("get-active-machines")]
        public IActionResult GetActiveMachineDetails()
        {
            var UserId = User.GetUserId();
            var response = _machineService.GetActiveMachineDetails(UserId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }



        /// <summary>
        /// scan machine by number
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("scan-machine")]
        public IActionResult GetMachineDetailSearch(string SearchKey)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(SearchKey);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _machineService.GetMachineDetailSearch(SearchKey, UserMasterAdminId);
            var jsonStringResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonStringResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        /// Inform create machine result unsafe
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [AllowAnonymous]
        [HttpPut]
        [Route("update-machine-result-unsafe")]
        public IActionResult UpdateMachineResultUnsafe()
        {
            var masterAdminUserIds = _userService.GetSystemMasterAdminIds();

            if (masterAdminUserIds != null)
            {
                int totalUpdatedCount = 0;

                foreach (var masterAdminId in masterAdminUserIds)
                {
 
                    int updatedCount = _machineService.UpdateMachineResultUnsafe(masterAdminId);
                    totalUpdatedCount += updatedCount; 
                }

                if (totalUpdatedCount > 0)
                {
                    NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response);
                    return Ok(new ResponseViewModel
                    {
                        Status = ResponseStatusType.Success.ToString(),
                        Message = $"{ResponseMessageConstants.RecordUpdateSuccess} {totalUpdatedCount}",
                        data = true
                    });
                }
                else
                {
                    NLog.LogManager.GetLogger("").Warn(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response);
                    return Ok(new ResponseViewModel
                    {
                        Status = ResponseStatusType.Error.ToString(),
                        Message = ResponseMessageConstants.RecordUpdateUnSuccessfull,
                        data = false
                    });
                }
            }
            NLog.LogManager.GetLogger("").Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RecordUpdateUnSuccessfull);
            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.MasterAdminExistenceError });
        }

    }
}
