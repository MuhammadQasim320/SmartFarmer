using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class TimeSheetController : ControllerBase
    {
        private readonly ITimeSheetService _timeSheetService;
        private readonly IUserService _userService;

        public TimeSheetController(ITimeSheetService timeSheetService,IUserService userService)
        {
            _timeSheetService = timeSheetService;
            _userService = userService;
        }


        /// <summary>
        ///get TimeSheet list by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-timeSheet-list-by-search-filter")]
        public IActionResult GetTimeSheetList(TimeSheetRequestViewModel model)
        {
            //if (model.PageSize <= 0) model.PageSize = 1;
            //if (model.PageNumber <= 0) model.PageNumber = 1;
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            string loginUserId = User.GetUserId();
            var masterAdminId = _userService.GetMasterAdminId(loginUserId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var response = _timeSheetService.GetTimeSheetList(masterAdminId,model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get TimeSheet list by search with pagination for APP
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [Authorize(Roles = "Operator, Both")]
        [HttpPost]
        [Route("get-timeSheet-list")]
        public IActionResult GetTimeSheetListBy(TimeSheetAppRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            string loginUserId = User.GetUserId();
            var masterAdminId = _userService.GetMasterAdminId(loginUserId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var response = _timeSheetService.GetTimeSheetListAPP(loginUserId, model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

    }
}
