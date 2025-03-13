using static SmartFarmer.Core.ViewModel.MasterViewModel;

namespace SmartFarmer.Core.Interface
{
    public interface IMasterService
    {
        //IEnumerable<ActionTypeViewModel> GetActionTypes();
        IEnumerable<UserStatusViewModel> GetUserStatuses();
        IEnumerable<OperatorStatusViewModel> GetOperatorStatuses();
        IEnumerable<UserTypeViewModel> GetUserTypes();
        IEnumerable<CheckTypeViewModel> GetCheckTypes();
        bool IsCheckTypeExist(int checkTypeId);
        IEnumerable<EventTypeViewModel> GetEventTypes();
        IEnumerable<FrequencyTypeViewModel> GetFrequencyTypes();
        bool IsFrequencyTypeExist(int frequencyTypeId);
        IEnumerable<TrainingTypeViewModel> GetTrainingTypes();
        IEnumerable<UnitsTypeViewModel> GetUnitsTypes();
        IEnumerable<InitialRiskAndAdjustedRiskViewModel> GetInitialRiskAndAdjustedRisks();
        IEnumerable<MachineStatusViewModel> GetMachineStatuses();
        IEnumerable<IssueTypeViewModel> GetIssueTypes();
        IEnumerable<IssueStatusViewModel> GetIssueStatuses();
        bool IsEventTypeExist(int eventTypeId);
        bool IsMachineStatusExist(int machineServiceId);
        bool IsIssueTypeExist(int issueTypeId);
        bool IsIssueStatusExist(int issueStatusId);
        IEnumerable<EventTypeViewModel> GetAlarmTypes();
        IEnumerable<HazardTypeViewModel> GetHazardTypes();
        IEnumerable<MobileActionTypeViewModel> GetMobileActionTypes();
        IEnumerable<CheckResultViewModel> GetCheckResults();
        bool IsHazardTypeExist(int hazardTypeId);
        bool IsMobileActionTypeExist(int IsMobileActionTypeId);
        bool IsResultExist(int resultId);
    }
}
