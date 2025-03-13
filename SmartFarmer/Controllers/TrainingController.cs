using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog.Filters;
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
    public class TrainingController : ControllerBase
    {
        private IFileService _fileService;
        private ITrainingFileService _trainingFileService;
        private ITrainingService _trainingService;
        //private ISmartCourseService _smartCourseService;
        private readonly IUserService _userService;

        public TrainingController(ITrainingService trainingService, /*ISmartCourseService smartCourseService,*/ IFileService fileService, ITrainingFileService trainingFileService, IUserService userService)
        {
            _trainingService = trainingService;
            //_smartCourseService = smartCourseService;
            _fileService = fileService;
            _trainingFileService = trainingFileService;
            _userService = userService;
        }

        /// <summary>
        ///add training into system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("add-training")]
        public async Task<IActionResult> AddTraining(TrainingRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var CreatedBy = User.GetUserId();
            var response = _trainingService.AddTraining(CreatedBy,model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
        }

        /// <summary>
        ///get training list by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-training-list-by-search-with-pagination")]
        public IActionResult GetTrainingListBySearchWithPagination(TrainingSearchRequestViewModel model)
        {
            if (model.PageSize <= 0) model.PageSize = 1;
            if (model.PageNumber <= 0) model.PageNumber = 1;
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            if (model.TrainingTypeId != null)
            {
                if (!_trainingService.IsTrainingTypeExist(model.TrainingTypeId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingTypeExistanceError + model.TrainingTypeId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingTypeExistanceError, data = model.TrainingTypeId });
                }
            }
            if (model.FilterId != null)
            {
                if (model.FilterId !=1 && model.FilterId !=2)
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingFilterIdExistanceError + model.FilterId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingFilterIdExistanceError, data = model.FilterId });
                }
            }
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var response = _trainingService.GetTrainingListBySearchWithPagination(UserMasterAdminId,model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get training list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-training-list")]
        public IActionResult GetTrainingList()
        {
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _trainingService.GetTrainingList(UserMasterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get training details by trainingId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-training-detail/{trainingId}")]
        public IActionResult GetTrainingDetail(Guid trainingId)
        {

            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + trainingId);
            if (!_trainingService.IsTrainingExist(trainingId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingExistenceError + trainingId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingExistenceError, data = trainingId });
            }
            var response = _trainingService.GetTrainingDetails(trainingId);
            var response2 = _trainingFileService.GetTrainingFiles(trainingId);
            TrainingDetailViewModel details = new TrainingDetailViewModel();
            details.Data = response;
            details.TrainingFilesList = response2;

            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(details);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = details });
        }

        /// <summary>
        ///update training details by trainingId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPut]
        [Route("update-training-detail/{trainingId}")]
        public IActionResult UpdateTrainingDetail(Guid trainingId, UpdateTrainingRequestViewModel model)
        {
            UpdateTrainingViewModel trainingView = new() { Name = model.Name, TrainingId = trainingId, Description = model.Description, Link = model.Link, Validity = model.Validity, Certification = model.Certification, Expires = model.Expires };
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + trainingId);
            if (!_trainingService.IsTrainingExist(trainingId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingExistenceError + trainingId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingExistenceError, data = trainingId });
            }
            //if (model.SmartCourseId != null)
            //{
            //    if (!_smartCourseService.IsSmartCourseExist(model.SmartCourseId.Value))
            //    {
            //        NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.SmartCourseExistenceError + model.SmartCourseId);
            //        return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.SmartCourseExistenceError, data = model.SmartCourseId });
            //    }
            //}
            //if (!_trainingService.IsTrainingTypeExist(model.TrainingTypeId))
            //{
            //    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingTypeExistanceError + model.TrainingTypeId);
            //    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingTypeExistanceError, data = model.TrainingTypeId });
            //}
            var response = _trainingService.UpdateTrainingDetails(trainingView);
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
        ///upload training file by trainingId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("upload-training-file/{trainingId}")]
        public IActionResult UploadTrainingFile(Guid trainingId, [FromForm] AddFileViewModel model)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + trainingId);
            if (!_trainingService.IsTrainingExist(trainingId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingExistenceError + trainingId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingExistenceError, data = trainingId });
            }
            var fileResponse = _fileService.UploadFile(model.File);
            if (fileResponse != null)
            {
                TrainingFileViewModel trainingFile = new() { TrainingId = trainingId, FileUrl = fileResponse.FileLink, FileUniqueName = fileResponse.Name , FileName = model.FileName };
                var response = _trainingFileService.UploadTrainingFile(trainingFile);
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
        ///delete training file by trainingId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpDelete]
        [Route("delete-training-file/{trainingFileId}")]
        public IActionResult DeleteTrainingFile(Guid trainingFileId)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + trainingFileId);
            if (!_trainingFileService.IsTrainingFileExist(trainingFileId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingFileExistenceError + trainingFileId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingFileExistenceError, data = trainingFileId });
            }
            var res = _trainingFileService.GetTrainingFile(trainingFileId);
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
            var response = _trainingFileService.DeleteTrainingFile(trainingFileId);
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
        /// assign training to operator into system
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Client</returns>
        /// <response code="201">Returns the newly created item </response>
        /// <response code="400">If the item is null</response>  
        [HttpPost]
        [Route("assign-training-to-operator")]
        public async Task<IActionResult> AssignTraining(Guid trainingId, string operatorId)
        {
            var trainingIdJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(trainingId);
            var operatorIdJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(operatorId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + "trainingId:" + trainingIdJsonString + "operatorId:" + operatorIdJsonString);
            if (!_trainingService.IsTrainingExist(trainingId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingExistenceError + trainingId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingExistenceError, data = trainingId });
            }
                if (!_userService.IsUserExist(operatorId))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.UserExistanceError + operatorId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserExistanceError, data = " " });
                }

                if (_trainingService.IsTrainingAssignedToOperator(trainingId, operatorId))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingOperatorExistanceError + trainingId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingOperatorExistanceError, data = " " });
                }

                var response = _trainingService.AssignTrainingToOperator(trainingId, operatorId);
                if (response == null)
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RecordAddError);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RecordAddError });
                }
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
        }

        /// <summary>
        ///get operator training list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-operator-trainings/{operatorId}")]
        public IActionResult GetOperatorTrainings(string operatorId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(operatorId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);

            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _trainingService.GetOperatorTrainings(operatorId);
            TrainingOperatorListViewModel model = new() { List = response };
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }
        /// <summary>
        ///get operator training list for app
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-operator-all-trainings/{operatorId}")]
        public IActionResult GetOperatorAllTrainings(string operatorId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(operatorId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);

            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _trainingService.GetOperatorTrainingsAndTrainingRecord(operatorId);
         //   TrainingOperatorListViewModel model = new() { List = response };
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }
            ///// <summary>
            /////get operator questions and answers 
            ///// </summary>
            ///// <param name="model"></param>
            ///// <returns></returns>
            ///// <response code="400">If the item is null</response>
            //[HttpGet]
            //[Route("get-operator-questions-and-answers")]
            //public IActionResult GetOperatorQuestionAndAnswers()
            //{
            //    string UserId = User.GetUserId();
            //    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(UserId);
            //    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            //    var response = _trainingService.GetOperatorTrainingsAndTrainingRecord(UserId);
            //    //OperatorTrainingAndTrainingRecord model = new() { List = response };
            //    var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            //    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            //    return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
            //}

        /// <summary>
        /// add / update training question answers into system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPut]
        [Route("add-update-training-question-answers/{trainingId}")]
        public async Task<IActionResult> AddTrainingQuestionAnswers(Guid trainingId , AddQuestionsRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var CreatedBy = User.GetUserId();

            if (!_trainingService.IsTrainingExist(trainingId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingExistenceError + trainingId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingExistenceError, data = trainingId });
            }

            foreach (var question in model.Questions)
            {
                var correctAnswersCount = question.Answers.Count(a => a.IsCorrect);

                if (correctAnswersCount != 1)
                {


                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + $"Question: '{question.QuestionText}' {ResponseMessageConstants.AnswerCorrectError}");
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = $"Question: '{question.QuestionText}' {ResponseMessageConstants.AnswerCorrectError}" });
                }
            }

            var response = _trainingService.AddTrainingQuestionAnswers(CreatedBy,trainingId, model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
        }

        /// <summary>
        ///get training questions list for BackOffice
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-training-questions/{trainingId}")]
        public IActionResult GetTrainingQuestions(Guid trainingId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(trainingId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);

            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _trainingService.GetTrainingQuestions(trainingId,UserMasterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get training questions list for App
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-training-questions-app/{trainingId}")]
        public IActionResult GetAppTrainingQuestions(Guid trainingId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(trainingId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);

            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _trainingService.GetTrainingQuestionsApp(trainingId, UserMasterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get operator questions answers list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-operator-questions-answers/{trainingId}")]
        public IActionResult GetAppTrainingQuestionsAnswers(Guid trainingId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(trainingId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);

            string UserId = User.GetUserId();
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _trainingService.GetTrainingQuestionsAnswers(trainingId, UserId, UserMasterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get operator questions result list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-operator-questions-result/{trainingId}")]
        public IActionResult GetOperatorQuestionsResult(Guid trainingId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(trainingId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);

            string UserId = User.GetUserId();
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            var response = _trainingService.GetOperatorQuestionsResult(trainingId, UserId, UserMasterAdminId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        /// update training archive status from system
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Client</returns>
        /// <response code="201">Returns the newly created item </response>
        /// <response code="400">If the item is null</response>  
        [HttpPut]
        [Route("archive-training")]
        public async Task<IActionResult> UpdateTrainingArchiveStatus(Guid trainingId, bool Archive)
        {
            TrainingArchiveViewModel model = new TrainingArchiveViewModel() { TrainingId = trainingId, Archive = Archive };
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (!_trainingService.IsTrainingExist(trainingId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingExistenceError + trainingId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingExistenceError, data = " " });
            }

            var response = _trainingService.UpdateTrainingArchiveStatus(trainingId, Archive);
            if (response != null)
            {

                var jsonStringResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonStringResponse);
                return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = response });
            }

            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RecordUpdateUnSuccessfull);
            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateUnSuccessfull });
        }

        /// <summary>
        /// update questions order
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Client</returns>
        /// <response code="201">Returns the newly created item </response>
        /// <response code="400">If the item is null</response>  
        [HttpPut]
        [Route("update-questions-order")]
        public async Task<IActionResult> UpdateQuestionsOrder(IEnumerable<UpdateQuestionOrderViewModel> models)
        {
            var requestModels = models.Select(a => Mapper.MapUpdateQuestionOrderViewModelToQuestionResponseViewModel(a)).ToList();

            UpdateQuestionOrderListViewModel orderListViewModel = new()
            {
                Questions = models
            };
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(orderListViewModel);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var response = _trainingService.UpdateQuestionsOrder(requestModels).ToList();
            AddQuestionsResponseViewModel question = new() { Questions = response };
            if (response.Count() != 0)
            {
                var jsonStringResponse = Newtonsoft.Json.JsonConvert.SerializeObject(question);
                NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonStringResponse);
                return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateSuccess, data = response });
            }
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RecordUpdateUnSuccessfull);
            return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUpdateUnSuccessfull });
        }

        /// Training Record

        /// <summary>
        ///add training record into system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("add-training-record")]
        public async Task<IActionResult> AddTrainingRecord(TrainingRecordRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            if (model.TrainingTypeId == (int)Core.Common.Enums.TrainigTypeEnum.SmartFarmer)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingTypeIdError);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingTypeIdError});
            }
            var CreatedBy = User.GetUserId();
            var response = _trainingService.AddTrainingRecord(CreatedBy, model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
        }

        /// <summary>
        ///update training record details by trainingRecordId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPut]
        [Route("update-training-record-detail/{trainingRecordId}")]
        public IActionResult UpdateTrainingRecordDetail(Guid trainingRecordId, UpdateTrainingRecordRequestViewModel model)
        {
            UpdateTrainingRecordViewModel trainingRecordView = new() { Name = model.Name, TrainingRecordId = trainingRecordId, Expires = model.Expires, Validity = model.Validity, Certification = model.Certification, Qualification = model.Qualification, Archived = model.Archived , CompletedDate=model.CompletedDate,TrainingTypeId=model.TrainingTypeId, Description=model.Description};
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + trainingRecordId);
            if (!_trainingService.IsTrainingRecordExist(trainingRecordId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingRecordExistenceError + trainingRecordId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingRecordExistenceError, data = trainingRecordId });
            }
            if (model.TrainingTypeId == (int)Core.Common.Enums.TrainigTypeEnum.SmartFarmer)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingTypeIdError);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingTypeIdError });
            }
            var response = _trainingService.UpdateTrainingRecordDetails(trainingRecordView);
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
        ///get training record list by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("get-training-record-list-by-search-with-pagination")]
        public IActionResult GetTrainingRecordListBySearchWithPagination(TrainingRecordSearchRequestViewModel model)
        {
            if (model.PageSize <= 0) model.PageSize = 1;
            if (model.PageNumber <= 0) model.PageNumber = 1;
            //string UserMasterAdminId = User.GetUserMasterAdminId();
            var LogInUser = User.GetUserId();
            var UserMasterAdminId = _userService.GetMasterAdminId(LogInUser);
            if (model.TrainingTypeId != null)
            {
                if (!_trainingService.IsTrainingTypeExist(model.TrainingTypeId.Value))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingTypeExistanceError + model.TrainingTypeId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingTypeExistanceError, data = model.TrainingTypeId });
                }
            }
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var response = _trainingService.GetTrainingRecordListBySearchWithPagination(UserMasterAdminId, model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///get training record details by trainingRecordId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-training-record-detail/{trainingRecordId}")]
        public IActionResult GetTrainingRecordDetail(Guid trainingRecordId)
        {

            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + trainingRecordId);
            if (!_trainingService.IsTrainingRecordExist(trainingRecordId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingRecordExistError + trainingRecordId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingRecordExistError, data = trainingRecordId });
            }
            var response = _trainingService.GetTrainingRecordDetails(trainingRecordId);

            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        /// assign trainingRecord to operator into system
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Client</returns>
        /// <response code="201">Returns the newly created item </response>
        /// <response code="400">If the item is null</response>  
        [HttpPost]
        [Route("assign-training-record-to-operator")]
        public async Task<IActionResult> AssignTrainingrecord(Guid trainingRecordId, List<string> operatorIds)
        {

            var trainingRecordIdJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(trainingRecordId);
            var operatorIdsJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(operatorIds);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + "trainingRecordId:" + trainingRecordIdJsonString + "operatorIds:" + operatorIdsJsonString);
            if (!_trainingService.IsTrainingRecordExist(trainingRecordId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingRecordExistError + trainingRecordId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingRecordExistError, data = trainingRecordId });
            }
            foreach (var operatorId in operatorIds)
            {
                if (!_userService.IsUserExist(operatorId))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.UserExistanceError + operatorId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserExistanceError, data = operatorId });
                }

                if (_trainingService.IsTrainingRecordAssignedToOperator(trainingRecordId, operatorId))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingRecordOperatorExistanceError);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingRecordOperatorExistanceError });
                }

                var response = _trainingService.AssignTrainingRecordToOperator(trainingRecordId, operatorId);
                if (response == null)
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RecordAddError);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.RecordAddError });
                }
            }

            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RecordAddSuccess);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess });
        }



        /// <summary>
        /// unassign operator romm training record from system
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Client</returns>
        /// <response code="201">Returns the newly created item </response>
        /// <response code="400">If the item is null</response>  
        [HttpPost]
        [Route("unassign-operator-from-training-record")]
        public async Task<IActionResult> UnAssignTrainingrecord(Guid trainingRecordId, List<string> operatorIds)
        {

            var trainingRecordIdJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(trainingRecordId);
            var operatorIdsJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(operatorIds);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + "trainingRecordId:" + trainingRecordIdJsonString + "operatorIds:" + operatorIdsJsonString);
            if (!_trainingService.IsTrainingRecordExist(trainingRecordId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingRecordExistError + trainingRecordId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingRecordExistError, data = trainingRecordId });
            }
            foreach (var operatorId in operatorIds)
            {
                if (!_userService.IsUserExist(operatorId))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.UserExistanceError + operatorId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserExistanceError, data = operatorId });
                }

                if (!_trainingService.IsTrainingRecordAssignedToOperator(trainingRecordId, operatorId))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + trainingRecordId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UnassignOperatortoTrainingError });
                }

                var response = _trainingService.UnAssignTrainingRecordToOperator(trainingRecordId, operatorId);
                if (response == false)
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.UnassignOperatortoTrainingError);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UnassignOperatortoTrainingError });
                }
            }

            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.RecordUnassignSuccess);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordUnassignSuccess });
        }

        /// <summary>
        /// get operators by trainingRecordId
        /// </summary>
        ///  <param name="trainingRecordId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>

        [Authorize]
        [HttpGet]
        [Route("get-operator-trainingRecord/{trainingRecordId}")]
        public IActionResult GetTrainingRecordOperators(Guid trainingRecordId)
        {
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + trainingRecordId);

            if (!_trainingService.IsTrainingRecordExist(trainingRecordId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.TrainingRecordExistenceError + trainingRecordId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.TrainingRecordExistenceError, data = " " });
            }
            var response = _trainingService.GetTrainingRecordOperators(trainingRecordId);
            TrainingRecordoperatorListViewModel model = new() { List = response };
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + model);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordGetSuccess, data = response });
        }

        /// <summary>
        ///add operator  answers into system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("add-operator-answers")]
        public async Task<IActionResult> AddOperatorAnswers( AddOperatorQuestionsRequestViewModel model)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            var operatorId = User.GetUserId();
            foreach (var question in model.Questions)
            {
                if (!_trainingService.IsQuestionExist(question.QuestionId))
                {
                    NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.QuestionExistenceError + question.QuestionId);
                    return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.QuestionExistenceError, data = question.QuestionId });
                }
            }
            if (!_trainingService.IsUserTypeCorrect(operatorId))
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + ResponseMessageConstants.UserTypeIncorrect + operatorId);
                return BadRequest(new ResponseViewModel { Status = ResponseStatusType.Error.ToString(), Message = ResponseMessageConstants.UserTypeIncorrect, data = operatorId });
            }
            var response = _trainingService.AddOperatorAnswers(operatorId, model);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel { Status = ResponseStatusType.Success.ToString(), Message = ResponseMessageConstants.RecordAddSuccess, data = response });
        }




        /// <summary>
        ///get operator answer into system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [Route("get-result/{trainingId}/{operatorId}")]
        public async Task<IActionResult> GetOperatorAnswersResult(Guid trainingId , string operatorId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(trainingId);
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Request + jsonString);
            //var operatorId = User.GetUserId();
            var response = _trainingService.GetOperatorAnswersResult(trainingId,operatorId);
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            if (response == false)
            {
                NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
                return Ok(new ResponseViewModel
                {
                    Status = ResponseStatusType.Error.ToString(),
                    Message = ResponseMessageConstants.RecordGetSuccess,
                    data = new { Result = "fail" }
                });
            }
          
            NLog.LogManager.GetLogger(User.Identity.Name).Info(System.Reflection.MethodBase.GetCurrentMethod().Name + Core.Common.Constants.Response + jsonResponse);
            return Ok(new ResponseViewModel
            {
                Status = ResponseStatusType.Success.ToString(),
                Message = ResponseMessageConstants.RecordGetSuccess,
                data = new { Result = "pass" } 
            });

        }
    }
}
