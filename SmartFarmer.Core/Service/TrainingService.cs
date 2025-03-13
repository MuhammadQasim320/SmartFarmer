using Microsoft.AspNetCore.Http.HttpResults;
using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;
using System.Reflection;

namespace SmartFarmer.Core.Service
{
    public class TrainingService : ITrainingService
    {
        private ITrainingRepository _trainingRepository;
        public TrainingService(ITrainingRepository trainingRepository)
        {
            _trainingRepository = trainingRepository;
        }
        /// <summary>
        /// Add Training
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public TrainingViewModel AddTraining(string CreatedBy, TrainingRequestViewModel model)
        {
            // return Mapper.MapTrainingEntityToTrainingViewModel(_trainingRepository.AddTraining(Mapper.MapTrainingRequestViewModelToTrainingEntity(CreatedBy,model)));
            DateTime? dueDate = _trainingRepository.CalculateDueDate(model.Validity);

            var training = Mapper.MapTrainingRequestViewModelToTrainingEntity(CreatedBy, model);

            training.DueDate = dueDate;

            var addTraining = _trainingRepository.AddTraining(training);

            return Mapper.MapTrainingEntityToTrainingViewModel(addTraining);


        }

        /// <summary>
        /// get Training deatils 
        /// </summary>
        /// <param name="trainingId"></param>
        /// <returns></returns>
        public TrainingViewModel GetTrainingDetails(Guid trainingId)
        {
           var training= Mapper.MapTrainingEntityToTrainingViewModel(_trainingRepository.GetTrainingDetails(trainingId));
            if (training.Expires == true && training.Validity != null)
            {
                var today = DateTime.Today;
                if (training.DueDate.Value <= today)
                {
                    training.Expired = true;
                }
            }
            return training;
        }

        /// <summary>
        /// Get Training List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public TrainingListViewModel GetTrainingList(string UserMasterAdminId)
        {
            TrainingListViewModel trainingListViewModel = new();
            trainingListViewModel.TrainingList = _trainingRepository.GetTrainingList(UserMasterAdminId).Select(a => Mapper.MapTrainingEntityToTrainingNameListViewModel(a))?.ToList();
            return trainingListViewModel;
        }

        /// <summary>
        /// Get Training List By Search With Pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public TrainingSearchResponseViewModel GetTrainingListBySearchWithPagination(string UserMasterAdminId, TrainingSearchRequestViewModel model)
        {
            TrainingSearchResponseViewModel trainingSearchResponse = new();
            trainingSearchResponse.List = _trainingRepository.GetTrainingListBySearch(model.PageNumber, model.PageSize, model.SearchKey, model.TrainingTypeId, UserMasterAdminId,model.FilterId)?.Select(a => Mapper.MapTrainingEntityToTrainingWithFileURLsResponseViewModel(a))?.ToList();
            trainingSearchResponse.TotalCount = _trainingRepository.GetTrainingCountBySearch(model.SearchKey, model.TrainingTypeId, UserMasterAdminId, model.FilterId);

            foreach (var training in trainingSearchResponse.List)
            {
                if (training.Expires == true && training.Validity !=null)
                {
                    var today = DateTime.Today;
                   if(training.DueDate.Value <= today)
                    {
                        training.Expired = true;
                    }
                }
            }


            return trainingSearchResponse;
        }

        /// <summary>
        /// Is Training Exist
        /// </summary>
        /// <param name="trainingId"></param>
        /// <returns></returns>
        public bool IsTrainingExist(Guid trainingId)
        {
            return _trainingRepository.IsTrainingExist(trainingId);
        }

        /// <summary>
        /// Is Training Type Exist
        /// </summary>
        /// <param name="trainingTypeId"></param>
        /// <returns></returns>
        public bool IsTrainingTypeExist(int trainingTypeId)
        {
            return _trainingRepository.IsTrainingTypeExist(trainingTypeId);
        }

        /// <summary>
        /// Update Training Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public UpdateTrainingViewModel UpdateTrainingDetails(UpdateTrainingViewModel model)
        {
           // return Mapper.MapTrainingEntityToUpdateTrainingViewModel(_trainingRepository.UpdateTrainingDetails(Mapper.MapUpdateTrainingViewModelToTrainingEntity(model)));



            DateTime? dueDate = _trainingRepository.CalculateDueDate(model.Validity);

            var training = new Training
            {
                TrainingId = model.TrainingId,
                Name = model.Name,
                Expires = model.Expires,
                Validity = model.Validity,
                Certification = model.Certification,
                Description = model.Description,
                TrainingTypeId = model.TrainingTypeId,
                Link = model.Link,
                DueDate = dueDate,
            };
            var updatedTraining = _trainingRepository.UpdateTrainingDetails(training);

            return Mapper.MapTrainingEntityToUpdateTrainingViewModel(updatedTraining);
        }

