using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Interface;

namespace SmartFarmer.Core.Service
{
    public class MachineTypeService : IMachineTypeService
    {
        private IMachineTypeRepository _machineTypeRepository;
        public MachineTypeService(IMachineTypeRepository machineType)
        {
            _machineTypeRepository = machineType;
        }

        /// <summary>
        /// Add MachineType
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public MachineTypeViewModel AddMachineType(string CreatedBy, MachineTypeRequestViewModel model)
        {
            return Mapper.MapMachineTypeToMachineTypeViewModel(_machineTypeRepository.AddMachineType(Mapper.MapMachineTypeRequestViewModelToMachineType(CreatedBy,model)));
        }

        /// <summary>
        /// get MachineType deatils 
        /// </summary>
        /// <param name="machineTypeId"></param>
        /// <returns></returns>
        public MachineTypeViewModel GetMachineTypeDetails(Guid machineTypeId)
        {
            return Mapper.MapMachineTypeToMachineTypeViewModel(_machineTypeRepository.GetMachineTypeDetails(machineTypeId));
        }

        /// <summary>
        /// Get MachineType List By Search With Pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MachineTypeSearchResponseViewModel GetMachineTypeListBySearchWithPagination(string masterAdminId,MachineTypeSearchRequestViewModel model)
        {
            MachineTypeSearchResponseViewModel machineTypeSearchResponse = new();
            machineTypeSearchResponse.List = _machineTypeRepository.GetMachineTypeListBySearch(masterAdminId,model.PageNumber, model.PageSize, model.SearchKey, model.NeedsTraining, model.RiskAssessmentId)?.Select(a => Mapper.MapMachineTypeToMachineTypeViewModel(a))?.ToList();
            machineTypeSearchResponse.TotalCount = _machineTypeRepository.GetMachineTypeCountBySearch(masterAdminId,model.SearchKey, model.NeedsTraining, model.RiskAssessmentId);
            return machineTypeSearchResponse;
        }

        /// <summary>
        /// Is MachineType Exist
        /// </summary>
        /// <param name="machineTypeId"></param>
        /// <returns></returns>
        public bool IsMachineTypeExist(Guid machineTypeId)
        {
            return _machineTypeRepository.IsMachineTypeExist(machineTypeId);
        }
        /// <summary>
        /// Is MachineStatus Exist
        /// </summary>
        /// <param name="machineStatusId"></param>
        /// <returns></returns>
        public bool IsMachineStatusExist(int machineStatusId)
        {
            return _machineTypeRepository.IsMachineStatusExist(machineStatusId);
        }

        /// <summary>
        /// Is UnitsType Exist
        /// </summary>
        /// <param name="unitsTypeId"></param>
        /// <returns></returns>
        public bool IsUnitsTypeExist(int unitsTypeId)
        {
            return _machineTypeRepository.IsUnitsTypeExist(unitsTypeId);
        }

        /// <summary>
        /// Update MachineType Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MachineTypeViewModel UpdateMachineTypeDetails(MachineTypeViewModel model)
        {
            return Mapper.MapMachineTypeToMachineTypeViewModel(_machineTypeRepository.UpdateMachineTypeDetails(Mapper.MapMachineTypeViewModelToMachineType(model)));
        }

        /// <summary>
        ///get machine type name list
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public MachineTypeNameListViewModel GetMachineTypeNameList(string masterAdminId)
        {
            MachineTypeNameListViewModel machineTypeNameListViewModel = new();
            machineTypeNameListViewModel.List = _machineTypeRepository.GetMachineTypeNameList( masterAdminId).Select(a => Mapper.MapMachineTypeEntityToMachineTypeNameViewModel(a))?.ToList();
            return machineTypeNameListViewModel;
        }
    }
}
