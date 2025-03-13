namespace SmartFarmer.Domain.Model
{
    public class SmartQuestion
    {
        public Guid SmartQuestionId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public int OrderNumber { get; set; }
        public Guid TrainingId { get; set; }
        public Training Training { get; set; }
        public string CreatedBy { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
         public ICollection<Answer> Answers { get; set; }

        //public ICollection<QuestionAnswerMapping> questionAnswerMappings { get; set; }
    }
}
