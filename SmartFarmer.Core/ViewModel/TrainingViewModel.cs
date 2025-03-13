using SmartFarmer.Domain.Model;
using System.ComponentModel.DataAnnotations;

namespace SmartFarmer.Core.ViewModel
{
    public class TrainingViewModel : TrainingRequestViewModel
    {
        public Guid TrainingId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int TrainingTypeId { get; set; }
        public string TrainingTypeName { get; set; }
        public bool Expired { get; set; }
    }

    public class TrainingOperatorListViewModel
    {
        public IEnumerable<TrainingOperatorViewModel> List { get; set; }
    }

    public class OperatorTrainingAndTrainingRecord
    {
        //public IEnumerable<TrainingOperatorViewModel> Trainings { get; set; }
        public IEnumerable<TrainingRecordViewModel> TrainingRecords { get; set; }
        //public Guid TrainingId { get; set; }
        //public Guid TrainingRecordId { get; set; }
        //public Guid TrainingOperatorMappingId { get; set; }
        //public Guid TrainingRecordOperatorMappingId { get; set; }
        //public Guid TrainingId { get; set; }
        //public string Name { get; set; }
        //public DateTime? DueDate { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public bool Expired { get; set; }
        //public int? TrainingTypeId { get; set; }
        //public string TrainingTypeName { get; set; }
        //public string OperatorId { get; set; }
        ////public string Name { get; set; }
        //public string Image { get; set; }
    }

