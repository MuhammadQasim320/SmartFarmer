using Microsoft.AspNetCore.Http.HttpResults;
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
    public class FarmService : IFarmService
    {
        private readonly IFarmRepository _farmRepository;
        private readonly IUserRepository _userRepository;
        public FarmService(IFarmRepository farmRepository, IUserRepository userRepository)
        {
            _farmRepository = farmRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// add farm into system
        /// </summary>
        /// <param name="farm"></param>
        /// <returns></returns>
        public FarmResponseViewModel AddFarm(FarmViewModel farmViewModel)
        {
            return Mapper.MapFarmEntityToFarmResponseViewModel(_farmRepository.AddFarm(Mapper.MapFarmViewModelToFarmEntity(farmViewModel)));
        }

        /// <summary>
        /// get farm by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FarmCountRequestViewModel GetFarmListBySearch(string LogInUser, string SearchKey,string email)
        {
            FarmCountRequestViewModel farmList = new FarmCountRequestViewModel();
            farmList.List = _farmRepository.GetFarmListBySearch(SearchKey,email).Select(a => Mapper.MapFarmEntityToFarmResponseViewModel(a)).ToList();
            var user = _userRepository.GetUserDetails(LogInUser);
            foreach (var item in farmList.List)
            {
              var farm=  _farmRepository.GetFarmDetail(item.FarmId);

                if(farm!=null&& user!=null)
                {
                    if (farm.MasterAdminId == user.MasterAdminId)
                    {
                        item.IsWorking = true;
                    }
                    else
                    {
                        item.IsWorking = false;
                    }
                }
            }
            return farmList;
        }

        /// <summary>
        /// check Farm existence
        /// </summary>
        /// <param name="farmId"></param>
        /// <returns></returns>
        public bool IsFarmExist(Guid farmId)
        {
            return _farmRepository.IsFarmExist(farmId);
        }
        
        /// <summary>
        /// Check Is this farm assigned to user
        /// </summary>
        /// <param name="farmId"></param>
        /// <returns></returns>
        public bool IsFarmAssignedToUser(Guid farmId, string email)
        {
            return _farmRepository.IsFarmAssignedToUser(farmId, email);
        }

        /// <summary>
        /// check AlarmAction existence
        /// </summary>
        /// <param name="alarmActionId"></param>
        /// <returns></returns>
        public bool IsAlarmActionExist(Guid alarmActionId)
        {
            return _farmRepository.IsAlarmActionExist(alarmActionId);
        }

        ///// <summary>
        ///// assign farm to user
        ///// </summary>
        ///// <param name="farmId"></param>
        ///// <returns></returns>
        //public bool AssignFarm(Guid farmId, String UserId)
        //{
        //   // return Mapper.MapFarmEntityToFarmResponseViewModel(_farmRepository.GetFarmDetails(farmId));
        //    return _farmRepository.AssignfarmToOperator(farmId, UserId);
        //}
        
        /// <summary>
        /// switch farm to user
        /// </summary>
        /// <param name="farmId"></param>
        /// <returns></returns>
        public bool SwitchFarm(Guid farmId, String email)
        {
            return _farmRepository.SwitchFarm(farmId, email);
        }

        /// <summary>
        /// add training question answers
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>s
        public AddAlarmActionResponseViewModel AddAlarmAction(string CreatedBy, AddAlarmActionRequestViewModel model, string smsNumbersJson)
        {
            if (model.AlarmActionId == null)
            {
                return Mapper.MapAlarmActionEntityToAlarmActionResponseViewModel(_farmRepository.AddAlarmAction(Mapper.MapAddAlarmViewModelToAddAlarmEntity(model, CreatedBy, smsNumbersJson)));
            }
            else
            {
                return Mapper.MapAlarmActionEntityToAlarmActionResponseViewModel(_farmRepository.UpdateAlarmAction(model.AlarmActionId.Value, Mapper.MapNewAlarmActionRequestViewModelToAlarmActionEntity(model, CreatedBy, model.AlarmActionId.Value, smsNumbersJson)));
            } 
        }
        
        /// <summary>
        /// add training question answers
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>s
        public AddAlarmActionResponseViewModel GetAlarmActionDetails(string MasterUserId)
        {
           return Mapper.MapAlarmActionEntityToAlarmActionResponseViewModel(_farmRepository.GetAlarmActionDetails(MasterUserId));
        }
        
        /// <summary>
        /// get login user farm id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>s
        public Guid GetLoginUserFarmId(string MasterUserId)
        {
           return _farmRepository.GetLoginUserFarmId(MasterUserId);
        }

        


             /// <summary>
             /// get farm by search with pagination
             /// </summary>
             /// <param name="model"></param>
             /// <returns></returns>
        public CountFarmResponseViewModel GetFarmListBySearchWithPagination(FarmSearchRequestViewModel model)
        {
            CountFarmResponseViewModel farmList = new CountFarmResponseViewModel();
            farmList.List = _farmRepository.GetFarmListBySearchWithPagiation(model.PageNumber, model.PageSize, model.SearchKey).Select(a => Mapper.MapFarmEntityToFarmDetailViewModel(a)).ToList();
            farmList.TotalCount = _farmRepository.GetFarmCountBySearch(model.SearchKey);
            return farmList;
        }

        
            /// <summary>
            /// get farm details
            /// </summary>
            /// <param name="farmId"></param>
            /// <returns></returns>
        public FarmDetailViewModel GetFarmDetail(Guid farmId)
        {
            return Mapper.MapFarmEntityToFarmDetailViewModel(_farmRepository.GetFarmDetail(farmId));
        }

        /// <summary>
        ///update farm details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FarmDetailViewModel UpdateFarmDetail(FarmDetailViewModel model)
        {
            return Mapper.MapFarmEntityToFarmDetailViewModel(_farmRepository.UpdateFarmDetail(model.FarmId, model.FarmName,model.MasterAdminFirstName,model.MasterAdminLastName));
        }

        /// <summary>
        ///get all farm list
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public FarmNameListViewModel GetAllFarmList()
        {
            FarmNameListViewModel model = new();
            model.List = _farmRepository.GetAllFarmList().Select(a => Mapper.MapFarmEntityToFarmNameListViewModel(a))?.ToList();
            return model;
        }

        /// <summary>
        ///access farm
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool AccessFarm(Guid farmId, string email)
        {
            return _farmRepository.AccessFarm(farmId, email);
        }

        /// <summary>
        /// Check Is this farm assigned to user
        /// </summary>
        /// <param name="farmId"></param>
        /// <returns></returns>
        public bool IsFarmAssigned(Guid farmId, string email)
        {
            return _farmRepository.IsFarmAssigned(farmId, email);
        }

    }
}
