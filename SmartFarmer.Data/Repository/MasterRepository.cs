using SmartFarmer.Data.Context;
using SmartFarmer.Domain;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;

namespace SmartFarmer.Data.Repository
{
    public class MasterRepository : IMasterRepository
    {
        private readonly SmartFarmerContext _context;
        public MasterRepository(SmartFarmerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get Action Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        //public IEnumerable<ActionType> GetActionTypes()
        //{
        //    return _context.ActionTypes.ToList();
        //}

        /// <summary>
        /// Get User Status
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<ApplicationUserStatus> GetUserStatuses()
        {
            return _context.ApplicationUserStatuses.ToList();
        }

        /// <summary>
        /// Get Operator Status
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<OperatorStatus> GetOperatorStatuses()
        {
            return _context.OperatorStatuses.ToList();
        }

        /// <summary>
        /// Get User Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<ApplicationUserType> GetUserTypes()
        {
            return _context.ApplicationUserTypes.Where(a=>a.ApplicationUserTypeId != (int)Core.Common.Enums.ApplicationUserTypeEnum.SuperAdmin && a.ApplicationUserTypeId != (int)Core.Common.Enums.ApplicationUserTypeEnum.MasterAdmin  && a.ApplicationUserTypeId != (int)Core.Common.Enums.ApplicationUserTypeEnum.Unicorn).ToList();
        }

        /// <summary>
        /// Get Check Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<CheckType> GetCheckTypes()
        {
            return _context.CheckTypes.ToList();
        }

        /// <summary>
        /// check the CheckType existence
        /// </summary>
        /// <param name="checkTypeId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsCheckTypeExist(int checkTypeId)
        {
            return _context.CheckTypes.Find(checkTypeId) == null ? false : true;
        }

        /// <summary>
        /// Get Event Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<EventType> GetEventTypes()
        {
            return _context.EventTypes.ToList();
        }

        /// <summary>
        /// Get Frequency Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<FrequencyType> GetFrequencyTypes()
        {
            return _context.FrequencyTypes.ToList();
        }

        /// <summary>
        /// check the FrequencyType existence
        /// </summary>
        /// <param name="frequencyTypeId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsFrequencyTypeExist(int frequencyTypeId)
        {
            return _context.FrequencyTypes.Find(frequencyTypeId) == null ? false : true;
        }

        /// <summary>
        /// Get Training Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<TrainingType> GetTrainingTypes()
        {
            return _context.TrainingTypes.ToList();
        }

        /// <summary>
        /// Get Units Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<UnitsType> GetUnitsTypes()
        {
            return _context.UnitsTypes.ToList();
        }

        /// <summary>
        /// Get InitialRiskAndAdjustedRisk
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<InitialRiskAndAdjustedRisk> GetInitialRiskAndAdjustedRisks()
        {
            return _context.InitialRiskAndAdjustedRisks.ToList();
        }
        
        /// <summary>
        /// Get Machine Statuses
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<MachineStatus> GetMachineStatuses()
        {
            return _context.MachineStatuses.ToList();
        }
        
        /// <summary>
        /// Get Issue Types
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<IssueType> GetIssueTypes()
        {
            return _context.IssueTypes.ToList();
        }
        
        /// <summary>
        /// Get Issue Status
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<IssueStatus> GetIssueStatuses()
        {
            return _context.IssueStatuses.ToList();
        }

        /// <summary>
        /// check eventtype exitance
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsEventTypeExist(int eventTypeId)
        {
            return _context.EventTypes.Find(eventTypeId) == null ? false : true;
        }
        
        /// <summary>
        /// check machineStatus exitance
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsMachineStatusExist(int machineStatusId)
        {
            return _context.MachineStatuses.Find(machineStatusId) == null ? false : true;
        }
        
        /// <summary>
        /// check issueType exitance
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsIssueTypeExist(int issueTypeId)
        {
            return _context.IssueTypes.Find(issueTypeId) == null ? false : true;
        }
        
        /// <summary>
        /// check issueStatus exitance
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsIssueStatusExist(int issueStatusId)
        {
            return _context.IssueStatuses.Find(issueStatusId) == null ? false : true;
        }

        /// <summary>
        /// Get Alarm Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<EventType> GetAlarmTypes()
        {
            return _context.EventTypes.Where(a => a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.SOS || a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Fall || a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Welfare).ToList();
        }

        /// <summary>
        /// Get Hazard Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<HazardType> GetHazardTypes()
        {
            return _context.HazardTypes.ToList();
        }

        /// <summary>
        /// Get MobileAction Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<MobileActionType> GetMobileActionTypes()
        {
            return _context.MobileActionTypes.ToList();
        }
        /// <summary>
        /// Get checkResult
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<CheckResult> GetCheckResults()
        {
            return _context.CheckResults.ToList();
        }

        /// <summary>
        /// check Hazard Type exitance
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsHazardTypeExist(int hazardTypeId)
        {
            return _context.HazardTypes.Find(hazardTypeId) == null ? false : true;
        }

        /// <summary>
        /// check MobileAction Type exitance
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsMobileActionTypeExist(int IsMobileActionTypeId)
        {
            return _context.MobileActionTypes.Find(IsMobileActionTypeId) == null ? false : true;
        }  
        
        /// <summary>
        /// check MobileAction Type exitance
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsResultExist(int resultId)
        {
            return _context.CheckResults.Find(resultId) == null ? false : true;
        }
    }
}
