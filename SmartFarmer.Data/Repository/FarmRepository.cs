using Azure;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmartFarmer.Data.Context;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Data.Repository
{
    public class FarmRepository : IFarmRepository
    {
        private readonly SmartFarmerContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public FarmRepository(SmartFarmerContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// add farm into system
        /// </summary>
        /// <param name="farm"></param>
        /// <returns></returns>
        public Farm AddFarm(Farm farm)
        {
            _context.Farms.Add(farm);
            _context.SaveChanges();
            var response = _context.Farms.Where(a => a.FarmId == farm.FarmId).Include(a => a.CreatedByUser).FirstOrDefault();
            return response;
        }
        /// <summary>
        /// get farm from system
        /// </summary>
        /// <param name="farm"></param>
        /// <returns></returns>
        public Farm GetFarmDetail(Guid farmId)
        {
            return _context.Farms.Where(a => a.FarmId == farmId).Include(a => a.CreatedByUser).Include(a=>a.ApplicationUser).FirstOrDefault();
        }

        /// <summary>
        /// get farm list by search 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public IEnumerable<Farm> GetFarmListBySearch(string searchKey,string email)
        {
            var users = _context.ApplicationUsers.Where(a => a.Email.ToLower() == email.ToLower()).ToList();

            List<Farm> farmList = new List<Farm>();
            foreach (var user in users)
            {
                var farms = _context.Farms
                    .Where(a => a.MasterAdminId == user.MasterAdminId)
                    .Include(a => a.ApplicationUser)
                    .ToList();

                farmList.AddRange(farms);
            }
            if (!string.IsNullOrEmpty(searchKey))
            {
                searchKey = searchKey.ToLower();
                farmList = farmList
                    .Where(a => a.FarmName.ToLower().Contains(searchKey))
                    .ToList();
            }
            return farmList.OrderByDescending(a => a.CreatedDate).ToList();
        }

        ///// <summary>
        ///// get farm count by search 
        ///// </summary>
        ///// <param name="searchKey"></param>
        ///// <returns></returns>
        //public int GetFarmCountBySearch(string searchKey)
        //{
        //    var Farms = _context.Farms.AsQueryable();
        //    if (searchKey != null)
        //    {
        //        searchKey = searchKey.ToLower();
        //        Farms = Farms.Where(a => a.Name.ToLower().Contains(searchKey));
        //    }
        //    return Farms.Count();
        //}

        /// <summary>
        /// check farm existence
        /// </summary>
        /// <param name="farmId"></param>
        /// <returns></returns>
        public bool IsFarmExist(Guid farmId)
        {
            return _context.Farms.Find(farmId) == null ? false : true;
        }
        
        /// <summary>
        /// is this farm assigned to user
        /// </summary>
        /// <param name="farmId"></param>
        /// <returns></returns>
        public bool IsFarmAssignedToUser(Guid farmId, string email)
        {
            var farm = _context.Farms.FirstOrDefault(a => a.FarmId == farmId);
            return _context.ApplicationUsers.Where(a => a.Email == email && a.MasterAdminId == farm.MasterAdminId).FirstOrDefault() == null ? false : true;
        }

        /// <summary>
        /// check alarmAction existence
        /// </summary>
        /// <param name="alarmActionId"></param>
        /// <returns></returns>
        public bool IsAlarmActionExist(Guid alarmActionId)
        {
            return _context.AlarmActions.Find(alarmActionId) == null ? false : true;
        }

        ///// <summary>
        ///// assign training to operator
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public bool AssignfarmToOperator(Guid farmId, string UserId)
        //{
        //    var isMappingExist = _context.FarmOperatorMappings.FirstOrDefault(a=>a.FarmId== farmId && a.OperatorId== UserId);
        //    FarmOperatorMapping farmOperatorMapping = new FarmOperatorMapping();
        //    farmOperatorMapping.FarmId = isMappingExist.FarmId;
        //    farmOperatorMapping.FarmOperatorMappingId = isMappingExist.FarmOperatorMappingId;
        //    farmOperatorMapping.CreatedDate = isMappingExist.CreatedDate;
        //    farmOperatorMapping.OperatorId = isMappingExist.OperatorId;
        //    var farm =  _context.Farms.FirstOrDefault(a => a.FarmId == farmId);
        //    var user = _context.ApplicationUsers.FirstOrDefault(a => a.Id == UserId);
        //    user.MasterAdminId = farm.MasterAdminId;
        //    _context.ApplicationUsers.Update(user);
        //    _context.SaveChanges();
        //    return farmOperatorMapping;
        //}
        
        /// <summary>
        /// switch farm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SwitchFarm(Guid farmId, string email)
        {
            var farm = _context.Farms.FirstOrDefault(a=>a.FarmId==farmId);
            var exist = _context.ApplicationUsers.Where(a => a.Email.ToLower() == email.ToLower() && a.MasterAdminId == farm.MasterAdminId).FirstOrDefault();
            if(exist == null)
            {
                return false;
            }
            var users = _context.ApplicationUsers.Where(a => a.Email.ToLower() == email.ToLower()).ToList();
            users.ForEach(user => user.FarmId = farmId);

            _context.SaveChanges();

            return true;
        }

        /// <summary>
        /// add AlarmAction
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public AlarmAction AddAlarmAction(AlarmAction model)
        {
            _context.AlarmActions.Add(model);
            _context.SaveChanges();
            var result = _context.AlarmActions.Where(a => a.AlarmActionId == model.AlarmActionId).Include(a => a.ApplicationUser).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// update AlarmAction
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public AlarmAction UpdateAlarmAction(Guid alarmActionId, AlarmAction model)
        {
            var alarmAction = _context.AlarmActions.FirstOrDefault(a => a.AlarmActionId == alarmActionId);
            alarmAction.MobileNumber = model.MobileNumber;
            alarmAction.SMS = model.SMS;
            alarmAction.MakeSound = model.MakeSound;
            alarmAction.SmsNumber = model.SmsNumber;
            alarmAction.MobileActionTypeId = model.MobileActionTypeId;
            alarmAction.UpdatedAt = DateTime.Now;
            _context.AlarmActions.Update(alarmAction);
            _context.SaveChanges();
            var result = _context.AlarmActions.Where(a => a.AlarmActionId == alarmActionId).Include(a => a.ApplicationUser).FirstOrDefault();
            return alarmAction;
        }


        /// <summary>
        /// get alarmAction detail 
        /// </summary>
        /// <param name="alarmActionId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public AlarmAction GetAlarmActionDetails(string MasterUserId)
        {
            return _context.AlarmActions.Where(a => a.ApplicationUser.MasterAdminId == MasterUserId).Include(a => a.ApplicationUser).Include(a=>a.MobileActionTypes).FirstOrDefault();
        }

        /// <summary>
        /// get login user farm id 
        /// </summary>
        /// <param name="MasterUserId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Guid GetLoginUserFarmId(string MasterUserId)
        {
            return _context.Farms.FirstOrDefault(a=>a.MasterAdminId == MasterUserId).FarmId;
        }


        /// <summary>
        /// get farm list by search with pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public IEnumerable<Farm> GetFarmListBySearchWithPagiation(int pageNumber, int pageSize, string searchKey)
        {
            var farms = _context.Farms.Include(a => a.ApplicationUser).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                farms = farms.Where(a => a.FarmName.ToLower().Contains(searchKey)).Include(a => a.ApplicationUser);
            }
            return farms.OrderByDescending(a => a.CreatedDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(a => a.CreatedByUser).ToList();
        }



            /// <summary>
            /// get field count by search 
            /// </summary>
            /// <param name="searchKey"></param>
            /// <returns></returns>
            public int GetFarmCountBySearch(string searchKey)
           {
            var farms = _context.Farms.AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                farms = farms.Where(a => a.FarmName.ToLower().Contains(searchKey));
            }
            return farms.Count();
            }
        /// <summary>
        ///update farm details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Farm UpdateFarmDetail(Guid farmId, string name, string masterAdminFirstName, string masterAdminLastName)
        {
            //var res = _context.Farms.Find(farmId);
            var res = _context.Farms.Include(a => a.ApplicationUser).FirstOrDefault(a=>a.FarmId==farmId);
            if (res != null)
            {
                res.FarmName = name;
                if (res.ApplicationUser != null)
                {
                    res.ApplicationUser.FirstName = masterAdminFirstName;
                    res.ApplicationUser.LastName = masterAdminLastName;
                    _context.ApplicationUsers.Update(res.ApplicationUser);
                }
                _context.Farms.Update(res);
                _context.SaveChanges();
                var response = _context.Farms.Where(a => a.FarmId == farmId).Include(a => a.CreatedByUser).FirstOrDefault();
                return response;
            }
            return null;
        }

        /// <summary>
        ///get all farm list
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<Farm> GetAllFarmList()
        {
            return _context.Farms.OrderBy(a => a.FarmName).ToList();
        }




        /// <summary>
        /// access farm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AccessFarm(Guid farmId, string email)
        {
            var farm = _context.Farms.FirstOrDefault(a => a.FarmId == farmId);
            var user = _context.ApplicationUsers.Where(a => a.Email.ToLower() == email.ToLower()).FirstOrDefault();
            if(user != null)
            {
                if (farm != null)
                {
                    user.FarmId = farm.FarmId;
                    user.MainAdminId = farm.MasterAdminId;
                    user.MasterAdminId = farm.MasterAdminId;
                    _context.ApplicationUsers.Update(user);
                } 

                _context.SaveChanges();

                return true;
            }

            return false;
        }


        /// <summary>
        /// is this farm assigned to user
        /// </summary>
        /// <param name="farmId"></param>
        /// <returns></returns>
        public bool IsFarmAssigned(Guid farmId, string email)
        {
            var farm = _context.Farms.FirstOrDefault(a => a.FarmId == farmId);
            return _context.ApplicationUsers.Where(a => a.Email == email).FirstOrDefault() == null ? false : true;
        }
    }
}
