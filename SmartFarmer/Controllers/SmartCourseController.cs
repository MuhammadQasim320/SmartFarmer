//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using SmartFarmer.API.Extension;
//using SmartFarmer.Core.Common;
//using SmartFarmer.Core.Interface;
//using SmartFarmer.Core.ViewModel;
//using static SmartFarmer.Core.Common.Enums;

//namespace SmartFarmer.API.Controllers
//{
//    [Authorize]
//    [Route("api/[controller]")]
//    [ApiController]
//    public class SmartCourseController : ControllerBase
//    {
//        private ISmartCourseService _smartCourseService;
//        private readonly IUserService _userService;

//        public SmartCourseController(ISmartCourseService smartCourseService, IUserService userService)
//        {
//            _smartCourseService = smartCourseService;
//            _userService = userService;
//        }

//        /// <summary>
//        ///add smart Course into system
//        /// </summary>
//        /// <param name="model"></param>
//        /// <returns></returns>
//        /// <response code="400">If the item is null</response>
//        [HttpPost]
//        [Route("add-smart-course")]
//        public IActionResult AddSmartCourse(SmartCourseRequestViewModel model)
//        {
//            SmartCourseViewModel smartCourseView = new() { Name = model.Name, SmartCourseId = Guid.NewGuid() };
//            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
//            var CreatedBy = User.GetUserId();
//            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
//            var response = _smartCourseService.AddSmartCourse(CreatedBy,smartCourseView);
//            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
//            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
//            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
//        }

//        /// <summary>
//        ///get smart course list by search with pagination
//        /// </summary>
//        /// <param name="model"></param>
//        /// <returns></returns>
//        /// <response code="400">If the item is null</response>
//        [HttpPost]
//        [Route("get-smart-course-list-by-search-with-pagination")]
//        public IActionResult GetSmartCourseListBySearchWithPagination(SmartCourseSearchRequestViewModel model)
//        {
//            if (model.PageSize <= 0) model.PageSize = 1;
//            if (model.PageNumber <= 0) model.PageNumber = 1;
//            string UserMasterAdminId = User.GetUserMasterAdminId();
//            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
//            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
//            var response = _smartCourseService.GetSmartCourseListBySearchWithPagination(UserMasterAdminId,model);
//            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
//            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
//            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
//        }

//        /// <summary>
//        ///get smart Course details
//        /// </summary>
//        /// <param name="model"></param>
//        /// <returns></returns>
//        /// <response code="400">If the item is null</response>
//        [HttpGet]
//        [Route("get-smart-course-detail/{smartCourseId}")]
//        public IActionResult GetSmartCourseDetail(Guid smartCourseId)
//        {

//            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + smartCourseId);
//            if (!_smartCourseService.IsSmartCourseExist(smartCourseId))
//            {
//                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.SmartCourseExistenceError + smartCourseId);
//                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.SmartCourseExistenceError, data = smartCourseId });
//            }
//            var response = _smartCourseService.GetSmartCourseDetails(smartCourseId);
//            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
//            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
//            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
//        }

//        /// <summary>
//        ///update smart Course details
//        /// </summary>
//        /// <param name="model"></param>
//        /// <returns></returns>
//        /// <response code="400">If the item is null</response>
//        [HttpPut]
//        [Route("update-smart-course-detail/{smartCourseId}")]
//        public IActionResult UpdateSmartCourseDetail(Guid smartCourseId, SmartCourseRequestViewModel model)
//        {
//            SmartCourseViewModel smartCourseView = new() { Name = model.Name, SmartCourseId = smartCourseId };
//            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + smartCourseId);
//            if (!_smartCourseService.IsSmartCourseExist(smartCourseId))
//            {
//                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.SmartCourseExistenceError + smartCourseId);
//                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.SmartCourseExistenceError, data = smartCourseId });
//            }
//            var response = _smartCourseService.UpdateSmartCourseDetails(smartCourseView);
//            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
//            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
//            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = response });
//        }

//        /// <summary>
//        ///delete smart Course
//        /// </summary>
//        /// <param name="smartCourseId"></param>
//        /// <returns></returns>
//        /// <response code="400">If the item is null</response>
//        [HttpDelete]
//        [Route("delete-smartCourse/{smartCourseId}")]
//        public IActionResult DeleteSmartCourse(Guid smartCourseId)
//        {
//            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + smartCourseId);
//            if (!_smartCourseService.IsSmartCourseExist(smartCourseId))
//            {
//                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.SmartCourseExistenceError + smartCourseId);
//                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.SmartCourseExistenceError, data = smartCourseId });
//            }
//            var response = _smartCourseService.DeleteSmartCourse(smartCourseId);
//            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
//            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
//            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordDeleteSuccess, data = response });
//        }
        
//        /// <summary>
//        ///get smartcourse name list
//        /// </summary>
//        /// <param name=""></param>
//        /// <returns></returns>
//        /// <response code="400">If the item is null</response>
//        [HttpGet]
//        [Route("get-smartcourse-name-list")]
//        public IActionResult GetSmartCourseNameList()
//        {
//            string UserMasterAdminId = User.GetUserMasterAdminId();
//            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request);
//            var response = _smartCourseService.GetSmartCourseNameList(UserMasterAdminId);
//            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
//            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
//            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordDeleteSuccess, data = response });
//        }
//    }
//}