        /// <summary>
        /// add training question answers
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>s
        public AddQuestionsResponseViewModel AddTrainingQuestionAnswers(string createdBy, Guid trainingId, AddQuestionsRequestViewModel request)
        {
            AddQuestionsResponseViewModel addQuestionsResponseViewModel = new AddQuestionsResponseViewModel();
            foreach (var question in request.Questions)
            {
                WebQuestionResponseViewModel item = new WebQuestionResponseViewModel();
                if (question.SmartQuestionId == null)
                {
                    item = Mapper.MapSmartQuestionEntityToQuestionResponseViewModel(_trainingRepository.AddTrainingQuestionAnswers(trainingId, Mapper.MapQuestionRequestViewModelToQuestionEntity(createdBy, trainingId, question)));
                }
                else
                {
                    item = Mapper.MapSmartQuestionEntityToQuestionResponseViewModel(_trainingRepository.UpdateTrainingQuestionAnswers(trainingId, Mapper.MapNewQuestionRequestViewModelToQuestionEntity(createdBy, trainingId, question)));
                }
                addQuestionsResponseViewModel.TrainingId = trainingId;
                addQuestionsResponseViewModel.Questions.Add(item);
            }
            return addQuestionsResponseViewModel;
        }



        /// <summary>
        /// Get Training List By Search With Pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<WebQuestionResponseViewModel> GetTrainingQuestions(Guid trainingId, string UserMasterAdminId)
        {

         var   trainingQuestionResponse= _trainingRepository.GetTrainingQuestions(trainingId, UserMasterAdminId)?.Select(a => Mapper.MapQuestionEntityToWebQuestionResponseViewModel(a))?.ToList();
            return trainingQuestionResponse;
        }

        /// <summary>
        /// Get Training List By Search With Pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<QuestionResponseViewModel> GetTrainingQuestionsApp(Guid trainingId, string UserMasterAdminId)
        {

            var trainingQuestionResponse = _trainingRepository.GetTrainingQuestions(trainingId, UserMasterAdminId)?.Select(a => Mapper.MapQuestionEntityToQuestionResponseViewModelApp(a))?.ToList();
            return trainingQuestionResponse;
        }

        /// <summary>
        /// Get Operator Questions Answers
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<QuestionResponseViewModel> GetTrainingQuestionsAnswers(Guid trainingId, string UserId, string UserMasterAdminId)
        {
            var trainingQuestionResponse = _trainingRepository.GetTrainingQuestions(trainingId, UserMasterAdminId)?.Select(a => Mapper.MapQuestionEntityToQuestionResponseViewModelApp(a))?.ToList();

            foreach (var trainingQuestion in trainingQuestionResponse)
            {
                foreach (var answer in trainingQuestion.Answers)
                {
                       var answerFound = _trainingRepository.GetQuestionsAnswers( answer.AnswerId, UserId);
                       answer.OperatorAnswered = answerFound;
                };
            };
            return trainingQuestionResponse;
        }

        /// <summary>
        /// Get Operator Questions Answers
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<QuestionResponseViewModel> GetOperatorQuestionsResult(Guid trainingId, string UserId, string UserMasterAdminId)
        {
            var trainingQuestionResponse = _trainingRepository.GetTrainingQuestions(trainingId, UserMasterAdminId)?
                .Select(a => Mapper.MapQuestionResultEntityToQuestionResultResponseViewModelApp(a))?.ToList();

            bool allAnswersCorrect = true;

            foreach (var trainingQuestion in trainingQuestionResponse)
            {
                bool questionCorrect = true;

                foreach (var answer in trainingQuestion.Answers)
                {
                    var answerFound = _trainingRepository.GetQuestionsAnswers(answer.AnswerId, UserId);

                    answer.OperatorAnswered = answerFound != null && answerFound == true;
                    if (answer.IsCorrect != answer.OperatorAnswered)
                    {
                        questionCorrect = false; 
                        allAnswersCorrect = false;  
                        break; 
                    }
                }
                if (!allAnswersCorrect)
                {
                    break;
                }
            }

            return trainingQuestionResponse;
        }


        /// <summary>
        /// update training archive status
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateTrainingArchiveStatus(Guid trainingId, bool archive)
        {
            return _trainingRepository.UpdateTrainingArchiveStatus(trainingId, archive);
        }

