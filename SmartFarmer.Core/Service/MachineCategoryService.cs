using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Core.Service
{
    public class MachineCategoryService : IMachineCategoryService
    {
        private readonly IMachineCategoryRepository _machineCategoryRepository;
        public MachineCategoryService(IMachineCategoryRepository machineCategoryRepository)
        {
            _machineCategoryRepository = machineCategoryRepository;
        }

        /// <summary>
        /// add MachineCategory into system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MachineCategoryResponseViewModel AddMachineCategory(string CreatedBy, MachineCategoryRequestViewModel model)
        {
            return Mapper.MapMachineCategoryEntityToMachineCategoryResponseViewModel(_machineCategoryRepository.AddMachineCategory(Mapper.MapMachineCategoryRequestViewModelToMachineCategoryEntity(CreatedBy,model)));
        }

        /// <summary>
        /// get machineCategory by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MachineCategoryCountRequestViewModel GetMachineCategoryListBySearchWithPagination(string masterAdminId,SearchMachineCategoryRequestViewModel model)
        {
            MachineCategoryCountRequestViewModel machineCategoryList = new MachineCategoryCountRequestViewModel();
            machineCategoryList.List = _machineCategoryRepository.GetMachineCategoryListBySearch(masterAdminId,model.PageNumber, model.PageSize, model.SearchKey).Select(a => Mapper.MapMachineCategoryEntityToMachineCategoryResponseViewModel(a)).ToList();
            machineCategoryList.TotalCount = _machineCategoryRepository.GetMachineCategoryCountBySearch(masterAdminId,model.SearchKey);
            return machineCategoryList;
        }

        /// <summary>
        /// check machineCategory existence
        /// </summary>
        /// <param name="machineCategoryId"></param>
        /// <returns></returns>
        public bool IsMachineCategoryExist(Guid machineCategoryId)
        {
            return _machineCategoryRepository.IsMachineCategoryExist(machineCategoryId);
        }
        /// <summary>
        /// check machineCategory essign
        /// </summary>
        /// <param name="machineCategoryId"></param>
        /// <returns></returns>
        public bool IsMachineCategoryAssign(Guid machineCategoryId)
        {
            return _machineCategoryRepository.IsMachineCategoryAssign(machineCategoryId);
        }

        /// <summary>
        /// get machineCategory details
        /// </summary>
        /// <param name="machineCategoryId"></param>
        /// <returns></returns>
        public MachineCategoryResponseViewModel GetMachineCategoryDetails(Guid machineCategoryId)
        {
            return Mapper.MapMachineCategoryEntityToMachineCategoryResponseViewModel(_machineCategoryRepository.GetMachineCategoryDetails(machineCategoryId));
        }

        /// <summary>
        ///update machineCategory details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MachineCategoryResponseViewModel UpdateMachineCategoryDetails(MachineCategoryResponseViewModel model)
        {
            return Mapper.MapMachineCategoryEntityToMachineCategoryResponseViewModel(_machineCategoryRepository.UpdateMachineCategoryDetails(model.MachineCategoryId, Mapper.MapMachineCategoryResponseViewModelToMachineCategoryEntity(model)));
        }

        /// <summary>
        /// delete machineCategory 
        /// </summary>
        /// <param name="machineCategoryId"></param>
        /// <returns></returns>
        public bool DeleteMachineCategory(Guid machineCategoryId)
        {
            return _machineCategoryRepository.DeleteMachineCategory(machineCategoryId);
        }

        /// <summary>
        /// get MachineCategory name list 
        /// </summary>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public MachineCategoryNameListViewModel GetMachineCategoryNameList(string masterAdminId)
        {
            MachineCategoryNameListViewModel machineCategoryNameListViewModel = new();
            machineCategoryNameListViewModel.List = _machineCategoryRepository.GetMachineCategoryNameList(masterAdminId).Select(a => Mapper.MapMachineCategoryEntityToMachineCategoryNameViewModel(a))?.ToList();
            return machineCategoryNameListViewModel;
        }
    }
}
