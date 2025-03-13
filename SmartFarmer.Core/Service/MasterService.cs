using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Domain.Interface;
using static SmartFarmer.Core.ViewModel.MasterViewModel;

namespace SmartFarmer.Core.Service
{
    public class MasterService : IMasterService
    {
        private readonly IMasterRepository _masterRepository;
        public MasterService(IMasterRepository masterRepository)
        {
            _masterRepository = masterRepository;
        }

        ///// <summary>
        ///// Get Action Type
        ///// </summary>
        ///// <returns></returns>
        ///// <response code="400">If the item is null</response>
        //public IEnumerable<ActionTypeViewModel> GetActionTypes()
        //{
        //    return _masterRepository.GetActionTypes().Select(a => Mapper.MapActionTypeEntityToActionTypeViewModel(a)).ToList();
        //}

        /// <summary>
        /// Get User Status
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<UserStatusViewModel> GetUserStatuses()
        {
            return _masterRepository.GetUserStatuses().Select(a => Mapper.MapUserStatusEntityToUserStatusViewModel(a)).ToList();
        }

        /// <summary>
        /// Get Operator Status
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<OperatorStatusViewModel> GetOperatorStatuses()
        {
            return _masterRepository.GetOperatorStatuses().Select(a => Mapper.MapOperatorStatusEntityToOperatorStatusViewModel(a)).ToList();
        }

        /// <summary>
        /// Get User Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<UserTypeViewModel> GetUserTypes()
        {
            return _masterRepository.GetUserTypes().Select(a => Mapper.MapUserTypeEntityToUserTypeViewModel(a)).ToList();
        }

        /// <summary>
        /// Get Check Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<CheckTypeViewModel> GetCheckTypes()
        {
            return _masterRepository.GetCheckTypes().Select(a => Mapper.MapCheckTypeEntityToCheckTypeViewModel(a)).ToList();
        }

        /// <summary>
        /// Is CheckType Exist
        /// </summary>
        /// <param name="checkTypeId"></param>
        /// <returns></returns>
        public bool IsCheckTypeExist(int checkTypeId)
        {
            return _masterRepository.IsCheckTypeExist(checkTypeId);
        }

        /// <summary>
        /// Get Event Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<EventTypeViewModel> GetEventTypes()
        {
            return _masterRepository.GetEventTypes().Select(a => Mapper.MapEventTypeEntityToEventTypeViewModel(a)).ToList();
        }

        /// <summary>
        /// Get Frequency Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<FrequencyTypeViewModel> GetFrequencyTypes()
        {
            return _masterRepository.GetFrequencyTypes().Select(a => Mapper.MapFrequencyTypeEntityToFrequencyTypeViewModel(a)).ToList();
        }

        /// Is FrequencyType Exist
        /// </summary>
        /// <param name="frequencyTypeId"></param>
        /// <returns></returns>
        public bool IsFrequencyTypeExist(int frequencyTypeId)
        {
            return _masterRepository.IsFrequencyTypeExist(frequencyTypeId);
        }

        /// <summary>
        /// Get Training Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<TrainingTypeViewModel> GetTrainingTypes()
        {
            return _masterRepository.GetTrainingTypes().Select(a => Mapper.MapTrainingTypeEntityToTrainingTypeViewModel(a)).ToList();
        }

        /// <summary>
        /// Get Units Type
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<UnitsTypeViewModel> GetUnitsTypes()
        {
            return _masterRepository.GetUnitsTypes().Select(a => Mapper.MapUnitsTypeEntityToUnitsTypeViewModel(a)).ToList();
        }

        /// <summary>
        /// Get InitialRiskAndAdjustedRisk
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<InitialRiskAndAdjustedRiskViewModel> GetInitialRiskAndAdjustedRisks()
        {
            return _masterRepository.GetInitialRiskAndAdjustedRisks().Select(a => Mapper.MapInitialRiskAndAdjustedRiskEntityToInitialRiskAndAdjustedRiskViewModel(a)).ToList();
        }
        
        /// <summary>
        /// Get Machine Statuses
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<MachineStatusViewModel> GetMachineStatuses()
        {
            return _masterRepository.GetMachineStatuses().Select(a => Mapper.MapMachineStatusEntityToMachineStatusViewModel(a)).ToList();
        }
        
        /// <summary>
        /// Get Issue Types
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<IssueTypeViewModel> GetIssueTypes()
        {
            return _masterRepository.GetIssueTypes().Select(a => Mapper.MapIssueTypeEntityToIssueTypeViewModel(a)).ToList();
        }
        
        /// <summary>
        /// Get Issue Statuses
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<IssueStatusViewModel> GetIssueStatuses()
        {
            return _masterRepository.GetIssueStatuses().Select(a => Mapper.MapIssueStatusEntityToIssueStatusViewModel(a)).ToList();
        }

        /// <summary>
        /// check eventtype existance
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsEventTypeExist(int eventTypeId)
        {
            return _masterRepository.IsEventTypeExist(eventTypeId);
        }
        
        /// <summary>
        /// check machineStatus existance
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsMachineStatusExist(int machineStatusId)
        {
            return _masterRepository.IsMachineStatusExist(machineStatusId);
        }
        
        /// <summary>
        /// check issueType existance
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsIssueTypeExist(int issueTypeId)
        {
            return _masterRepository.IsIssueTypeExist(issueTypeId);
        }
        
        /// <summary>
        /// check issueStatus existance
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsIssueStatusExist(int issueStatusId)
        {
            return _masterRepository.IsIssueStatusExist(issueStatusId);
        }

        /// <summary>
        /// Get Alarm Types
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<EventTypeViewModel> GetAlarmTypes()
        {
            return _masterRepository.GetAlarmTypes().Select(a => Mapper.MapEventTypeEntityToEventTypeViewModel(a)).ToList();
        }

        /// <summary>
        /// Get Hazard Types
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<HazardTypeViewModel> GetHazardTypes()
        {
            return _masterRepository.GetHazardTypes().Select(a => Mapper.MapHazardTypeEntityToHazardTypeViewModel(a)).ToList();
        }


        /// <summary>
        /// Get MobileAction Types
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<MobileActionTypeViewModel> GetMobileActionTypes()
        {
            return _masterRepository.GetMobileActionTypes().Select(a => Mapper.MapMobileActionTypeEntityToMobileActionTypeViewModel(a)).ToList();
        }

        /// <summary>
        /// Get MobileAction Types
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public IEnumerable<CheckResultViewModel> GetCheckResults()
        {
            return _masterRepository.GetCheckResults().Select(a => Mapper.MapCheckResultEntityToCheckResultViewModel(a)).ToList();
        }


        /// <summary>
        /// check Hazard Type existance
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsHazardTypeExist(int hazardTypeId)
        {
            return _masterRepository.IsHazardTypeExist(hazardTypeId);
        }

        /// <summary>
        /// check MobileAction Type existance
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsMobileActionTypeExist(int IsMobileActionTypeId)
        {
            return _masterRepository.IsMobileActionTypeExist(IsMobileActionTypeId);
        }

        /// <summary>
        /// check result existance
        /// </summary>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsResultExist(int resultId)
        {
            return _masterRepository.IsResultExist(resultId);
        }
    }
}
