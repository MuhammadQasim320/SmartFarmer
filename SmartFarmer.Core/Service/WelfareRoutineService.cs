using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Interface;

namespace SmartFarmer.Core.Service
{
    public class WelfareRoutineService : IWelfareRoutineService
    {
        private readonly IWelfareRoutineRepository _welfareRoutineRepository;
        public WelfareRoutineService(IWelfareRoutineRepository welfareRoutineRepository)
        {
            _welfareRoutineRepository = welfareRoutineRepository;
        }

        /// <summary>
        /// check welfare routine existance
        /// </summary>
        /// <param name="welfareRoutineId"></param>
        /// <returns></returns>
        public bool IsWelfareRoutineExists(Guid welfareRoutineId)
        {
            return _welfareRoutineRepository.IsWelfareRoutineExists(welfareRoutineId);
        }

        /// <summary>
        /// add welfareRoutine into system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WelfareRoutineResponseViewModel AddWelfareRoutine(string CreatedBy, WelfareRoutineRequestViewModel model)
        {
            return Mapper.MapWelfareRoutineEntityToWelfareRoutineResponseViewModel(_welfareRoutineRepository.AddWelfareRoutine(Mapper.MapWelfareRoutineRequestViewModelToWelfareRoutineEntity(CreatedBy,model)));
        }

        /// <summary>
        /// get welfareRoutine by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WelfareRoutineCountRequestViewModel GetWelfareRoutineListBySearchWithPagination(string UserMasterAdminId, SearchWelfareRoutineRequestViewModel model)
        {
            WelfareRoutineCountRequestViewModel welfareRoutineList = new WelfareRoutineCountRequestViewModel();
            welfareRoutineList.List = _welfareRoutineRepository.GetWelfareRoutineListBySearch(model.PageNumber, model.PageSize, model.SearchKey, UserMasterAdminId).Select(a => Mapper.MapWelfareRoutineEntityToWelfareRoutineResponseViewModel(a)).ToList();
            welfareRoutineList.TotalCount = _welfareRoutineRepository.GetWelfareRoutineCountBySearch(model.SearchKey, UserMasterAdminId);
            return welfareRoutineList;
        }

        /// <summary>
        /// get welfareRoutine by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WelfareRoutineResponseViewModel GetWelfareRoutineDetails(Guid welfareRoutineId)
        {
            return Mapper.MapWelfareRoutineEntityToWelfareRoutineResponseViewModel(_welfareRoutineRepository.GetWelfareRoutineDetails(welfareRoutineId));
        }

        /// <summary>
        /// update welfare routine details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WelfareRoutineResponseViewModel UpdateWelfareRoutineDetail(WelfareRoutineResponseViewModel model)
        {
            return Mapper.MapWelfareRoutineEntityToWelfareRoutineResponseViewModel(_welfareRoutineRepository.UpdateWelfareRoutineDetail(model.WelfareRoutineId, Mapper.MapWelfareRoutineRequestViewModelToWelfareRoutineEntity(model)));
        }


        /// <summary>
        /// Is UserGroup Exist in welfareRoutine
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsGroupAssignedToOtherWelfareRoutine(Guid userGroupId)
        {
            return _welfareRoutineRepository.IsGroupAssignedToOtherWelfareRoutine(userGroupId);
        }  
        
        /// <summary>
        /// Is UserGroup Exist in welfareRoutine
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsGroupAssignedToWelfareRoutine(Guid userGroupId, Guid welfareRoutineId)
        {
            return _welfareRoutineRepository.IsGroupAssignedToWelfareRoutine(userGroupId, welfareRoutineId);
        } 
        /// <summary>
        /// Is UserGroup Exist in welfareRoutine
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsGroupAssigned(Guid userGroupId, Guid welfareRoutineId)
        {
            return _welfareRoutineRepository.IsGroupAssigned(userGroupId, welfareRoutineId);
        }
    }
}