        /// <summary>
        /// update training questions order
        /// </summary>
        /// <param></param>
        /// <returns></returns>s
        public IEnumerable<WebQuestionResponseViewModel> UpdateQuestionsOrder(IEnumerable<WebQuestionResponseViewModel> models)
        {
            return _trainingRepository.UpdateQuestionsOrder(models.Select(a => Mapper.MapSmartQuestionResponseViewModelToSmartQuestionEntity(a))).Select(a => Mapper.MapQuestionEntityToQuestionResponseViewModel(a)).ToList();
        }



        public bool IsQuestionExist(Guid questionId)
        {
            return _trainingRepository.IsQuestionExist(questionId);
        }



        /// <summary>
        /// Add Training record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// 
        public TrainingRecordViewModel AddTrainingRecord(string CreatedBy, TrainingRecordRequestViewModel model)
        {
            DateTime? dueDate = _trainingRepository.CalculateDueDate(model.Validity);

            var trainingRecordEntity = Mapper.MapTrainingRecordRequestViewModelToTrainingRecordEntity(CreatedBy, model);

            trainingRecordEntity.DueDate = dueDate;

            var savedEntity = _trainingRepository.AddTrainingRecord(trainingRecordEntity);

            return Mapper.MapTrainingRecordEntityToTrainingRecordViewModel(savedEntity);
        }



        /// <summary>
        /// Update Training record Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public UpdateTrainingRecordViewModel UpdateTrainingRecordDetails(UpdateTrainingRecordViewModel model)
        {
            DateTime? dueDate = _trainingRepository.CalculateDueDate(model.Validity);

            var trainingRecordEntity = new TrainingRecord
            {
                TrainingRecordId = model.TrainingRecordId,
                Name = model.Name,
                Expires = model.Expires,
                Validity = model.Validity,
                Certification = model.Certification,
                Qualification = model.Qualification,
                Archived = model.Archived,
                CompletedDate = model.CompletedDate,
                DueDate = dueDate,
                TrainingTypeId = model.TrainingTypeId, 
                Description = model.Description, 
            };
            var updatedEntity = _trainingRepository.UpdateTrainingRecordDetails(trainingRecordEntity);

            return Mapper.MapTrainingRecordEntityToUpdateTrainingRecordViewModel(updatedEntity);
        }
      



        /// <summary>
        /// Is Training record Exist
        /// </summary>
        /// <param name="trainingRecordId"></param>
        /// <returns></returns>
        public bool IsTrainingRecordExist(Guid trainingRecordId)
        {
            return _trainingRepository.IsTrainingRecordExist(trainingRecordId);
        }




        /// <summary>
        /// Get Training record List By Search With Pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public TrainingRecordSearchResponseViewModel GetTrainingRecordListBySearchWithPagination(string UserMasterAdminId, TrainingRecordSearchRequestViewModel model)
        {
            TrainingRecordSearchResponseViewModel trainingRecordSearchResponse = new();
            trainingRecordSearchResponse.List = _trainingRepository.GetTrainingRecordListBySearch(model.PageNumber, model.PageSize, model.SearchKey, model.TrainingTypeId,model.Archived,model.Expired, UserMasterAdminId)?.Select(a => Mapper.MapTrainingRecordEntityToTrainingRecordsViewModel(a))?.ToList();
            trainingRecordSearchResponse.TotalCount = _trainingRepository.GetTrainingRecordCountBySearch(model.SearchKey, model.TrainingTypeId, model.Archived, model.Expired, UserMasterAdminId);

            foreach (var record in trainingRecordSearchResponse.List)
            {
                if (record.Expires == true && record.Validity != null)
                {
                    var today = DateTime.Today;
                    var dueThreshold = today.AddDays(30);

                    if (record.DueDate.Value < today)
                    {
                        record.Expired = true;
                    }
                    else if (record.DueDate.Value >= today && record.DueDate.Value <= dueThreshold)
                    {
                        record.Due = true;
                    }
                }
            }
            return trainingRecordSearchResponse;
        }


        /// <summary>
        /// get Training record deatils 
        /// </summary>
        /// <param name="trainingRecordId"></param>
        /// <returns></returns>
        public UpdateTrainingRecordViewModel GetTrainingRecordDetails(Guid trainingRecordId)
        {
            var trainingRecord = Mapper.MapTrainingRecordEntityToUpdateTrainingRecordViewModel(_trainingRepository.GetTrainingRecordDetails(trainingRecordId));


            if (trainingRecord.Expires == true && trainingRecord.Validity != null)
            {
                var today = DateTime.Today;
                var dueThreshold = today.AddDays(30);

                if (trainingRecord.DueDate.Value < today)
                {
                    trainingRecord.Expired = true;
                }
                else if (trainingRecord.DueDate.Value >= today && trainingRecord.DueDate.Value <= dueThreshold)
                {
                    trainingRecord.Due = true;
                }
            }
            

            return trainingRecord;
        }



