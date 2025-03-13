using Microsoft.EntityFrameworkCore;
using SmartFarmer.Data.Context;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;
using static SmartFarmer.Core.ViewModel.OperatorViewModel;

namespace SmartFarmer.Data.Repository
{
    public class OperatorRepository : IOperatorRepository
    {
        private readonly SmartFarmerContext _context;
        public OperatorRepository(SmartFarmerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// get Operator list by search with pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public IEnumerable<ApplicationUser> GetOperatorListBySearch(int pageNumber, int pageSize, string searchKey, int? operatorStatusId, Guid? userGroupId, string UserMasterAdminId)
        {
            var mappings = _context.ApplicationUsers.Where(a=> a.MasterAdminId == UserMasterAdminId).Include(a => a.ApplicationUserStatus).Include(a => a.ApplicationUserType).Include(a => a.OperatorStatus).Include(a => a.UserGroup).AsQueryable();
            //if (searchKey != null)
            //{
            //    searchKey = searchKey.ToLower();
            //    applicationUsers = applicationUsers.Where(a => a.FirstName.ToLower().Contains(searchKey) || (a.LastName.ToLower().Contains(searchKey)));
            //}
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                //searchKey = searchKey.ToLower();
                var searchWords = searchKey.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                mappings = mappings.Where(a =>
                    searchWords.Any(word => a.FirstName.ToLower().Contains(word) || a.LastName.ToLower().Contains(word))
                );
            }
            if (operatorStatusId != null)
            {
                mappings = mappings.Where(a => a.OperatorStatusId == operatorStatusId);
            }
            if (userGroupId != null)
            {
                mappings = mappings.Where(a => a.UserGroupId == userGroupId);
            }
            return mappings.Where(a => a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Operator || a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Both && a.ApplicationUserStatusId == (int)Core.Common.Enums.ApplicationUserStatusEnum.Live).OrderBy(a => a.FirstName).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// get Operator count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public int GetOperatorCountBySearch(string searchKey, int? operatorStatusId, Guid? userGroupId,string UserMasterAdminId)
        {
            var mappings = _context.ApplicationUsers.Where(a =>a.MasterAdminId == UserMasterAdminId).AsQueryable();
            //if (searchKey != null)
            //{
            //    searchKey = searchKey.ToLower();
            //    applicationUsers = applicationUsers.Where(a => a.FirstName.ToLower().Contains(searchKey) || (a.LastName.ToLower().Contains(searchKey)));
            //}
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                var searchWords = searchKey.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                mappings = mappings.Where(a =>
                    searchWords.Any(word => a.FirstName.ToLower().Contains(word) || a.LastName.ToLower().Contains(word))
                );
            }
            if (operatorStatusId != null)
            {
                mappings = mappings.Where(a => a.OperatorStatusId == operatorStatusId);
            }
            if (userGroupId != null)
            {
                mappings = mappings.Where(a => a.UserGroupId == userGroupId);
            }
            return mappings.Where(a => a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Operator || a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Both && a.ApplicationUserStatusId == (int)Core.Common.Enums.ApplicationUserStatusEnum.Live).Count();
        }
        
        /// <summary>
        /// get Operator count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public bool IsOperatorStatusExist(int operatorStatusId)
        {
            return _context.OperatorStatuses.Find(operatorStatusId) == null ? false : true ;
        }
        
        /// <summary>
        /// add notification 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public void AddNotification(string userId, string title, string description)
        {
            DateTime dateTime = DateTime.Now;
            Notification notification = new()
            {
                NotificationId = Guid.NewGuid(),
                CreatedDate = dateTime,
                Title = title,
                Description = description,
                ToId = userId,
                IsRead = false
            };
            _context.Notifications.Add(notification);
            _context.SaveChanges();
        }

        /// <summary>
        /// get notification list
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public IEnumerable<Notification> GetAllNotification(string userId)
        {
                return _context.Notifications.Where(a => a.ToId == userId).ToList();
        }
    }
}
