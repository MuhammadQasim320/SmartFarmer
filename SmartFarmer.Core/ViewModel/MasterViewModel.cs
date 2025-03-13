namespace SmartFarmer.Core.ViewModel
{
    public class MasterViewModel
    {
        public class ActionTypeViewModel
        {
            public int ActionTypeId { get; set; }
            public string Type { get; set; }
        }
        public class ActionTypeListViewModel
        {
            public IEnumerable<ActionTypeViewModel> List { get; set; }
        }

        public class UserStatusViewModel
        {
            public int ApplicationUserStatusId { get; set; }
            public string Status { get; set; }
        }
        
        public class OperatorStatusViewModel
        {
            public int OperatorStatusId { get; set; }
            public string Status { get; set; }
        }
        public class OperatorStatusListViewModel
        {
            public IEnumerable<OperatorStatusViewModel> List { get; set; }
        }
        public class UserStatusListViewModel
        {
            public IEnumerable<UserStatusViewModel> List { get; set; }
        }

        public class UserTypeViewModel
        {
            public int ApplicationUserTypeId { get; set; }
            public string Type { get; set; }
        }
        public class UserTypeListViewModel
        {
            public IEnumerable<UserTypeViewModel> List { get; set; }
        }

        public class CheckTypeViewModel
        {
            public int CheckTypeId { get; set; }
            public string Type { get; set; }
        }
        public class CheckTypeListViewModel
        {
            public IEnumerable<CheckTypeViewModel> List { get; set; }
        }

        public class EventTypeViewModel
        {
            public int EventTypeId { get; set; }
            public string Type { get; set; }
        }
        public class EventTypeListViewModel
        {
            public IEnumerable<EventTypeViewModel> List { get; set; }
        }

        public class FrequencyTypeViewModel
        {
            public int FrequencyTypeId { get; set; }
            public string Type { get; set; }
        }
        public class FrequencyTypeListViewModel
        {
            public IEnumerable<FrequencyTypeViewModel> List { get; set; }
        }

        public class TrainingTypeViewModel
        {
            public int TrainingTypeId { get; set; }
            public string Type { get; set; }
        }
        public class TrainingTypeListViewModel
        {
            public IEnumerable<TrainingTypeViewModel> List { get; set; }
        }

        public class UnitsTypeViewModel
        {
            public int UnitsTypeId { get; set; }
            public string Units { get; set; }
        }
        public class UnitsTypeListViewModel
        {
            public IEnumerable<UnitsTypeViewModel> List { get; set; }
        }

        public class InitialRiskAndAdjustedRiskViewModel
        {
            public int InitialRiskAndAdjustedRiskId { get; set; }
            public string RiskValue { get; set; }
        }
        public class InitialRiskAndAdjustedRiskListViewModel
        {
            public IEnumerable<InitialRiskAndAdjustedRiskViewModel> List { get; set; }
        }


        public class MachineStatusViewModel
        {
            public int MachineStatusId { get; set; }
            public string Status { get; set; }
        }
        public class MachineStatusListViewModel
        {
            public IEnumerable<MachineStatusViewModel> List { get; set; }
        }
        
        public class IssueTypeViewModel
        {
            public int IssueTypeId { get; set; }
            public string Type { get; set; }
        }
        public class IssueTypeListViewModel
        {
            public IEnumerable<IssueTypeViewModel> List { get; set; }
        }

        public class IssueStatusViewModel
        {
            public int IssueStatusId { get; set; }
            public string Status { get; set; }
        }
        public class IssueStatusListViewModel
        {
            public IEnumerable<IssueStatusViewModel> List { get; set; }
        } 
        public class HazardTypeViewModel
        {
            public int HazardTypeId { get; set; }
            public string Type { get; set; }
        }
        public class HazardTypeListViewModel
        {
            public IEnumerable<HazardTypeViewModel> List { get; set; }
        }
        
        public class MobileActionTypeViewModel
        {
            public int MobileActionTypeId { get; set; }
            public string Type { get; set; }
        }
        public class MobileActionTypeListViewModel
        {
            public IEnumerable<MobileActionTypeViewModel> List { get; set; }
        }

        public class CheckResultViewModel
        {
            public int ResultId { get; set; }
            public string Result { get; set; }
        }
        public class CheckResultListViewModel
        {
            public IEnumerable<CheckResultViewModel> List { get; set; }
        }
    }
}