        /// <summary>
        /// check is trainingrecord assigned to operator
        /// </summary>
        /// <param name="modelSourceId"></param>
        /// <returns></returns>s
        public bool IsTrainingRecordAssignedToOperator(Guid trainingRecordId, string operatorId)
        {
            return _trainingRepository.IsTrainingRecordAssignedToOperator(trainingRecordId, operatorId);
        }

        /// <summary>
        /// check is training assigned to operator
        /// </summary>
        /// <param name="modelSourceId"></param>
        /// <returns></returns>s
        public bool IsTrainingAssignedToOperator(Guid trainingId, string operatorId)
        {
            return _trainingRepository.IsTrainingAssignedToOperator(trainingId, operatorId);
        }

        /// <summary>
        /// assign training to operator
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>s
        public TrainingOperatorMappingViewModel AssignTrainingToOperator(Guid trainingId, string operatorId)
        {
            return Mapper.MapTrainingOperatorMappingEntityToTrainingOperatorMappingViewModel(_trainingRepository.AssignTrainingToOperator(trainingId, operatorId));
        }


        /// <summary>
        /// assign trainingRecord to operator
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>s
        public TrainingRecordOperatorMappingViewModel AssignTrainingRecordToOperator(Guid trainingRecordId, string operatorId)
        {
            return Mapper.MapTrainingRecordOperatorMappingEntityToTrainingRecordOperatorMappingViewModel(_trainingRepository.AssignTrainingRecordToOperator(trainingRecordId,operatorId));
        }

        /// <summary>
        /// assign trainingRecord to operator
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>s
        public bool UnAssignTrainingRecordToOperator(Guid trainingRecordId, string operatorId)
        {
            return _trainingRepository.UnAssignTrainingRecordToOperator(trainingRecordId,operatorId);
        }

        /// <summary>
        /// get trainingRecord to operator
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>s
        public IEnumerable<TrainingRecordoperatorViewModel> GetTrainingRecordOperators(Guid trainingRecordId)
        {
            var users = _trainingRepository.GetTrainingRecordOperators(trainingRecordId).Select(a => Mapper.MapTrainingRecordOperatorMappingEntityToTrainingRecordoperatorViewModel(a)).ToList();
            return users;
        }

        /// <summary>
        /// get training to operator
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>s
        public IEnumerable<TrainingOperatorViewModel> GetOperatorTrainings(string operatorId)
        {
            var trainings = _trainingRepository.GetOperatorTrainings(operatorId).Select(a => Mapper.MapTrainingOperatorMappingEntityToTrainingOperatorViewModel(a)).ToList();
            foreach (var training in trainings)
            {
                if (training.DueDate  != null)
                {
                    var today = DateTime.Today;
                    if (training.DueDate.Value <= today)
                    {
                        training.Expired = true;
                    }
                }
            }
            return trainings;
        }

        /// <summary>
        /// get training to operator
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>s
        public OperatorTrainingAndTrainingRecord GetOperatorTrainingsAndTrainingRecord(string operatorId)
        {
            OperatorTrainingAndTrainingRecord operatorTrainingAndTrainingRecord = new();
           // operatorTrainingAndTrainingRecord.Trainings = _trainingRepository.GetOperatorTrainings(operatorId).Select(a => Mapper.MapTrainingOperatorMappingEntityToTrainingOperatorViewModel(a)).ToList();
            operatorTrainingAndTrainingRecord.TrainingRecords = _trainingRepository.GetOperatorsTrainingRecord(operatorId).Select(a => Mapper.MapTrainingRecordOperatorMappingEntityToTrainingRecordViewModel(a)).ToList();
            //if (operatorTrainingAndTrainingRecord.Trainings != null)
            //{
            //    foreach (var training in operatorTrainingAndTrainingRecord.Trainings)
            //    {
            //        if (training.DueDate != null)
            //        {
            //            var today = DateTime.Today;
            //            if (training.DueDate.Value <= today)
            //            {
            //                training.Expired = true;
            //            }
            //        }
            //    }
            //}
            if (operatorTrainingAndTrainingRecord.TrainingRecords != null)
            {
                foreach (var training in operatorTrainingAndTrainingRecord.TrainingRecords)
                {
                    if (training.DueDate != null)
                    {
                        var today = DateTime.Today;
                        if (training.DueDate.Value <= today)
                        {
                            training.Expired = true;
                        }
                    }
                }
            }

            return operatorTrainingAndTrainingRecord;
        }