    public class TrainingOperatorViewModel 
    {
        public Guid TrainingId { get; set; }
        public string Name { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool Expired { get; set; }
        public int? TrainingTypeId { get; set; }
        public string TrainingType { get; set; }
        public bool Expires { get; set; }
        public int? Validity { get; set; }
        public bool IsArchived { get; set; }
        public string Certification { get; set; }
    }
    public class TrainingWithFileURLsResponseViewModel : TrainingViewModel
    {
        public List<string> Files { get; set; }
    }
    public class TrainingRequestViewModel
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Expires { get; set; }
        public int? Validity { get; set; }
        public bool IsArchived { get; set; }
        public string Link { get; set; }
        [MaxLength(100)]
        public string Certification { get; set; }
        //public Guid? SmartCourseId { get; set; }

    }
    public class UpdateTrainingViewModel : UpdateTrainingRequestViewModel
    {
        public Guid TrainingId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? DueDate { get; set; }
        public int TrainingTypeId { get; set; }
        public string TrainingTypeName { get; set; }
    }
    public class UpdateTrainingRequestViewModel
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Expires { get; set; }
        public int? Validity { get; set; }
        public string Link { get; set; }
        [MaxLength(100)]
        public string Certification { get; set; }
        //public Guid? SmartCourseId { get; set; }

    }
    public class TrainingSearchRequestViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchKey { get; set; }
        public int? TrainingTypeId { get; set; }
        public int? FilterId { get; set; }

    }
    public class TrainingSearchResponseViewModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<TrainingWithFileURLsResponseViewModel> List { get; set; }

    }
    public class TrainingNameListViewModel
    {
        public Guid TrainingId { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }

    }
    public class TrainingListViewModel
    {
        public IEnumerable<TrainingNameListViewModel> TrainingList { get; set; }
    }  
    public class TrainingDetailViewModel
    {
        public TrainingViewModel Data { get; set; }
        public IEnumerable<TrainingFileViewModel> TrainingFilesList { get; set; }
    }
    public class AddQuestionsRequestViewModel
    {
        public List<QuestionRequestViewModel> Questions { get; set; }
    }
    public class QuestionRequestViewModel
    {
        public Guid? SmartQuestionId { get; set; }
        public string QuestionText { get; set; }
        public int OrderNumber { get; set; }
        public List<AnswerRequestViewModel> Answers { get; set; }
    }
    public class AnswerRequestViewModel
    {
        public Guid? AnswerId { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }

    public class AddQuestionsResponseViewModel
    {
        public Guid TrainingId { get; set; }
        public List<WebQuestionResponseViewModel> Questions { get; set; } = new List<WebQuestionResponseViewModel>(); 

    }

    public class WebQuestionResponseViewModel
    {
        public Guid SmartQuestionId { get; set; }
        public string QuestionText { get; set; }
        public int OrderNumber { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public List<WebAnswerResponseViewModel> Answers { get; set; }
    }
    public class QuestionResponseViewModel
    {
        public Guid SmartQuestionId { get; set; }
        public string QuestionText { get; set; }
        public int OrderNumber { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public List<AnswerResponseViewModel> Answers { get; set; }
    }
    public class QuestionsAnwersResponseViewModel
    {
        public Guid SmartQuestionId { get; set; }
        //public string QuestionText { get; set; }
        //public int OrderNumber { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public AnswersQuestionsResponseViewModel Answers { get; set; }
    }

    public class AnswerResponseViewModel
    {
        public Guid AnswerId { get; set; }
        public string AnswerText { get; set; }
        public bool? IsCorrect { get; set; }
        public bool? OperatorAnswered { get; set; }
        
    }
    public class WebAnswerResponseViewModel
    {
        public Guid AnswerId { get; set; }
        public string AnswerText { get; set; }
        public bool? IsCorrect { get; set; }

    }

    public class AnswersQuestionsResponseViewModel
    {
        public Guid AnswerId { get; set; }
        public string AnswerText { get; set; }
        //public Guid Answer { get; set; }
    }

    public class TrainingArchiveViewModel
    {
        public Guid TrainingId { get; set; }
        public bool Archive { get; set; }
    }

    public class UpdateQuestionOrderViewModel
    {
        public Guid SmartQuestionId { get; set; }
        public int OrderNumber { get; set; }
    }
    public class UpdateQuestionOrderListViewModel
    {
        public IEnumerable<UpdateQuestionOrderViewModel> Questions { get; set; }
        public int OrderNumber { get; set; }
    }
    public class TrainingRecordViewModel : TrainingRecordRequestViewModel
    {
        public Guid TrainingRecordId { get; set; }
        public int TrainingTypeId { get; set; }
        public string TrainingType { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public bool Expired { get; set; }
        public bool Due { get; set; }
    }
    public class TrainingRecordRequestViewModel
    {
        [MaxLength(100)]
        public string Name { get; set; }
        //public DateTime CompletedDate { get; set; }

        public string Description { get; set; }
        public bool Expires { get; set; }
        public int? Validity { get; set; }
        public DateTime? CompletedDate { get; set; }
        public bool Certification { get; set; }
        [MaxLength(200)]
        public string Qualification { get; set; }
        public bool Archived { get; set; }
        public int TrainingTypeId { get; set; }
        //public bool Expired { get; set; }
    }



    public class UpdateTrainingRecordViewModel : UpdateTrainingRecordRequestViewModel
    {
        public Guid TrainingRecordId { get; set; }
        public string TrainingType { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public bool Expired { get; set; }
        public bool Due { get; set; }

    }
    public class UpdateTrainingRecordRequestViewModel
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public DateTime? CompletedDate { get; set; }
        public bool Expires { get; set; }
        public int? Validity { get; set; }
        public bool Certification { get; set; }
        [MaxLength(200)]
        public string Qualification { get; set; }
        public bool Archived { get; set; }
        public int TrainingTypeId { get; set; }
        public string Description { get; set; }
    }

    public class TrainingOperatorMappingListViewModel
    {
        public IEnumerable<TrainingOperatorMappingViewModel> List { get; set; }

    }

    public class TrainingOperatorMappingViewModel
    {
        [Key]
        public Guid TrainingOperatorMappingId { get; set; }
        public Guid TrainingId { get; set; }
        public string OperatorId { get; set; }
    }



    public class TrainingRecordSearchRequestViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchKey { get; set; }
        public int? TrainingTypeId { get; set; }
        public bool? Archived { get; set; }
        public bool? Expired { get; set; }

    }
    public class TrainingRecordSearchResponseViewModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<TrainingRecordsViewModel> List { get; set; }

    }

    public class TrainingRecordOperatorMappingListViewModel
    {
        public IEnumerable<TrainingRecordOperatorMappingViewModel> List { get; set; }

    }

    public class TrainingRecordOperatorMappingViewModel
    {
        [Key]
        public Guid TrainingRecordOperatorMappingId { get; set; }
        public Guid TrainingRecordId { get; set; }
        public string OperatorId { get; set; }
    }





    public class TrainingRecordoperatorListViewModel
    {
        public IEnumerable<TrainingRecordoperatorViewModel> List { get; set; }
    }
    public class TrainingRecordoperatorViewModel
    {
        public string OperatorId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public Guid? UserGroupId { get; set; }
        public string GroupName { get; set; }
    }


    //public class UpdateAnswerOrderViewModel
    //{
    //    public Guid AnswerId { get; set; }
    //    public int OrderNumber { get; set; }
    //}
    //public class UpdateAnswerOrderListViewModel
    //{
    //    public IEnumerable<UpdateAnswerOrderViewModel> Questions { get; set; }
    //    public int OrderNumber { get; set; }
    //}



    public class AddOperatorQuestionsResponseViewModel: AddOperatorQuestionsRequestViewModel
    {
        public string OperatorId { get; set; }
        public DateTime CreatedDate { get; set; }
    }


    public class AddOperatorQuestionsRequestViewModel
    {
        public List<OperatorQuestionAnswerModel> Questions { get; set; }
    }

    public class OperatorQuestionAnswerModel
    {
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
    }

    public class OperatorTrainingRecordViewModel
    {
        public Guid TrainingRecordId { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public bool Expires { get; set; }
        public bool Certification { get; set; }
        public string Qualification { get; set; }
        public bool Archived { get; set; }
        public int TrainingTypeId { get; set; }
        public string TrainingType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public bool Expired { get; set; }
        public bool Due { get; set; }
        public int? Validity { get; set; }
        public DateTime? DueDate { get; set; }
        public string Description { get; set; }
    }




    public class TrainingRecordsViewModel 
    {
        public Guid TrainingRecordId { get; set; }
        public string TrainingType { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public bool Expired { get; set; }
        public bool Due { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
        public DateTime? CompletedDate { get; set; }
        public bool Expires { get; set; }
        public int? Validity { get; set; }
        public bool Certification { get; set; }
        [MaxLength(200)]
        public string Qualification { get; set; }
        public bool Archived { get; set; }
        public int TrainingTypeId { get; set; }
        public string Description { get; set; }
        public IEnumerable<TrainingRecordApplicationUserViewModel> Users { get; set; }

    }

}
