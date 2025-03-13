using Microsoft.EntityFrameworkCore;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Data.Context;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;

namespace SmartFarmer.Data.Repository
{
    public class TrainingRepository : ITrainingRepository
    {
        private SmartFarmerContext _dbContext;
        public TrainingRepository(SmartFarmerContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Add Training
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Training AddTraining(Training model)
        {
            _dbContext.Trainings.Add(model);
            _dbContext.SaveChanges();
            var response = _dbContext.Trainings.Where(a => a.TrainingId == model.TrainingId).Include(a => a.ApplicationUser).Include(a => a.TrainingType).FirstOrDefault();
            return response;
        }

        /// <summary>
        /// get Training details 
        /// </summary>
        /// <param name="trainingId"></param>
        /// <returns></returns>
        public Training GetTrainingDetails(Guid trainingId)
        {
            return _dbContext.Trainings.Where(a => a.TrainingId == trainingId)/*.Include(a => a.SmartCourse)*/.Include(a => a.TrainingType).Include(a => a.ApplicationUser).FirstOrDefault();
        }

        /// <summary>
        /// get Training list by search pagination 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<Training> GetTrainingListBySearch(int pageNumber, int pageSize, string searchKey, int? trainingTypeId, string UserMasterAdminId, int? filterId)
        {
            var today = DateTime.Today;
            var training = _dbContext.Trainings.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).Include(a => a.ApplicationUser).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                training = training.Where(a => a.Name.ToLower().Contains(searchKey));
            }
            if (trainingTypeId != null)
            {
                training = training.Where(a => a.TrainingTypeId == trainingTypeId);
            }
            if (filterId != null)
            {
                if (filterId == 1)
                {
                    training = training.Where(a =>a.DueDate !=null && a.DueDate.Value  > today);
                }
                if (filterId == 2)
                {
                    training = training.Where(a => a.DueDate != null && a.DueDate.Value <= today);
                }
            }
            return training.Include(a => a.TrainingType).Include(a => a.TrainingFiles).Include(a => a.ApplicationUser)/*.Include(a => a.SmartCourse)*/.Skip((pageNumber - 1) * pageSize).OrderByDescending(a => a.CreatedDate).Take(pageSize).ToList();
        }

