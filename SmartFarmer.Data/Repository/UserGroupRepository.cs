using Microsoft.EntityFrameworkCore;
using SmartFarmer.Data.Context;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;

namespace SmartFarmer.Data.Repository
{
    public class UserGroupRepository : IUserGroupRepository
    {
        private readonly SmartFarmerContext _context;
        public UserGroupRepository(SmartFarmerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Is userGroup Exist
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsUserGroupExist(Guid userGroupId)
        {
            return _context.UserGroups.Find(userGroupId) == null ? false : true;
        }

        /// <summary>
        /// add userGroup into system
        /// </summary>
        /// <param name="userGroup"></param>
        /// <returns></returns>
        public UserGroup AddUserGroup(UserGroup model)
        {
            model.UserGroupId = Guid.NewGuid();
            model.CreatedDate = DateTime.Now;
            _context.UserGroups.Add(model);
            _context.SaveChanges();
            var response = _context.UserGroups.Where(a => a.UserGroupId == model.UserGroupId).Include(a => a.ApplicationUser).FirstOrDefault();
            return response;
        }

        /// <summary>
        /// get userGroup list by search with pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public IEnumerable<UserGroup> GetUserGroupListBySearch(int pageNumber, int pageSize, string searchKey, string UserMasterAdminId)
        {
            var userGroups = _context.UserGroups.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).Include(a => a.ApplicationUser).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                userGroups = userGroups.Where(a => a.GroupName.ToLower().Contains(searchKey)).Include(a => a.ApplicationUser);
            }
            return userGroups.Skip((pageNumber - 1) * pageSize).Take(pageSize).OrderBy(a => a.GroupName).ToList();
        }

        /// <summary>
        /// get userGroup count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public int GetUserGroupCountBySearch(string searchKey,string UserMasterAdminId)
        {
            var userGroups = _context.UserGroups.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                userGroups = userGroups.Where(a => a.GroupName.ToLower().Contains(searchKey));
            }
            return userGroups.Count();
        }

        /// <summary>
        /// get userGroup details 
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        public UserGroup GetUserGroupDetails(Guid userGroupId)
        {
            return _context.UserGroups.Where(a => a.UserGroupId == userGroupId).Include(a => a.ApplicationUser).FirstOrDefault();
        }

        /// <summary>
        /// update userGroup details 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public UserGroup UpdateUserGroupDetails(Guid userGroupId, UserGroup model)
        {
            var res = _context.UserGroups.Find(userGroupId);
            if (res != null)
            {
                res.GroupName = model.GroupName;
                res.Description = model.Description;
                _context.UserGroups.Update(res);
                _context.SaveChanges();
                var response = _context.UserGroups.Where(a => a.UserGroupId == userGroupId).Include(a => a.ApplicationUser).FirstOrDefault();
                return response;
            }
            return null;
        }

        /// <summary>
        /// Get UserGroup list 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<UserGroup> GetUserGroupList(string UserMasterAdminId)
        {
            var userGroup = _context.UserGroups.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).AsQueryable();
            return userGroup.OrderBy(a=>a.GroupName).ToList();
        }




        /// <summary>
        /// delete userGroup
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        public bool DeleteUserGroup(Guid userGroupId)
        {
            var userGroup = _context.UserGroups.FirstOrDefault(m => m.UserGroupId == userGroupId);
            if (userGroup != null)
            {
                _context.UserGroups.Remove(userGroup);
                _context.SaveChanges();
                return true;
            }
            return false;

        }



        /// <summary>
        /// get user details 
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        public ApplicationUser GetUserDetails(Guid userGroupId)
        {
            return _context.ApplicationUsers.Where(a => a.UserGroupId == userGroupId).Include(a => a.UserGroup).FirstOrDefault();
        }


        /// <summary>
        /// get welfareRoutine details 
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        public WelfareRoutine GetWelfareRoutineDetails(Guid userGroupId)
        {
            return _context.WelfareRoutines.Where(a => a.UserGroupId == userGroupId).Include(a => a.UserGroup).Include(a => a.ApplicationUser).FirstOrDefault();
        }
    }
}
