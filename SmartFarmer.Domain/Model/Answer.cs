namespace SmartFarmer.Domain.Model
{
    public class Answer
    {
        public Guid AnswerId { get; set; }
        public string Text { get; set; }
        public bool? IsCorrect { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public Guid SmartQuestionId { get; set; }
        public SmartQuestion SmartQuestion { get; set; }
        ////FK
        public ICollection<OperatorAnswerMapping> OperatorAnswerMappings { get; set; }
    }
}
