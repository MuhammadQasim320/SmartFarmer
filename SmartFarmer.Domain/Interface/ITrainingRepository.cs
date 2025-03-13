using SmartFarmer.Domain.Model;

namespace SmartFarmer.Domain.Interface
{
    public interface ITrainingRepository
    {
        Training AddTraining(Training model);
        Training GetTrainingDetails(Guid trainingId);
        Training UpdateTrainingDetails(Training model);
        bool IsTrainingExist(Guid trainingId);
        bool IsTrainingTypeExist(int trainingTypeId);
        IEnumerable<Training> GetTrainingListBySearch(int pageNumber, int pageSize, string searchKey, int? trainingTypeId,string UserMasterAdminId,int? filterId);
        IEnumerable<Training> GetTrainingList(string UserMasterAdminId);
        int GetTrainingCountBySearch(string searchKey, int? trainingTypeId,string UserMasterAdminId, int? filterId);
        bool IsQuestionExist(Guid questionId);
        SmartQuestion AddTrainingQuestionAnswers(Guid trainingId, SmartQuestion question);
        SmartQuestion UpdateTrainingQuestionAnswers(Guid trainingId, SmartQuestion model);
        bool UpdateTrainingArchiveStatus(Guid trainingId, bool archive);
        IEnumerable<SmartQuestion> UpdateQuestionsOrder(IEnumerable<SmartQuestion> models);
        IEnumerable<SmartQuestion> GetTrainingQuestions(Guid trainingId, string UserMasterAdminId);
        bool GetQuestionsAnswers(Guid answerId, string UserId);
        //List<OperatorAnswerMapping> GetTrainingQuestionsAnswers(Guid trainingId, string UserId);
        DateTime? CalculateDueDate(int? validityMonths);
        TrainingRecord AddTrainingRecord(TrainingRecord model);

        TrainingRecord UpdateTrainingRecordDetails(TrainingRecord model);
        bool IsTrainingRecordExist(Guid trainingRecordId);
        IEnumerable<TrainingRecord> GetTrainingRecordListBySearch(int pageNumber, int pageSize, string searchKey, int? trainingTypeId,bool? archived, bool? expired, string UserMasterAdminId);
        int GetTrainingRecordCountBySearch(string searchKey, int? trainingTypeId, bool? archived, bool? expired, string UserMasterAdminId);
        TrainingRecord GetTrainingRecordDetails(Guid trainingRecordId);
        bool IsTrainingRecordAssignedToOperator(Guid trainingRecordId, string operatorId);
        bool IsTrainingAssignedToOperator(Guid trainingId, string operatorId);
        TrainingRecordOperatorMapping AssignTrainingRecordToOperator(Guid trainingRecordId,string operatorId);
        bool UnAssignTrainingRecordToOperator(Guid trainingRecordId,string operatorId);
        OperatorAnswerMapping GetoperatorAnswer(Guid answerId,string operatorId);
        TrainingOperatorMapping AssignTrainingToOperator(Guid trainingId,string operatorId);
        IEnumerable<TrainingRecordOperatorMapping> GetTrainingRecordOperators(Guid trainingRecordId);
        //IEnumerable<TrainingRecordOperatorMapping> GetTrainingRecordOperators(string operatorId);
        IEnumerable<TrainingRecordOperatorMapping> GetOperatorsTrainingRecord(string operatorId);
        IEnumerable<TrainingOperatorMapping> GetOperatorTrainings(string operatorId);
        IEnumerable<TrainingRecordOperatorMapping> GetOperatorTrainingRecords(string operatorId);
        Answer GetAnswerByQuestionAndAnswerId(Guid questionId, Guid answerId);
        IEnumerable<Answer> GetAnswersByQuestion(Guid questionId);
        void AddOperatorAnswerMappings(List<OperatorAnswerMapping> operatorAnswerMappings);
        void UpdateAnswerMapping(Guid AnswerId, Guid OperatorAnswerMappingId);
        Training GetTrainingBySmartQuestionId(Guid smartQuestionId);
        IEnumerable<SmartQuestion> GetTrainingQuestions(Guid trainingId);
        Answer GetCorrectAnswer(Guid answerId);
        bool IsUserTypeCorrect(string operatorId);
    }
}
