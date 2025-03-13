using Microsoft.AspNetCore.Http.HttpResults;
using SmartFarmer.Core.Common;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;
using System.Reflection;

namespace SmartFarmer.Core.Interface
{
    public interface ITrainingService
    {
        TrainingViewModel AddTraining(string CreatedBy,TrainingRequestViewModel model);
        TrainingViewModel GetTrainingDetails(Guid trainingId);
        UpdateTrainingViewModel UpdateTrainingDetails(UpdateTrainingViewModel model);
        bool IsTrainingExist(Guid trainingId);
        bool IsTrainingTypeExist(int trainingTypeId);
        TrainingListViewModel GetTrainingList(string UserMasterAdminId);
        TrainingSearchResponseViewModel GetTrainingListBySearchWithPagination(string UserMasterAdminId,TrainingSearchRequestViewModel model);
        AddQuestionsResponseViewModel AddTrainingQuestionAnswers(string CreatedBy,Guid trainingId, AddQuestionsRequestViewModel model);
        IEnumerable<WebQuestionResponseViewModel> GetTrainingQuestions(Guid trainingId, string UserMasterAdminId);
        IEnumerable<QuestionResponseViewModel> GetTrainingQuestionsApp(Guid trainingId, string UserMasterAdminId);
        IEnumerable<QuestionResponseViewModel> GetTrainingQuestionsAnswers(Guid trainingId, string UserId, string UserMasterAdminId);
        IEnumerable<QuestionResponseViewModel> GetOperatorQuestionsResult(Guid trainingId, string UserId, string UserMasterAdminId);
        bool UpdateTrainingArchiveStatus(Guid trainingId, bool archive);
        IEnumerable<WebQuestionResponseViewModel> UpdateQuestionsOrder(IEnumerable<WebQuestionResponseViewModel> models);
        TrainingRecordViewModel AddTrainingRecord(string CreatedBy, TrainingRecordRequestViewModel model);
        UpdateTrainingRecordViewModel UpdateTrainingRecordDetails(UpdateTrainingRecordViewModel model);
        bool IsTrainingRecordExist(Guid trainingRecordId);
        bool IsQuestionExist(Guid questionId);
        TrainingRecordSearchResponseViewModel GetTrainingRecordListBySearchWithPagination(string UserMasterAdminId, TrainingRecordSearchRequestViewModel model);

        UpdateTrainingRecordViewModel GetTrainingRecordDetails(Guid trainingRecordId);
        bool IsTrainingRecordAssignedToOperator(Guid trainingRecordId, string operatorId);
        bool IsTrainingAssignedToOperator(Guid trainingId, string operatorId);
        TrainingRecordOperatorMappingViewModel AssignTrainingRecordToOperator(Guid trainingRecordId, string operatorId);
        bool UnAssignTrainingRecordToOperator(Guid trainingRecordId, string operatorId);
        TrainingOperatorMappingViewModel AssignTrainingToOperator(Guid trainingId, string operatorId);
        IEnumerable<TrainingRecordoperatorViewModel> GetTrainingRecordOperators(Guid trainingRecordId);
        AddOperatorQuestionsResponseViewModel AddOperatorAnswers(string operatoeId, AddOperatorQuestionsRequestViewModel model);
        IEnumerable<TrainingOperatorViewModel> GetOperatorTrainings(string operatorId);
        OperatorTrainingAndTrainingRecord GetOperatorTrainingsAndTrainingRecord(string operatorId);
        bool GetOperatorAnswersResult(Guid trainingId, string operatorId);
        bool IsUserTypeCorrect(string operatorId);



    }
}
