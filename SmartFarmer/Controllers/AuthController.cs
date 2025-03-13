using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NLog;
using SmartFarmer.API.Extension;
using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.Service;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static SmartFarmer.Core.Common.Enums;

namespace SmartFarmer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static Logger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IFileService _fileService;
        private readonly IUserGroupService _userGroupService;
        private readonly IEventService _eventService;
        private readonly IFarmService _farmService;
        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IUserService userService, IFileService fileService, IUserGroupService userGroupService, IEventService eventService,IFarmService farmService)
        {
            _userManager = userManager;
            _userService = userService;
            _configuration = configuration;
            this.signInManager = signInManager;
            _roleManager = roleManager;
            _logger = LogManager.GetLogger("AuthController");
            _fileService = fileService;
            _userGroupService = userGroupService;
            _eventService = eventService;
            _farmService = farmService;
        }

        /// <summary>
        /// Login for  Application User.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>User Token, User Id and user role</returns>
        /// <response code="400">If the item is null</response>       
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {

            LoginViewModel loginViewModel = new() { Email = model.Email };
            var jsonString = JsonConvert.SerializeObject(loginViewModel);
            _logger.Info("Login" + Core.Common.Constants.Request + jsonString);
            var EmailUser = _userService.GetUserDetailByEmail(model.Email);
            if(EmailUser == null)
            {
                _logger.Error("Login" + Core.Common.Constants.Response + ResponseMessageConstants.LoginFailed);
                return Unauthorized(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.EmailIncorrectError });
            }
            ApplicationUser user;
            if(EmailUser.FarmId != null && EmailUser.ApplicationUserTypeId != (int)Core.Common.Enums.ApplicationUserTypeEnum.Unicorn)
            {
                var farmUser = await _userManager.FindByNameAsync(model.Email + EmailUser.FarmId);
                if (farmUser == null)
                {
                    _logger.Error("Login" + Core.Common.Constants.Response + ResponseMessageConstants.LoginFailed);
                    return Unauthorized(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserNameIncorrectError });
                }
                user = farmUser;
            }
            else
            {
                var simpleUser = await _userManager.FindByNameAsync(model.Email);
                if (simpleUser == null)
                {
                    _logger.Error("Login" + Core.Common.Constants.Response + ResponseMessageConstants.LoginFailed);
                    return Unauthorized(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.EmailIncorrectError });
                }
                user = simpleUser;
            }
            var userRole = _userService.GetUserRole(user.Id);

            var rolename = await _userManager.GetRolesAsync(user);
            var role = rolename.FirstOrDefault();
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Email, user.Email),
            };
                //string farmName= null;
                if (!string.Equals(role, "SuperAdmin", StringComparison.OrdinalIgnoreCase) && !string.Equals(role, "Unicorn", StringComparison.OrdinalIgnoreCase))
                {
                    authClaims.Add(new Claim(ClaimTypes.Actor, user.MasterAdminId.ToString()));
                    //farmName = _userService.GetFarmNameByMasterAdminId(user.MasterAdminId);
                }
                Farm farm = null;
                if (!string.Equals(role, "SuperAdmin", StringComparison.OrdinalIgnoreCase))
                {
                    farm = _userService.GetFarmByMasterAdminId(user.MasterAdminId);
                }
                
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                DateTime tokenExpireDateTime = DateTime.Now.AddDays(30);
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: tokenExpireDateTime,
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                ApplicationUserViewModel applicationUser = new() { ApplicationUserId = user.Id, FirstName = user.FirstName, LastName=user.LastName };
                var jsonStringResponse = JsonConvert.SerializeObject(applicationUser);
                _logger.Info("Login" + Core.Common.Constants.Response + jsonStringResponse);

                return Ok(new
                {

                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    userId = applicationUser.ApplicationUserId,
                    userRole = userRole,
                    userName = applicationUser.FirstName + " " + applicationUser.LastName,
                    Email = model.Email,
                    FarmId = farm?.FarmId,
                    FarmName = farm?.FarmName
                });
            }
            else
            {
                _logger.Error("Login" + Core.Common.Constants.Response + ResponseMessageConstants.PasswordIncorrectError);
                return Unauthorized(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.PasswordIncorrectError });
            }
        }

        /// <summary>
        /// Login for  operator and  both.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>User Token, User Id and user role</returns>
        /// <response code="400">If the item is null</response>       
        [HttpPost]
        [Route("app-login")]
        public async Task<IActionResult> AppLogin([FromBody] LoginViewModel model)
        {

            LoginViewModel loginViewModel = new() { Email = model.Email };
            var jsonString = JsonConvert.SerializeObject(loginViewModel);
            _logger.Info("Login" + Core.Common.Constants.Request + jsonString);
            var EmailUser = _userService.GetUserDetailByEmail(model.Email);
            if (EmailUser == null)
            {
                _logger.Error("Login" + Core.Common.Constants.Response + ResponseMessageConstants.LoginFailed);
                return Unauthorized(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.EmailIncorrectError });
            }
            ApplicationUser user;
            if (EmailUser.FarmId != null)
            {
                var farmUser = await _userManager.FindByNameAsync(model.Email + EmailUser.FarmId);
                if (farmUser == null)
                {
                    _logger.Error("Login" + Core.Common.Constants.Response + ResponseMessageConstants.LoginFailed);
                    return Unauthorized(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserNameIncorrectError });
                }
                user = farmUser;
            }
            else
            {
                var simpleUser = await _userManager.FindByNameAsync(model.Email);
                if (simpleUser == null)
                {
                    _logger.Error("Login" + Core.Common.Constants.Response + ResponseMessageConstants.LoginFailed);
                    return Unauthorized(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.EmailIncorrectError });
                }
                user = simpleUser;
            }
            var userRole = _userService.GetUserRole(user.Id);

            var rolename = await _userManager.GetRolesAsync(user);
            var role = rolename.FirstOrDefault();
            if (!string.Equals(role, "Operator", StringComparison.OrdinalIgnoreCase) && !string.Equals(role, "Both", StringComparison.OrdinalIgnoreCase))
            {
                _logger.Error("Login" + Core.Common.Constants.Response + ResponseMessageConstants.UserRoleNotAuthorized);
                return Unauthorized(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserRoleNotAuthorized });
            }
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Email, user.Email),
            };
                string farmName = null;
                if (!string.Equals(role, "SuperAdmin", StringComparison.OrdinalIgnoreCase))
                {
                    authClaims.Add(new Claim(ClaimTypes.Actor, user.MasterAdminId.ToString()));
                    farmName = _userService.GetFarmNameByMasterAdminId(user.MasterAdminId);
                }
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                DateTime tokenExpireDateTime = DateTime.Now.AddDays(30);
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: tokenExpireDateTime,
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                ApplicationUserViewModel applicationUser = new() { ApplicationUserId = user.Id, FirstName = user.FirstName, LastName = user.LastName };
                var jsonStringResponse = JsonConvert.SerializeObject(applicationUser);
                _logger.Info("Login" + Core.Common.Constants.Response + jsonStringResponse);

                return Ok(new
                {

                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    userId = applicationUser.ApplicationUserId,
                    userRole = userRole,
                    userName = applicationUser.FirstName + " " + applicationUser.LastName,
                    Email = model.Email,
                    FarmName = farmName
                });
            }
            else
            {
                _logger.Error("Login" + Core.Common.Constants.Response + ResponseMessageConstants.PasswordIncorrectError);
                return Unauthorized(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.PasswordIncorrectError });
            }
        }




        /// <summary>
        /// add user into the system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize]
        [HttpPost]
        [Route("add-user")]
        public async Task<IActionResult> AddUser([FromForm] AddUserViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (model.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.SuperAdmin || model.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.MasterAdmin)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.ApplicationUserTypeIdError });
            }
            string userId = User.GetUserId();
            var loginUser = _userManager.Users.Where(a => a.Id == userId).FirstOrDefault();

            var createdBy = User.GetUserId();
            var masterAdminId = User.GetUserMasterAdminId();
            var farmId = _farmService.GetLoginUserFarmId(masterAdminId);
            var userExists = _userService.CheckUserExistsInTheSameFarm(model.Email,masterAdminId);
            if (userExists != false)
            {
                NLog.LogManager.GetLogger("").Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response);
                return BadRequest( new ResponseViewModel { Status = "Error", Message = "Email already exists!" });
            }
            if (model.UserGroupId != null)
            {
                if (_userGroupService.IsUserGroupExist(model.UserGroupId.Value) == false)
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + model.UserGroupId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserGroupIdIncorrectError, data = model.UserGroupId });
                }
            }
            if (!_userService.IsUserTypeExist(model.ApplicationUserTypeId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + model.ApplicationUserTypeId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserTypeExistanceError, data = model.ApplicationUserTypeId });
            }

            FileViewModel fileResponse = new FileViewModel();
            if (model.ProfileImage != null)
            {
                fileResponse = _fileService.UploadFile(model.ProfileImage);
                if (fileResponse == null)
                {
                    var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(fileResponse);
                    NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FileUploadUnSuccessfull + jsonResponse);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FileUploadUnSuccessfull, data = fileResponse });
                }
            }
            ApplicationUser user = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email,
                Email = model.Email,
                ApplicationUserTypeId = model.ApplicationUserTypeId,
                ApplicationUserStatusId = (int)Core.Common.Enums.ApplicationUserStatusEnum.Live,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ProfileImageLink = fileResponse.FileLink,
                ProfileImageName = fileResponse.Name,
                Mobile = model.Mobile,
                HouseNameNumber = model.HouseNameNumber,
                Street = model.Street,
                Addressline2 = model.Addressline2,
                Town = model.Town,
                County = model.County,
                PostCode = model.PostCode,
                CreatedDate = DateTime.Now,
                OperatorStatusId = null,
                UserGroupId = model?.UserGroupId,
                //CreatedBy = createdBy,

            };
            if (model.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Operator || model.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Both)
            {
                user.OperatorStatusId = (int)Core.Common.Enums.OperatorStatusEnum.Idle;
            }
            if (loginUser.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.MasterAdmin)
            {
                user.CreatedBy = createdBy;
                user.MasterAdminId = loginUser.Id;
                user.MainAdminId = loginUser.Id;
            }
            if (loginUser.ApplicationUserTypeId != (int)Core.Common.Enums.ApplicationUserTypeEnum.MasterAdmin && loginUser.ApplicationUserTypeId != (int)Core.Common.Enums.ApplicationUserTypeEnum.SuperAdmin)
            {
                user.CreatedBy = createdBy;
                user.MasterAdminId = loginUser.MasterAdminId;
                user.MainAdminId = loginUser.MasterAdminId;
            }
            var FarmId = _farmService.GetLoginUserFarmId(loginUser.MasterAdminId);
            var otheruser = _userService.GetUserDetailByEmail(model.Email);
            if (otheruser != null)
            {
                user.FarmId = otheruser.FarmId;
            }
            else
            {
                user.FarmId = _farmService.GetLoginUserFarmId(loginUser.MasterAdminId);
            }
            user.UserName = model.Email + FarmId;
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded != true)
            {
                var deletingServerFile = _fileService.DeleteUploadFile(user.ProfileImageName);
                NLog.LogManager.GetLogger("").Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + false);
                return BadRequest( new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.AccountCreationFail });
            }
            if (model.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Operator)
            {
                await _userManager.AddToRoleAsync(user, Enums.ApplicationUserTypeEnum.Operator.ToString());
            }
            if (model.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Admin)
            {
                await _userManager.AddToRoleAsync(user, Enums.ApplicationUserTypeEnum.Admin.ToString());
            }
            if (model.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Portal)
            {
                await _userManager.AddToRoleAsync(user, Enums.ApplicationUserTypeEnum.Portal.ToString());
            }
            if (model.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Both)
            {
                await _userManager.AddToRoleAsync(user, Enums.ApplicationUserTypeEnum.Both.ToString());
            }
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.UserCreationSucess, data = user.Id });
        }

        /// <summary>
        ///get user list by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize]
        [HttpPost]
        [Route("get-user-list-by-search-with-pagination")]
        public IActionResult GetUserListBySearchWithPagination(SearchUserRequestViewModel model)
        {

            if (model.PageSize <= 0) model.PageSize = 1;
            if (model.PageNumber <= 0) model.PageNumber = 1;
            string loginUserId = User.GetUserId();
            var masterAdminId = _userService.GetMasterAdminId(loginUserId);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var response = _userService.GetUserListBySearchWithPagination(model, masterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get user details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize]
        [HttpGet]
        [Route("get-user-details/{userId}")]
        public IActionResult GetUserdetails(string userId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(userId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var response = _userService.GetUserDetails(userId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }
        /// <summary>
        ///get user check In details for App
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize]
        [HttpGet]
        [Route("get-user-checkIn-details/{userId}")]
        public IActionResult GetUserChcekIndetails(string userId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(userId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var response = _userService.GetUserChcekInDetails(userId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });

        }
        /// <summary>
        ///check welfare alarm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [AllowAnonymous]
        [HttpGet]
        [Route("run-welfare-check")]
        public IActionResult CheckWelfareAlarm( string securityKey)
        {
            //string loginUserId = User.GetUserId();
            //var masterAdminId = _userService.GetMasterAdminId(loginUserId);
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request);

            if (securityKey == _configuration["TaskScheduler:SecurityKey"])
            {
                var masterAdminUserIds = _userService.GetSystemMasterAdminIds();
                foreach (var masterAdminId in masterAdminUserIds)
                {
                    _userService.CheckWelfareAlarm(masterAdminId);
                }
                NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response );
                return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess });

            }
            NLog.LogManager.GetLogger("").Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RecordGetUnSuccess);
            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.SecurityKeyBadRequest });

        }
        /// <summary>
        ///check welfare alarm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize]
        [HttpGet]
        [Route("check-welfare-alarm-app")]
        public IActionResult CheckWelfareForApp()
        {
            string loginUserId = User.GetUserId();
            var masterAdminId = _userService.GetMasterAdminId(loginUserId);
            var response = _userService.CheckWelfareForApp(loginUserId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }
        /// <summary>
        ///Inform for welfare alarm app
        /// </summary>
        /// <param name="securityKey"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [AllowAnonymous]
        [HttpGet]
        [Route("Create-welfare-alarm-app")]
        public IActionResult CreateWelfareForApp(string securityKey)
        {
            //string loginUserId = User.GetUserId();
            //var masterAdminId = _userService.GetMasterAdminId(loginUserId);
            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request);

            if (securityKey == _configuration["TaskScheduler:SecurityKey"])
            {
                var masterAdminUserIds = _userService.GetSystemMasterAdminIds();
                foreach (var masterAdminId in masterAdminUserIds)
                {
                     _userService.CreateWelfareForApp(masterAdminId);
                }
                NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response);
                return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess });

            }
            NLog.LogManager.GetLogger("").Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RecordGetUnSuccess);
            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.SecurityKeyBadRequest });

        }
        /// <summary>
        ///check alarm for web
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize]
        [HttpGet]
        [Route("check-alarm")]
        public IActionResult CheckWelfareForWeb()
        {
            string loginUserId = User.GetUserId();
            var masterAdminId = _userService.GetMasterAdminId(loginUserId);
            var response = _userService.CheckWelfareForWeb(masterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///check welfare alarm for app
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize]
        [HttpGet]
        [Route("cancle-welfare-alarm-app")]
        public IActionResult CancleWelfareForApp()
        {
            string loginUserId = User.GetUserId();
            var masterAdminId = _userService.GetMasterAdminId(loginUserId);
            var response = _userService.CancleWelfareForApp(loginUserId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }
        ///// <summary>
        /////cancel welfare alarm
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        ///// <response code="400">If the item is null</response>
        //[Authorize]
        //[HttpGet]
        //[Route("cancel-welfare-alarm-web")]
        //public IActionResult cancelWelfareAlarmWeb()
        //{
        //    string loginUserId = User.GetUserId();
        //    var masterAdminId = _userService.GetMasterAdminId(loginUserId);
        //    var response = _userService.CancleWelfareForWeb(masterAdminId);
        //    var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
        //    return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        //}
        /// <summary>
        ///cancel welfare alarm web
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize]
        [HttpGet]
        [Route("cancel-alarm-from-web")]
        public IActionResult CheckWelfareAlarmWeb( Guid EventId)
        {
            string loginUserId = User.GetUserId();
            var masterAdminId = _userService.GetMasterAdminId(loginUserId);
            if (!_eventService.IsEventExist(EventId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.EventExistanceError + EventId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.EventExistanceError, data = EventId });
            }
            var response = _userService.CancleWelfareFromWeb(EventId,loginUserId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = response });
        }
        /// <summary>
        ///update user details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize]
        [HttpPut]
        [Route("update-user-details")]
        public async Task<IActionResult> updateUserdetails(ApplicationUserRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (_userService.IsUserExist(model.ApplicationUserId) == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.UserExistanceError);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserExistanceError });
            }
            if (_userService.IsUserTypeExist(model.ApplicationUserTypeId) == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.UserTypeExistanceError);
                return BadRequest( new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserTypeExistanceError });
            }
            if (_userService.IsUserStatusExist(model.ApplicationUserStatusId) == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.UserStatusExistanceError);
                return BadRequest( new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserStatusExistanceError });
            }
            if (model.UserGroupId != null)
            {
                if (_userGroupService.IsUserGroupExist(model.UserGroupId.Value) == false)
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + model.UserGroupId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserGroupIdIncorrectError, data = model.UserGroupId });
                }
            }
            var response = _userService.UpdateUserDetails(model);
            var user = await _userManager.FindByIdAsync(model.ApplicationUserId);
            if (model.NewPassword != null)
            {
                var remove = await _userManager.RemovePasswordAsync(user);
                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (!result.Succeeded)
                {
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.PasswordChangeError });
                }
                
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRoleAsync(user, userRoles.FirstOrDefault());
            if (model.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Operator)
            {
                await _userManager.AddToRoleAsync(user, Enums.ApplicationUserTypeEnum.Operator.ToString());
            }
            if (model.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Admin)
            {
                await _userManager.AddToRoleAsync(user, Enums.ApplicationUserTypeEnum.Admin.ToString());
            }
            if (model.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Portal)
            {
                await _userManager.AddToRoleAsync(user, Enums.ApplicationUserTypeEnum.Portal.ToString());
            }
            if (model.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Both)
            {
                await _userManager.AddToRoleAsync(user, Enums.ApplicationUserTypeEnum.Both.ToString());
            }
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get user name list
        /// </summary>
        ///  <param name="searchKey"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize]
        [HttpGet]
        [Route("get-user-name-list")]
        public IActionResult GetUserNameList(string? searchKey)
        {
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(null);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var response = _userService.GetUserNameList(UserMasterAdminId, searchKey);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///update user profileImage
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize]
        [HttpPut]
        [Route("update-user-profileImage")]
        public async Task<IActionResult> updateUserProfileImage(string applicationUserId, IFormFile profileImage)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(applicationUserId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (_userService.IsUserExist(applicationUserId) == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.UserExistanceError);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserExistanceError });
            }
            var previousFile = _userService.GetProfileImage(applicationUserId);
            if (previousFile != null)
            {
                var deletingServerFile = _fileService.DeleteUploadFile(previousFile.Name);
            }
            var fileResponse = _fileService.UploadFile(profileImage);
            if (fileResponse == null)
            {
                var jsonResponse1 = Newtonsoft.Json.JsonConvert.SerializeObject(fileResponse);
                NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FileUploadUnSuccessfull + jsonResponse1);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FileUploadUnSuccessfull, data = fileResponse });
            }
            ApplicationUserProfileImageViewModel userProfileViewModel = new()
            {
                ApplicationUserId = applicationUserId,
                ProfileImageLink = fileResponse.FileLink,
                ProfileImageName = fileResponse.Name,
            };
            var res = _userService.UpdateProfileImage(userProfileViewModel);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(res);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = res });
        }

        /// <summary>
        ///update user location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize]
        [HttpPut]
        [Route("update-user/{location}")]
        public IActionResult UpdateUserLocation(string location)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(location);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var userId = User.GetUserId();
            var response = _userService.UpdateUserLocation(userId, location);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = response });
        }

        /// <summary>
        ///get operator and both users details
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize]
        [HttpGet]
        [Route("get-operator-both-user-details")]
        public IActionResult GetOperatorBothUserDetails()
        {
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(null);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var response = _userService.GetOperatorBothUserDetails(UserMasterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///update user details for app
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize]
        [HttpPut]
        [Route("update-user-detail-app/{userId}")]
        public async Task<IActionResult> UpdateUserDetailApp(string userId, UpdateUserRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (!_userService.IsUserExist(userId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.UserExistanceError + userId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserExistanceError + userId });
            }
            //if (!_userService.IsUserStatusExist(model.ApplicationUserStatusId))
            //{
            //    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.UserStatusExistanceError);
            //    return BadRequest( new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserStatusExistanceError });
            //}
            var response = _userService.UpdateUserDetailApp(userId,model);
            //if (model.NewPassword != null)
            //{
            //    var user = await _userManager.FindByIdAsync(userId);
            //    var remove = await _userManager.RemovePasswordAsync(user);
            //    var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
            //    if (!result.Succeeded)
            //    {
            //        return BadRequest( new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.PasswordChangeError });
            //    }
            //}

            if (response == null)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RecordUpdateUnSuccessfull + response);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RecordUpdateUnSuccessfull, data = response });
            }
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }


        //[Authorize]
        //[HttpPost]
        //[Route("add-farm-user")]
        //public async Task<IActionResult> AddFarmUser([FromForm] AddUserViewModel model)
        //{
        //    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
        //    NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);

        //    if (model.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.SuperAdmin || model.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.MasterAdmin)
        //    {
        //        NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response);
        //        return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.ApplicationUserTypeIdError });
        //    }
        //    string userId = User.GetUserId();
        //    var loginUser = _userManager.Users.Where(a => a.Id == userId).FirstOrDefault();
        //    var createdBy = User.GetUserId();

        //    var loginUserFarmId = _farmService.GetLoginUserFarmId(User.GetUserMasterAdminId());

        //    var existingUser = _userManager.Users.FirstOrDefault(u => u.Email == model.Email);
        //    if (existingUser != null)
        //    {
        //        if (model.UserGroupId != null)
        //        {
        //            if (_userGroupService.IsUserGroupExist(model.UserGroupId.Value) == false)
        //            {
        //                NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + model.UserGroupId);
        //                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserGroupIdIncorrectError, data = model.UserGroupId });
        //            }
        //        }
        //        if (!_userService.IsUserTypeExist(model.ApplicationUserTypeId))
        //        {
        //            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + model.ApplicationUserTypeId);
        //            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserTypeExistanceError, data = model.ApplicationUserTypeId });
        //        }
        //        var existingMapping = _userService.GetFarmOperatorMapping(loginUserFarmId, existingUser.Id);
        //        if (existingMapping != null)
        //        {
        //            existingMapping.ApplicationUserTypeId = model.ApplicationUserTypeId;
        //            existingMapping.UserGroupId = model.UserGroupId;
        //            _userService.UpdateFarmOpeartorMapping(existingMapping);

        //            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response);
        //            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.MappingUpdateSuccess });
        //        }
        //        else
        //        {
        //            var newMapping = new FarmOperatorMapping
        //            {
        //                OperatorId = existingUser.Id,
        //                FarmId = loginUserFarmId,
        //                UserGroupId = model.UserGroupId,
        //                ApplicationUserTypeId = model.ApplicationUserTypeId,
        //                CreatedDate = DateTime.Now
        //            };
        //            var mappingViewModel = Mapper.MapFarmOperatorMappingEntityToCreateFarmOperatorMappingViewModel(newMapping);
        //            _userService.CreateFarmOpeartorMapping(mappingViewModel);

        //            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response);
        //            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.MappingCreateSuccess });
        //        }
        //    }

        //    if (model.UserGroupId != null)
        //    {
        //        if (_userGroupService.IsUserGroupExist(model.UserGroupId.Value) == false)
        //        {
        //            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + model.UserGroupId);
        //            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserGroupIdIncorrectError, data = model.UserGroupId });
        //        }
        //    }
        //    if (!_userService.IsUserTypeExist(model.ApplicationUserTypeId))
        //    {
        //        NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + model.ApplicationUserTypeId);
        //        return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserTypeExistanceError, data = model.ApplicationUserTypeId });
        //    }
        //    FileViewModel fileResponse = new FileViewModel();
        //    if (model.ProfileImage != null)
        //    {
        //        fileResponse = _fileService.UploadFile(model.ProfileImage);
        //        if (fileResponse == null)
        //        {
        //            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(fileResponse);
        //            NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.FileUploadUnSuccessfull + jsonResponse);
        //            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.FileUploadUnSuccessfull, data = fileResponse });
        //        }
        //    }
        //    var user = new ApplicationUser
        //    {

        //        SecurityStamp = Guid.NewGuid().ToString(),
        //        UserName = model.Email,
        //        Email = model.Email,
        //        ApplicationUserTypeId = model.ApplicationUserTypeId,
        //        ApplicationUserStatusId = (int)Core.Common.Enums.ApplicationUserStatusEnum.Live,
        //        FirstName = model.FirstName,
        //        LastName = model.LastName,
        //        ProfileImageLink = fileResponse.FileLink,
        //        ProfileImageName = fileResponse.Name,
        //        Mobile = model.Mobile,
        //        HouseNameNumber = model.HouseNameNumber,
        //        Street = model.Street,
        //        Addressline2 = model.Addressline2,
        //        Town = model.Town,
        //        County = model.County,
        //        PostCode = model.PostCode,
        //        CreatedDate = DateTime.Now,
        //        OperatorStatusId = null,
        //        UserGroupId = model?.UserGroupId,
        //    };

        //    if (model.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Operator || model.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Both)
        //    {
        //        user.OperatorStatusId = (int)Core.Common.Enums.OperatorStatusEnum.Idle;
        //    }
        //    if (loginUser.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.MasterAdmin)
        //    {
        //        user.CreatedBy = createdBy;
        //        user.MasterAdminId = loginUser.Id;
        //        user.MainAdminId = loginUser.Id;
        //    }
        //    if (loginUser.ApplicationUserTypeId != (int)Core.Common.Enums.ApplicationUserTypeEnum.MasterAdmin && loginUser.ApplicationUserTypeId != (int)Core.Common.Enums.ApplicationUserTypeEnum.SuperAdmin)
        //    {
        //        user.CreatedBy = createdBy;
        //        user.MasterAdminId = loginUser.MasterAdminId;
        //        user.MainAdminId = loginUser.MasterAdminId;
        //    }
        //    var result = await _userManager.CreateAsync(user, model.Password);
        //    if (result.Succeeded != true)
        //    {
        //        var deletingServerFile = _fileService.DeleteUploadFile(user.ProfileImageName);
        //        NLog.LogManager.GetLogger("").Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + false);
        //        return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.AccountCreationFail });
        //    }
        //    if (model.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Operator)
        //    {
        //        await _userManager.AddToRoleAsync(user, Enums.ApplicationUserTypeEnum.Operator.ToString());
        //    }
        //    if (model.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Admin)
        //    {
        //        await _userManager.AddToRoleAsync(user, Enums.ApplicationUserTypeEnum.Admin.ToString());
        //    }
        //    if (model.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Portal)
        //    {
        //        await _userManager.AddToRoleAsync(user, Enums.ApplicationUserTypeEnum.Portal.ToString());
        //    }
        //    if (model.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Both)
        //    {
        //        await _userManager.AddToRoleAsync(user, Enums.ApplicationUserTypeEnum.Both.ToString());
        //    }

        //    var mapping = new FarmOperatorMapping
        //    {
        //        OperatorId = user.Id,
        //        FarmId = loginUserFarmId,
        //        UserGroupId = model.UserGroupId,
        //        ApplicationUserTypeId = model.ApplicationUserTypeId,
        //        CreatedDate = DateTime.Now
        //    };
        //    var createMappingViewModel = Mapper.MapFarmOperatorMappingEntityToCreateFarmOperatorMappingViewModel(mapping);
        //    _userService.CreateFarmOpeartorMapping(createMappingViewModel);


        //    NLog.LogManager.GetLogger("").Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response);
        //    return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(),Message = $"{ResponseMessageConstants.UserCreationSucess} and {ResponseMessageConstants.MappingCreateSuccess}",
        //        data = new
        //        {
        //            UserId = user.Id,
        //        }
        //    });
        //}

        /// <summary>
        ///get user details
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize]
        [HttpGet]
        [Route("get-user-detail/{email}")]
        public IActionResult GetUserDetailByEmail(string email)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(email);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var userExists = _userService.CheckUserEmailExistence(email);
            if (userExists == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.EmailExistanceError + email);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.EmailExistanceError, data = email });
            }
            var response = _userService.GetUserDetailByEmail(email);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get exiting user details of anoter farm
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize]
        [HttpGet]
        [Route("get-existing-user-detail-from-another-farm/{email}")]
        public IActionResult GetExistingUserDetailByEmail(string email)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(email);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var LoginUserMasterAdminId = User.GetUserMasterAdminId();
            var userExists = _userService.CheckUserExistsInTheSameFarm(email, LoginUserMasterAdminId);
            if (userExists == true)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.EmailExistanceError + email);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.EmailExistanceError, data = email });
            }
            var response = _userService.GetExistingUserDetailByEmail(email, LoginUserMasterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get user  list
        /// </summary>
        ///  <param name=""></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize]
        [HttpGet]
        [Route("get-users-list/{trainingRecordId}")]
        public IActionResult GetUsersList(Guid trainingRecordId)
        {
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(null);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var response = _userService.GetUsersList(UserMasterAdminId,trainingRecordId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }
    }
}
