using SmartFarmer.Domain.Model;

namespace SmartFarmer.Domain.Interface
{
    public interface IMasterRepository
    {
        //IEnumerable<ActionType> GetActionTypes();
        IEnumerable<ApplicationUserStatus> GetUserStatuses();
        IEnumerable<OperatorStatus> GetOperatorStatuses();
        IEnumerable<ApplicationUserType> GetUserTypes();
        IEnumerable<CheckType> GetCheckTypes();
        bool IsCheckTypeExist(int checkTypeId);
        IEnumerable<EventType> GetEventTypes();
        IEnumerable<FrequencyType> GetFrequencyTypes();
        bool IsFrequencyTypeExist(int frequencyTypeId);
        IEnumerable<TrainingType> GetTrainingTypes();
        IEnumerable<UnitsType> GetUnitsTypes();
        IEnumerable<InitialRiskAndAdjustedRisk> GetInitialRiskAndAdjustedRisks();
        IEnumerable<MachineStatus> GetMachineStatuses();
        IEnumerable<IssueType> GetIssueTypes();
        IEnumerable<IssueStatus> GetIssueStatuses();
        bool IsEventTypeExist(int eventTypeId);
        bool IsMachineStatusExist(int machineStatusId);
        bool IsIssueTypeExist(int issueTypeId);
        bool IsIssueStatusExist(int issueStatusId);
        IEnumerable<EventType> GetAlarmTypes();
        IEnumerable<HazardType> GetHazardTypes();
        IEnumerable<MobileActionType> GetMobileActionTypes();
        IEnumerable<CheckResult> GetCheckResults();
        bool IsHazardTypeExist(int hazardTypeId);
        bool IsMobileActionTypeExist(int IsMobileActionTypeId);
        bool IsResultExist(int resultId);
    }
}
