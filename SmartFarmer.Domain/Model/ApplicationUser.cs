using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace SmartFarmer.Domain.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImageName { get; set; }
        public string ProfileImageLink { get; set; }
        public string Mobile { get; set; }
        public string HouseNameNumber { get; set; }
        public string Street { get; set; }
        public string Addressline2 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string PostCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Location { get; set; }
        public string CreatedBy { get; set; }
        public string MasterAdminId { get; set; }
        public string MainAdminId { get; set; }
        public Guid? FarmId { get; set; }

        //FK
        public int ApplicationUserStatusId { get; set; }
        public int? OperatorStatusId { get; set; }
        public int ApplicationUserTypeId { get; set; }
        public Guid? UserGroupId { get; set; }
        public ApplicationUserStatus ApplicationUserStatus { get; set; }
        public OperatorStatus OperatorStatus { get; set; }
        public ApplicationUserType ApplicationUserType { get; set; }
        public UserGroup UserGroup { get; set; }
        public ApplicationUser MasterAdmin { get; set; }
        public ApplicationUser MainAdmin { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<Farm> Farms { get; set; }
        public ICollection<Farm> CreatedByFarms { get; set; }
        public ICollection<Field> Fields { get; set; }
        public ICollection<Machine> Machines { get; set; }
        public ICollection<MachineCategory> MachineCategorys { get; set; }
        public ICollection<Machine> OperatorMachines { get; set; }
        public ICollection<CheckList> CheckLists { get; set; }
        public ICollection<CheckList> OperatorCheckLists { get; set; }
        public ICollection<IssueCategory> IssueCategories { get; set; }
        public ICollection<Issue> Issues { get; set; }
        public ICollection<Issue> OperatorIssues { get; set; }
        //public ICollection<IssueComment> IssueComments { get; set; }
        public ICollection<RiskAssessment> RiskAssessments { get; set; }
        public ICollection<RiskAssessmentLog> RiskAssessmentLogs { get; set; }
        //public ICollection<SmartCourse> SmartCourses { get; set; }
        public ICollection<Training> Trainings { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }
        public ICollection<WelfareRoutine> WelfareRoutines { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public ICollection<MachineOperatorMapping> MachineOperatorMappings { get; set; }
        public ICollection<MachineType> MachineTypes { get; set; }
        public ICollection<TrainingOperatorMapping> TrainingOperatorMappings { get; set; }
        public ICollection<CheckListMachineMapping> CheckListMachineMappings { get; set; }
        public ICollection<SmartQuestion> SmartQuestions { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public ICollection<TrainingRecord> TrainingRecords { get; set; }
        public ICollection<TrainingRecordOperatorMapping> TrainingRecordOperatorMappings { get; set; }
        public ICollection<OperatorAnswerMapping> OperatorAnswerMappings { get; set; }
        public ICollection<HazardKey> HazardKeys { get; set; }
        public ICollection<ApplicationUser> MainApplicationUsers { get; set; }
        public ICollection<AlarmAction> AlarmActions { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
