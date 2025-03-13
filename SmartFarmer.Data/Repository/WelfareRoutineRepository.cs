using Microsoft.EntityFrameworkCore;
using SmartFarmer.Data.Context;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;

namespace SmartFarmer.Data.Repository
{
    public class WelfareRoutineRepository : IWelfareRoutineRepository
    {
        private readonly SmartFarmerContext _context;
        public WelfareRoutineRepository(SmartFarmerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// check WelfareRoutine existance
        /// </summary>
        /// <param name="welfareRoutineId"></param>
        /// <returns></returns>
        public bool IsWelfareRoutineExists(Guid welfareRoutineId)
        {
            return _context.WelfareRoutines.Find(welfareRoutineId) == null ? false : true;
        }

        /// <summary>
        /// add welfareRoutine into system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WelfareRoutine AddWelfareRoutine(WelfareRoutine model)
        {
            model.WelfareRoutineId = Guid.NewGuid();
            model.CreatedDate = DateTime.Now;
            _context.WelfareRoutines.Add(model);
            _context.SaveChanges();
            var response = _context.WelfareRoutines.Where(a => a.WelfareRoutineId == model.WelfareRoutineId).Include(a => a.ApplicationUser).FirstOrDefault();
            return response;
        }

        /// <summary>
        /// get welfareRoutine list by search with pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public IEnumerable<WelfareRoutine> GetWelfareRoutineListBySearch(int pageNumber, int pageSize, string searchKey, string UserMasterAdminId)
        {
            var welfareRoutines = _context.WelfareRoutines.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).Include(a => a.ApplicationUser).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                welfareRoutines = welfareRoutines.Where(a => a.Name.ToLower().Contains(searchKey)).Include(a => a.ApplicationUser);
            }
            return welfareRoutines.Include(a => a.UserGroup).Skip((pageNumber - 1) * pageSize).Take(pageSize).OrderBy(a => a.Name).ToList();
        }

        /// <summary>
        /// get welfareRoutine count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public int GetWelfareRoutineCountBySearch(string searchKey,string UserMasterAdminId)
        {
            var welfareRoutines = _context.WelfareRoutines.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                welfareRoutines = welfareRoutines.Where(a => a.Name.ToLower().Contains(searchKey));
            }
            return welfareRoutines.Count();
        }

        /// <summary>
        /// get welfareRoutine details 
        /// </summary>
        /// <param name="welfareRoutineId"></param>
        /// <returns></returns>
        public WelfareRoutine GetWelfareRoutineDetails(Guid welfareRoutineId)
        {
            return _context.WelfareRoutines.Where(a => a.WelfareRoutineId == welfareRoutineId).Include(a => a.UserGroup).Include(a => a.ApplicationUser).FirstOrDefault();
        }

        /// <summary>
        /// update welfare routine details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WelfareRoutine UpdateWelfareRoutineDetail(Guid welfareRoutineId, WelfareRoutine model)
        {
            var res = _context.WelfareRoutines.FirstOrDefault(a => a.WelfareRoutineId == welfareRoutineId);
            if (res != null)
            {
                res.UserGroupId = model?.UserGroupId;
                res.Minutes = model.Minutes;
                res.Name = model.Name;
                _context.WelfareRoutines.Update(res);
                _context.SaveChanges();
                var response = _context.WelfareRoutines.Where(a => a.WelfareRoutineId == welfareRoutineId).Include(a => a.ApplicationUser).FirstOrDefault();
                return response;
            }
            return null;
        }
        
        
        /// <summary>
        /// get welfare routine details by groupId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WelfareRoutine GetWelfareRoutineDetail(Guid? groupId)
        {
                return _context.WelfareRoutines.Where(a => a.UserGroupId == groupId).FirstOrDefault();
        }


        /// <summary>
        /// Is userGroup Exist in welfareRoutine
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsGroupAssignedToOtherWelfareRoutine(Guid userGroupId)
        {
           // return _context.WelfareRoutines.Find(userGroupId) == null ? false : true;
            return _context.WelfareRoutines.Any(a => a.UserGroupId == userGroupId);
        }


        /// <summary>
        /// Is userGroup Exist in welfareRoutine
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsGroupAssignedToWelfareRoutine(Guid userGroupId, Guid welfareRoutineId)
        {
            // return _context.WelfareRoutines.Where(a => a.UserGroupId == userGroupId && a.WelfareRoutineId == welfareRoutineId)==null?false:true;
            return _context.WelfareRoutines.Any(a => a.UserGroupId == userGroupId && a.WelfareRoutineId == welfareRoutineId);
        }
        /// <summary>
        /// Is userGroup Exist in welfareRoutine
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsGroupAssigned(Guid userGroupId, Guid welfareRoutineId)
        {
            return _context.WelfareRoutines.Any(a => a.UserGroupId == userGroupId && a.WelfareRoutineId != welfareRoutineId);
        }
    }
}
