using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Interface;
using static SmartFarmer.Core.ViewModel.MachinePreCheckHistoryViewModel;
using static SmartFarmer.Core.ViewModel.SearchEquipmentHistoryRequestViewModel;

namespace SmartFarmer.Core.Service
{
    public class EquipmentService : IEquipmentService
    {

        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMachineRepository _machineRepository;
        private readonly ICheckListRepository _checkListRepository;
        public EquipmentService(IEquipmentRepository equipmentRepository, IEventRepository eventRepository, IMachineRepository machineRepository, ICheckListRepository checkListRepository)
        {
            _equipmentRepository = equipmentRepository;
            _eventRepository = eventRepository;
            _machineRepository = machineRepository;
            _checkListRepository = checkListRepository;
        }

        /// <summary>
        /// get equipment by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public EquipmentListViewModel GetEquipmentListBySearchWithPagination(SearchEquipmentRequestViewModel model, string loginUserMasterAdminId)
        {

            var equipmentList = new EquipmentListViewModel();
            var equipments = _equipmentRepository.GetEquipmentListBySearch(model.PageNumber, model.PageSize, model.SearchKey, model.EquipmentStatusId, model.HasIssues,model.isOUtOfSeason, loginUserMasterAdminId);

            // Initialize a list to hold EquipmentDetailsViewModel instances
            var equipmentDetailsList = new List<EquipmentDetailsViewModel>();

            foreach (var Equipment in equipments)
            {
                var lastOperator = _machineRepository.GetMachineLastOperator(Equipment.MachineId);
                var lastCheck = Mapper.MapCheckListMachineMappingEntityToStartCheckListResponseViewModel(_checkListRepository.GetMachineLastCheckList(Equipment.MachineId));
                // Create a new instance for each equipment
                var equipmentDetailsViewModel = new EquipmentDetailsViewModel()
                {
                    EquipmentDetails = Mapper.MapMachineEntityToEquipmentResponseViewModel(Equipment),
                    LastEventDetails = Mapper.MapEventEntityToLastEventViewModel(_eventRepository.GetLastEventByMachineId(Equipment.MachineId)),
                    LastCheck = lastCheck?.CreatedDate,
                    LastOperatorId = lastOperator?.OperatorId,
                    LastOperatorName = lastOperator?.Operator?.FirstName + " " + lastOperator?.Operator?.LastName,
                    Issues = _machineRepository.GetMachineIssuesCount(Equipment.MachineId, loginUserMasterAdminId)
                };

                // Add the populated instance to the list
                equipmentDetailsList.Add(equipmentDetailsViewModel);
            }

            // Assign the list of details view models to equipmentList.List
            equipmentList.List = equipmentDetailsList;
            equipmentList.TotalCount = _equipmentRepository.GetEquipmentCountBySearch(model.SearchKey, model.EquipmentStatusId, model.HasIssues, model.isOUtOfSeason, loginUserMasterAdminId);

            return equipmentList;
        }

        /// <summary>
        /// get equipment history by machineId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MachineHistoryViewModel GetEquipmentHistory(Guid machineId, SearchEquipmentHistoryRequestViewModel model, string UserMasterAdminId)
        {
            MachineHistoryViewModel machineHistoryViewModel = new();

            machineHistoryViewModel.List = _equipmentRepository.GetEquipmentHistory(machineId, model.PageNumber, model.PageSize, model.EventTypeId, UserMasterAdminId).Select(a => Mapper.MapMachineHistoryEntityToMachineHistoryViewModel(a))?.ToList();
            machineHistoryViewModel.TotalCount = _equipmentRepository.GetEquipmentHistoryCountBySearch(machineId,model.EventTypeId, UserMasterAdminId);
            return machineHistoryViewModel;
        }



        /// <summary>
        /// get equipment precheck history by machineId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MachinePreCheckHistoryViewModel GetEquipmentPreCheckHistory(SearchEquipmentPreCheckHistoryRequestViewModel model, string userMasterAdminId)
        {
            MachinePreCheckHistoryViewModel machineHistoryViewModel = new()
            {
                List = new List<PreCheckEventResponseViewModel>(),
                TotalCount = 0
            };

            var machines = _machineRepository.GetMachineNameList(userMasterAdminId);

            foreach (var machine in machines)
            {
                var historyList = _equipmentRepository.GetEquipmentPreCheckHistory(machine.MachineId, model.SearchKey, model.MachineId, model.ResultId, userMasterAdminId).Select(a => Mapper.MapMachineHistoryEntityToMachinePreCheckHistoryViewModel(a)).ToList().OrderByDescending(a => a.CreatedDate);

                if (historyList != null)
                {
                    ((List<PreCheckEventResponseViewModel>)machineHistoryViewModel.List).AddRange(historyList);
                }

                machineHistoryViewModel.TotalCount += _equipmentRepository.GetEquipmentPreCheckHistoryCountBySearch(machine.MachineId, model.SearchKey, model.MachineId, model.ResultId, userMasterAdminId);
            }

            return machineHistoryViewModel;
        }



        ///// <summary>
        ///// get equipment precheck history by machineId
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public MachinePreCheckHistoryViewModel GetEquipmentPreCheckHistory(SearchEquipmentPreCheckHistoryRequestViewModel model, string userMasterAdminId)
        //{
        //    var machines = _machineRepository.GetMachineNameList(userMasterAdminId);

        //    var historyList = machines.SelectMany(machine => _equipmentRepository.GetEquipmentPreCheckHistory(machine.MachineId, model.PageNumber, model.PageSize, model.SearchKey, model.MachineId,model.ResultId, userMasterAdminId).Select(a => Mapper.MapMachineHistoryEntityToMachinePreCheckHistoryViewModel(a))).ToList();

        //    int totalCount = machines.Sum(machine => _equipmentRepository.GetEquipmentPreCheckHistoryCountBySearch(machine.MachineId, model.SearchKey, model.MachineId,model.ResultId, userMasterAdminId));

        //    return new MachinePreCheckHistoryViewModel
        //    {
        //        List = historyList,
        //        TotalCount = totalCount
        //    };
        //}


    }
}