        /// <summary>
        /// add operator answers
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public AddOperatorQuestionsResponseViewModel AddOperatorAnswers(string operatorId, AddOperatorQuestionsRequestViewModel model)
        {
            var operatorAnswerMappings = new List<OperatorAnswerMapping>();
            var AnswerCount = 0;
            Training trainingDetails = null;
            foreach (var question in model.Questions)
            {
                var answer = _trainingRepository.GetAnswerByQuestionAndAnswerId(question.QuestionId, question.AnswerId);
                var answers = _trainingRepository.GetAnswersByQuestion(question.QuestionId);
                var count = answers.Count();
                if (answer != null)
                {
                    if (trainingDetails == null)
                    {
                        trainingDetails = _trainingRepository.GetTrainingBySmartQuestionId(answer.SmartQuestionId);
                    }
                    
                    foreach (var ans in answers)
                    {
                        AnswerCount++;
                       var OperatorAnswer =  _trainingRepository.GetoperatorAnswer( ans.AnswerId, operatorId);
                        if (OperatorAnswer != null)
                        {
                            _trainingRepository.UpdateAnswerMapping(question.AnswerId, OperatorAnswer.OperatorAnswerMappingId);
                        }
                        else
                        {
                            if (ans.AnswerId == answer.AnswerId)
                            {
                                if (OperatorAnswer == null)
                                {
                                    operatorAnswerMappings.Add(new OperatorAnswerMapping
                                    {
                                        OperatorAnswerMappingId = Guid.NewGuid(),
                                        CreatedDate = DateTime.Now,
                                        AnswerId = answer.AnswerId,
                                        OperatorId = operatorId,
                                        //QuestionId = answer.SmartQuestionId
                                    });
                                   
                                }

                            }
                        }
                       

                      
                    }
                        
                }   
            }

            _trainingRepository.AddOperatorAnswerMappings(operatorAnswerMappings);
            if (trainingDetails != null)
            {
                var trainingRecordRequest = new TrainingRecordRequestViewModel
                {
                    Name = trainingDetails.Name,
                    Expires = trainingDetails.Expires,
                    Validity = trainingDetails.Validity,
                    Archived = trainingDetails.IsArchived,
                    TrainingTypeId = trainingDetails.TrainingTypeId,
                    CompletedDate = DateTime.Now,
                    Qualification = trainingDetails.Certification,
                    Certification=true,
                    Description=trainingDetails.Description

                };

                DateTime? dueDate = _trainingRepository.CalculateDueDate(trainingRecordRequest.Validity);
                var trainingRecordEntity = Mapper.MapTrainingRecordRequestViewModelToTrainingRecordEntity(operatorId, trainingRecordRequest);
                trainingRecordEntity.DueDate = dueDate;

                _trainingRepository.AddTrainingRecord(trainingRecordEntity);
                _trainingRepository.AssignTrainingRecordToOperator(trainingRecordEntity.TrainingRecordId, operatorId);
            }


            return new AddOperatorQuestionsResponseViewModel
            {
                OperatorId = operatorId,
                CreatedDate = DateTime.Now,
                Questions = operatorAnswerMappings.Select(a => new OperatorQuestionAnswerModel
                {
                    AnswerId = a.AnswerId,
                    QuestionId = a.Answer.SmartQuestionId
                }).ToList()
            };
        }



        /// <summary>
        /// get result
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>s
        public bool GetOperatorAnswersResult(Guid trainingId, string operatorId)
        {
            var questions =_trainingRepository.GetTrainingQuestions(trainingId);
            if (questions != null)
            {
                foreach (var question in questions)
                {
                    var answers = _trainingRepository.GetAnswersByQuestion(question.SmartQuestionId);
                    if(answers!= null)
                    {
                        foreach (var answer in answers)
                        {
                            var correctAnswer = _trainingRepository.GetCorrectAnswer(answer.AnswerId);
                            if (correctAnswer != null)
                            {
                                var ans = _trainingRepository.GetoperatorAnswer(correctAnswer.AnswerId, operatorId);
                                if (ans == null)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
           
            return true;
        }

        /// <summary>
        ///  usertype correct
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>s
        public bool IsUserTypeCorrect(string operatorId)
        {
                return _trainingRepository.IsUserTypeCorrect(operatorId);
        }
    }
}