        /// <summary>
        /// get Training list 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<Training> GetTrainingList(string UserMasterAdminId)
        {
            var training = _dbContext.Trainings.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).AsQueryable();
            return training.ToList();
        }

        /// <summary>
        /// get Training count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetTrainingCountBySearch(string searchKey, int? trainingTypeId, string UserMasterAdminId, int? filterId)
        {
            var today = DateTime.Today;
            var training = _dbContext.Trainings.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                training = training.Where(a => a.Name.ToLower().Contains(searchKey));
            }
            if (trainingTypeId != null)
            {
                training = training.Where(a => a.TrainingTypeId == trainingTypeId);
            }
            if (filterId != null)
            {
                if (filterId == 1)
                {
                    training = training.Where(a => a.DueDate != null && a.DueDate.Value > today);
                }
                if (filterId == 2)
                {
                    training = training.Where(a => a.DueDate != null && a.DueDate.Value <= today);
                }
            }
            return training.Count();
        }

        /// <summary>
        ///  check the Training existence
        /// </summary>
        /// <param name="trainingId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsTrainingExist(Guid trainingId)
        {
            return _dbContext.Trainings.Find(trainingId) == null ? false : true;
        }

        /// <summary>
        ///  check the Training Type existence
        /// </summary>
        /// <param name="trainingTypeId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsTrainingTypeExist(int trainingTypeId)
        {
            return _dbContext.TrainingTypes.Find(trainingTypeId) == null ? false : true;
        }

        /// <summary>
        /// Update Training Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Training UpdateTrainingDetails(Training model)
        {
            var training = _dbContext.Trainings.FirstOrDefault(a => a.TrainingId == model.TrainingId);
            if (training == null)
            {
                return null;
            }
            training.Name = model.Name;
            training.Expires = model.Expires;
            training.Certification = model.Certification;
            training.Validity = model.Validity;
            training.Description = model.Description;
            training.TrainingTypeId = (int)Core.Common.Enums.TrainigTypeEnum.SmartFarmer;
            training.Link = model.Link;
            training.DueDate = model.DueDate;
            //training.SmartCourseId = model.SmartCourseId;
            _dbContext.Trainings.Update(training);
            _dbContext.SaveChanges();
            var response = _dbContext.Trainings.Where(a => a.TrainingId == model.TrainingId).Include(a => a.TrainingType).Include(a => a.ApplicationUser).FirstOrDefault();
            return response;
        }

        /// <summary>
        /// add Training question answers
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public SmartQuestion AddTrainingQuestionAnswers(Guid trainingId, SmartQuestion question)
        {
            question.TrainingId = trainingId;
            question.CreatedDate = DateTime.Now;
            _dbContext.SmartQuestions.Add(question);
            _dbContext.SaveChanges();
            return question;
        }
        
        /// <summary>
        /// update Training question answers
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public SmartQuestion UpdateTrainingQuestionAnswers(Guid trainingId, SmartQuestion question)
        {
            question.TrainingId = trainingId;
            foreach(var answer in question.Answers)
            {
                if(answer.AnswerId == Guid.Empty)
                {
                    answer.AnswerId = Guid.NewGuid();
                    answer.CreatedDate = DateTime.Now;
                    _dbContext.Answers.Add(answer);
                }
                else
                {
                    answer.CreatedDate = DateTime.Now;
                    _dbContext.Answers.Update(answer);
                }
            }
            question.CreatedDate = DateTime.Now;
            _dbContext.SmartQuestions.Update(question);
            _dbContext.SaveChanges();
            return question;
        }

        /// <summary>
        /// update training archive status into system 
        /// </summary>
        ///<param name="model"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool UpdateTrainingArchiveStatus(Guid trainingId, bool archive)
        {
            var training = _dbContext.Trainings.Find(trainingId);
            if (training != null)
            {
                training.IsArchived = archive;
                _dbContext.Trainings.Update(training);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool IsQuestionExist(Guid questionId)
        {
            return _dbContext.SmartQuestions.Any(q => q.SmartQuestionId == questionId);
        }

        /// <summary>
        /// update questions order  
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<SmartQuestion> UpdateQuestionsOrder(IEnumerable<SmartQuestion> models)
        {
            List<SmartQuestion> updateQuestions = new List<SmartQuestion>();
            if (models.Count() != 0)
            {
                foreach (var model in models)
                {
                    var existingQuestion = _dbContext.SmartQuestions.Include(q => q.Answers).FirstOrDefault(q => q.SmartQuestionId == model.SmartQuestionId); 
                    if (existingQuestion != null)
                    {
                        existingQuestion.OrderNumber = model.OrderNumber;
                        _dbContext.SmartQuestions.Update(existingQuestion);
                        updateQuestions.Add(existingQuestion);
                    }

                }
                _dbContext.SaveChanges();
            }
            return updateQuestions.OrderBy(a => a.OrderNumber).ToList();
        }

        /// <summary>
        /// update questions order  
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<SmartQuestion> GetTrainingQuestions(Guid trainingId, string UserMasterAdminId)
        {
            var trainingQuestions = _dbContext.SmartQuestions.Where(a=>a.ApplicationUser.MasterAdminId== UserMasterAdminId && a.TrainingId== trainingId).Include(q => q.Answers).Include(a=>a.ApplicationUser);
            return trainingQuestions.OrderBy(a => a.OrderNumber).ToList();
        }
        
        ///// <summary>
        ///// Get Operator Questions Answers  
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public List<OperatorAnswerMapping> GetTrainingQuestionsAnswers(Guid trainingId, string UserId)
        //{
        //    var trainingQuestions = _dbContext.SmartQuestions.Where(a => a.TrainingId == trainingId).ToList();
        //    List<OperatorAnswerMapping> operatorAnswerMapping = new();
        //    foreach (var trainingQuestion in trainingQuestions)
        //    {
        //        var operatorAnswerMappings = _dbContext.OperatorAnswerMappings.Where(a => a.OperatorId == UserId && a.QuestionId == trainingQuestion.SmartQuestionId).Include(q => q.Answer).Include(a => a.Operator);
        //        operatorAnswerMapping.AddRange(operatorAnswerMappings);
        //    }; 
        //    return operatorAnswerMapping;
        //}
        /// <summary>
        /// Get Operator Questions Answers  
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool GetQuestionsAnswers(Guid AnswerId, string UserId)
        {
            
                var result= _dbContext.OperatorAnswerMappings.FirstOrDefault(a => a.OperatorId == UserId && a.AnswerId == AnswerId) == null ? false : true;

               return result;
        }
        public DateTime? CalculateDueDate(int? validityMonths)
        {
            if (validityMonths.HasValue)
            {
               var dueDate= DateTime.Now.AddMonths(validityMonths.Value);
                return dueDate;
            }  
            return null; 
        }

        /// <summary>
        /// Add Training record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public TrainingRecord AddTrainingRecord(TrainingRecord model)
        {
            _dbContext.TrainingRecords.Add(model);
            _dbContext.SaveChanges();
            var response = _dbContext.TrainingRecords.Where(a => a.TrainingRecordId == model.TrainingRecordId).Include(a=>a.TrainingType).Include(a => a.ApplicationUser).FirstOrDefault();
            return response;
        }

        /// <summary>
        /// Update Training record  Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public TrainingRecord UpdateTrainingRecordDetails(TrainingRecord model)
        {
            var trainingRecord = _dbContext.TrainingRecords.FirstOrDefault(a => a.TrainingRecordId == model.TrainingRecordId);
            if (trainingRecord == null)
            {
                return null;
            }
            trainingRecord.Name = model.Name;
            trainingRecord.Expires = model.Expires;
            trainingRecord.Certification = model.Certification;
            trainingRecord.CompletedDate = model.CompletedDate;
            trainingRecord.Validity = model.Validity;
            trainingRecord.Qualification = model.Qualification;
            trainingRecord.Archived = model.Archived;
            trainingRecord.TrainingTypeId = model.TrainingTypeId;
            trainingRecord.DueDate = model.DueDate;
            trainingRecord.Description = model.Description;

            _dbContext.TrainingRecords.Update(trainingRecord);
            _dbContext.SaveChanges();
            var response = _dbContext.TrainingRecords.Where(a => a.TrainingRecordId == model.TrainingRecordId).Include(a => a.ApplicationUser).Include(a=>a.TrainingType).FirstOrDefault();
            return response;
        }

        /// <summary>
        ///  check the Training record existence
        /// </summary>
        /// <param name="trainingRecordId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsTrainingRecordExist(Guid trainingRecordId)
        {
            return _dbContext.TrainingRecords.Find(trainingRecordId) == null ? false : true;
        }

        /// <summary>
        /// get Training record list by search pagination 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<TrainingRecord> GetTrainingRecordListBySearch(int pageNumber, int pageSize, string searchKey, int? trainingTypeId,bool? archived, bool? expired, string UserMasterAdminId)
        {
            var trainingRecord = _dbContext.TrainingRecords.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).Include(a => a.ApplicationUser).Include(a=>a.TrainingRecordOperatorMappings).ThenInclude(a=>a.Operator).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                trainingRecord = trainingRecord.Where(a => a.Name.ToLower().Contains(searchKey));
            }
            if (trainingTypeId != null)
            {
                trainingRecord = trainingRecord.Where(a => a.TrainingTypeId == trainingTypeId);
            }
            if (expired != null)
            {
                trainingRecord = trainingRecord.Where(a => a.Expires == expired);
            }
            if (archived != null)
            {
                trainingRecord = trainingRecord.Where(a => a.Archived == archived);
            }
            return trainingRecord.Include(a => a.TrainingType).Include(a => a.ApplicationUser).Skip((pageNumber - 1) * pageSize).OrderByDescending(a => a.CreatedDate).Take(pageSize).ToList();
        }

        /// <summary>
        /// get Training record count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetTrainingRecordCountBySearch(string searchKey, int? trainingTypeId, bool? archived, bool? expired, string UserMasterAdminId)
        {
            var trainingRecord = _dbContext.TrainingRecords.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).Include(a => a.ApplicationUser).Include(a => a.TrainingRecordOperatorMappings).ThenInclude(a => a.Operator).AsQueryable();

            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                trainingRecord = trainingRecord.Where(a => a.Name.ToLower().Contains(searchKey));
            }
            if (trainingTypeId != null)
            {
                trainingRecord = trainingRecord.Where(a => a.TrainingTypeId == trainingTypeId);
            }
            if (expired != null)
            {
                trainingRecord = trainingRecord.Where(a => a.Expires == expired);
            }
            if (archived != null)
            {
                trainingRecord = trainingRecord.Where(a => a.Archived == archived);
            }
            return trainingRecord.Count();
        }

        /// <summary>
        /// get Training record details 
        /// </summary>
        /// <param name="trainingRecordId"></param>
        /// <returns></returns>
        public TrainingRecord GetTrainingRecordDetails(Guid trainingRecordId)
        {
            return _dbContext.TrainingRecords.Where(a => a.TrainingRecordId == trainingRecordId).Include(a => a.TrainingType).Include(a => a.ApplicationUser).FirstOrDefault();
        }

        /// <summary>
        /// check is trainingRecord assigned to operator
        /// </summary>
        /// <param name="trainingRecordId"></param>
        /// <returns></returns>
        public bool IsTrainingRecordAssignedToOperator(Guid trainingRecordId, string operatorId)
        {
            // return _dbContext.TrainingRecordOperatorMappings.Where(a => a.TrainingRecordId == trainingRecordId && a.OperatorId == operatorId) == null ? false : true;

            return _dbContext.TrainingRecordOperatorMappings.Any(a => a.TrainingRecordId == trainingRecordId && a.OperatorId == operatorId);
        }

        /// <summary>
        /// check is training assigned to operator
        /// </summary>
        /// <param name="trainingId"></param>
        /// <returns></returns>
        public bool IsTrainingAssignedToOperator(Guid trainingId, string operatorId)
        {
            //return _dbContext.TrainingOperatorMappings.Where(a => a.TrainingId == trainingId && a.OperatorId == operatorId) == null ? false : true;
            return _dbContext.TrainingOperatorMappings.Any(a => a.TrainingId == trainingId && a.OperatorId == operatorId);
        }
        /// <summary>
        /// assign trainingRecord to operator
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public TrainingRecordOperatorMapping AssignTrainingRecordToOperator(Guid trainingRecordId, string operatorId)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(a => a.Id == operatorId && (a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Operator || a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Both));

            TrainingRecordOperatorMapping trainingRecordOperatorMapping = new TrainingRecordOperatorMapping()
            {
                TrainingRecordId = trainingRecordId,
                TrainingRecordOperatorMappingId = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                OperatorId = operatorId,
            };
            _dbContext.TrainingRecordOperatorMappings.Add(trainingRecordOperatorMapping);
            _dbContext.SaveChanges();


            return trainingRecordOperatorMapping;
        }

        /// <summary>
        /// Unassign trainingRecord to operator
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UnAssignTrainingRecordToOperator(Guid trainingRecordId, string operatorId)
        {
            var isTraingOperatorExist = _dbContext.TrainingRecordOperatorMappings.FirstOrDefault(a => a.OperatorId == operatorId && a.TrainingRecordId == trainingRecordId);

            if (isTraingOperatorExist == null) return false;
            _dbContext.TrainingRecordOperatorMappings.Remove(isTraingOperatorExist);
            _dbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// get oprator answer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OperatorAnswerMapping GetoperatorAnswer(Guid answerId, string operatorId)
        {
            return _dbContext.OperatorAnswerMappings.FirstOrDefault(a => a.OperatorId == operatorId && a.AnswerId == answerId);

             
        }

        /// <summary>
        /// assign training to operator
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public TrainingOperatorMapping AssignTrainingToOperator(Guid trainingId, string operatorId)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(a => a.Id == operatorId && (a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Operator || a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Both));

            TrainingOperatorMapping trainingOperatorMapping = new TrainingOperatorMapping()
            {
                TrainingId = trainingId,
                TrainingOperatorMappingId = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                OperatorId = operatorId,
            };
            _dbContext.TrainingOperatorMappings.Add(trainingOperatorMapping);
            _dbContext.SaveChanges();
            return trainingOperatorMapping;
        }

        /// <summary>
        /// get trainingRecord to operator
        /// </summary>
        /// <param name="trainingRecordId"></param>
        /// <returns></returns>
        public IEnumerable<TrainingRecordOperatorMapping> GetTrainingRecordOperators(Guid trainingRecordId)
        {
           var  users= _dbContext.TrainingRecordOperatorMappings.Where(a => a.TrainingRecordId == trainingRecordId).Include(a=>a.Operator).Include(a=>a.Operator.UserGroup).ToList();
            return users.DistinctBy(a => a.OperatorId);

        }

        /// <summary>
        /// get trainingRecord to operator
        /// </summary>
        /// <param name="trainingRecordId"></param>
        /// <returns></returns>
        public IEnumerable<TrainingRecordOperatorMapping> GetOperatorsTrainingRecord(string operatorId)
        {
            var trainingRecords = _dbContext.TrainingRecordOperatorMappings.Where(a => a.OperatorId == operatorId).Include(a => a.TrainingRecord).ToList();
            return trainingRecords;
        }

        /// <summary>
        /// get training of operator
        /// </summary>
        /// <param name="OperatorId"></param>
        /// <returns></returns>
        public IEnumerable<TrainingOperatorMapping> GetOperatorTrainings(string operatorId)
        {
            var trainings = _dbContext.TrainingOperatorMappings.Where(a => a.OperatorId == operatorId).Include(a => a.Training).Include(a => a.Training.TrainingType).ToList();
            return trainings;
        }
        /// <summary>
        /// get training record of operator
        /// </summary>
        /// <param name="OperatorId"></param>
        /// <returns></returns>
        public IEnumerable<TrainingRecordOperatorMapping> GetOperatorTrainingRecords(string operatorId)
        {
            var trainingRecords = _dbContext.TrainingRecordOperatorMappings.Where(a => a.OperatorId == operatorId).Include(a => a.TrainingRecord).Include(a => a.TrainingRecord.TrainingType).ToList();
            return trainingRecords;
        }

        /// <summary>
        /// get answer 
        /// </summary>
        /// <param name="trainingRecordId"></param>
        /// <returns></returns>
        public Answer GetAnswerByQuestionAndAnswerId(Guid questionId, Guid answerId)
        {
            return _dbContext.Answers.Include(a => a.SmartQuestion).FirstOrDefault(a => a.AnswerId == answerId && a.SmartQuestionId == questionId);
        }

        /// <summary>
        /// get all answers of question 
        /// </summary>
        /// <param name="trainingRecordId"></param>
        /// <returns></returns>
        public IEnumerable<Answer> GetAnswersByQuestion(Guid questionId)
        {
            return _dbContext.Answers.Include(a => a.SmartQuestion).Where(a =>a.SmartQuestionId == questionId).ToList();
        }

        /// <summary>
        /// add operator 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public void AddOperatorAnswerMappings(List<OperatorAnswerMapping> operatorAnswerMappings)
        {
            _dbContext.OperatorAnswerMappings.AddRange(operatorAnswerMappings);
            _dbContext.SaveChanges();
        }
        /// <summary>
        /// add operator 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public void UpdateAnswerMapping(Guid AnswerId, Guid OperatorAnswerMappingId)
        {

           var OperatorAnswerMapping = _dbContext.OperatorAnswerMappings.FirstOrDefault(a=>a.OperatorAnswerMappingId== OperatorAnswerMappingId);
            if (OperatorAnswerMapping != null) {
                OperatorAnswerMapping.AnswerId= AnswerId;
                _dbContext.OperatorAnswerMappings.Update(OperatorAnswerMapping);
                _dbContext.SaveChanges();
            }
           
        }

        /// <summary>
        /// get  training detail by questionId 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public Training GetTrainingBySmartQuestionId(Guid smartQuestionId)
        {
            return _dbContext.SmartQuestions.Where(q => q.SmartQuestionId == smartQuestionId).Include(q => q.Training).Select(q => q.Training).FirstOrDefault();
        }


        /// <summary>
        /// get training questions  
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<SmartQuestion> GetTrainingQuestions(Guid trainingId)
        {
            return _dbContext.SmartQuestions.Where(a => a.TrainingId == trainingId).ToList();
        }

        public Answer GetCorrectAnswer(Guid answerId)
        {
            return _dbContext.Answers.Where(a => a.AnswerId == answerId && a.IsCorrect==true).FirstOrDefault();
        }


        /// <summary>
        /// usertype correct 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool IsUserTypeCorrect(string operatorId)
        {
            var user= _dbContext.ApplicationUsers.Where(a=>a.Id==operatorId && (a.ApplicationUserTypeId== (int)Core.Common.Enums.ApplicationUserTypeEnum.Operator|| a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Both)).FirstOrDefault();
            if (user != null)
            {
                return true;
            }
            return false;
        }

    }
}
