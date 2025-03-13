using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartFarmer.Data.Context;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;

namespace SmartFarmer.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly SmartFarmerContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserRepository(SmartFarmerContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Is User Exist
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsUserExist(string userId)
        {
            return _context.ApplicationUsers.Find(userId) == null ? false : true;
        }

        /// <summary>
        /// Is User Type Exist
        /// </summary>
        /// <param name="userTypeId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsUserTypeExist(int userTypeId)
        {
            return _context.ApplicationUserTypes.Find(userTypeId) == null ? false : true;
        }

        /// <summary>
        /// Is User Status Exist
        /// </summary>
        /// <param name="userStatusId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsUserStatusExist(int userStatusId)
        {
            return _context.ApplicationUserStatuses.Find(userStatusId) == null ? false : true;
        }
        
        /// <summary>
        /// Is Opertor Exist
        /// </summary>
        /// <param name="userStatusId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsOperatorExist(string userId)
        {
            return _context.ApplicationUsers.Where(a=>a.Id == userId && (a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Operator) || a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Both) == null ? false : true;
        }

        /// <summary>
        /// get User role
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public string GetUserRole(string userId)
        {
            return _context.ApplicationUsers.Where(a => a.Id == userId).Include(a => a.ApplicationUserType).FirstOrDefault()?.ApplicationUserType?.Type;
        }

        /// <summary>
        /// Is Email Exist
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool CheckUserEmailExistence(string email)
        {
            return _context.ApplicationUsers.Any(u => u.Email == email);
        }
        
        /// <summary>
        /// Is User Name Exists
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool CheckFarmUserNameExist(string email, Guid farmId)
        {
            return _context.ApplicationUsers.Where(a => a.UserName == email + farmId).FirstOrDefault() == null ? false : true;
        }

        /// <summary>
        /// Is existing user Email in this farm
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool CheckUserExistsInTheSameFarm(string email, string masterAdminId)
        {
            return _context.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower() && u.MasterAdminId == masterAdminId) == null ? false : true;
        }
        /// <summary>
        /// Is Email Exist
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool InTrainingRecordExistance(string userId)
        {
            return _context.TrainingRecordOperatorMappings.FirstOrDefault(a=>a.OperatorId== userId) == null ? false : true;
        }

        /// <summary>
        /// get user list by search with pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public IEnumerable<ApplicationUser> GetUserListBySearch(int pageNumber, int pageSize, string searchKey, string masterAdminId)
        {
            var users = _context.ApplicationUsers.Where(a=>a.MasterAdminId== masterAdminId).Include(a => a.ApplicationUserType).Include(a=>a.UserGroup).AsQueryable();
            //if (searchKey != null)
            //{
            //    searchKey = searchKey.ToLower();
            //    users = users.Where(a => a.FirstName.ToLower().Contains(searchKey) || (a.LastName.ToLower().Contains(searchKey)));
            //}
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                var searchWords = searchKey.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                users = users.Where(a =>
                    searchWords.Any(word => a.FirstName.ToLower().Contains(word) || a.LastName.ToLower().Contains(word))
                );
            }

            return users.Where(a=>a.ApplicationUserTypeId != (int)Core.Common.Enums.ApplicationUserTypeEnum.SuperAdmin && a.ApplicationUserTypeId != (int)Core.Common.Enums.ApplicationUserTypeEnum.MasterAdmin).Include(a => a.ApplicationUserStatus).OrderBy(a => a.FirstName).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// get user list by search with pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public IEnumerable<ApplicationUser> GetUsers( string masterAdminId)
        {
            return _context.ApplicationUsers.Where(a => (a.MasterAdminId == masterAdminId) && (a.ApplicationUserStatusId == (int)Core.Common.Enums.ApplicationUserStatusEnum.Live) && (a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Both || a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Operator)).Include(a => a.ApplicationUserType).Include(a => a.UserGroup).Include(a => a.ApplicationUserStatus).ToList();
          
            
        }

        /// <summary>
        /// get user count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public int GetUserCountBySearch(string searchKey, string masterAdminId)
        {
            var users = _context.ApplicationUsers.Where(a => a.MasterAdminId == masterAdminId).AsQueryable();
            //if (searchKey != null)
            //{
            //    searchKey = searchKey.ToLower();
            //    users = users.Where(a => a.FirstName.ToLower().Contains(searchKey) || (a.LastName.ToLower().Contains(searchKey)));
            //}
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                var searchWords = searchKey.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                users = users.Where(a =>
                    searchWords.Any(word => a.FirstName.ToLower().Contains(word) || a.LastName.ToLower().Contains(word))
                );
            }
            return users.Where(a => a.ApplicationUserTypeId != (int)Core.Common.Enums.ApplicationUserTypeEnum.SuperAdmin && a.ApplicationUserTypeId != (int)Core.Common.Enums.ApplicationUserTypeEnum.MasterAdmin).Count();
        }

        /// <summary>
        /// get user details 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ApplicationUser GetUserDetails(string userId)
        {
            return _context.ApplicationUsers.Where(a => a.Id == userId).Include(a => a.ApplicationUserStatus).Include(a => a.ApplicationUserType).Include(a => a.OperatorStatus).Include(a => a.UserGroup).Include(a => a.Events).FirstOrDefault();
        }

        /// <summary>
        /// update user details 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ApplicationUser UpdateUserDetails(ApplicationUser model)
        {
            var response = _context.ApplicationUsers.FirstOrDefault(a => a.Id == model.Id);
            if (response != null)
            {
                response.FirstName = model.FirstName;
                response.LastName = model.LastName;
                response.ApplicationUserStatusId = model.ApplicationUserStatusId;
                response.ApplicationUserTypeId = model.ApplicationUserTypeId;
                response.Mobile = model.Mobile;
                response.HouseNameNumber = model.HouseNameNumber;
                response.PostCode = model.PostCode;
                response.Addressline2 = model.Addressline2;
                response.County = model.County;
                response.Street = model.Street;
                response.Town = model.Town;
                response.UserGroupId = model.UserGroupId; 
                _context.ApplicationUsers.Update(response);
                _context.SaveChanges();
                return _context.ApplicationUsers.Where(a => a.Id == model.Id).Include(a=>a.UserGroup).Include(a => a.ApplicationUserType).Include(a => a.ApplicationUserType).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// get user details 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<ApplicationUser> GetUserNameList(string UserMasterAdminId, string searchKey)
        {
            var Users = _context.ApplicationUsers.Where(a => a.MasterAdminId == UserMasterAdminId)
                            .Include(a => a.ApplicationUserType).OrderBy(a=>a.FirstName)
                            .AsQueryable();

            if (Users != null)
            {
                //if (searchKey != null)
                //{
                //    searchKey = searchKey.ToLower();
                //    Users = Users.Where(a => a.FirstName.ToLower().Contains(searchKey) || a.LastName.ToLower().Contains(searchKey) || a.Email.ToLower().Contains(searchKey));
                //}

                if (searchKey != null)
                {
                    searchKey = searchKey.ToLower();
                    var searchWords = searchKey.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    Users = Users.Where(a =>a.Email.ToLower().Contains(searchKey)||
                        searchWords.Any(word => a.FirstName.ToLower().Contains(word) || a.LastName.ToLower().Contains(word))
                    );
                }
                return Users.ToList();
            }
            return null;
        }
        
        /// <summary>
        /// get operator and both user details 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<ApplicationUser> GetOperatorBothUserDetails(string masterAdminId)
        {
            var response = _context.ApplicationUsers.Where(a => a.MasterAdminId == masterAdminId && a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Operator || a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Both)
                            .Include(a => a.ApplicationUserType).OrderBy(a=>a.FirstName)
                            .ToList(); 
            return response;
        }
        
        /// <summary>
        /// update user profile image 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ApplicationUser UpdateProfileImage(ApplicationUser applicationUser)
        {
            var resp = _context.ApplicationUsers.FirstOrDefault(a => a.Id == applicationUser.Id);
            resp.ProfileImageLink = applicationUser.ProfileImageLink;
            resp.ProfileImageName = applicationUser.ProfileImageName;
            var res = _context.ApplicationUsers.Update(resp);
            if (res == null)
            {
                return null;
            }
            _context.SaveChanges();
            var result = _context.ApplicationUsers.FirstOrDefault(a=>a.Id == applicationUser.Id);
            return result;
        }

        /// <summary>
        /// get user profile image 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ApplicationUser GetProfileImage(string userId)
        {
            return _context.ApplicationUsers.FirstOrDefault(a=>a.Id == userId);
        }
        
        /// <summary>
        /// update user location 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateUserLocation(string userId, string location)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(a => a.Id == userId);
            user.Location = location;
            _context.ApplicationUsers.Update(user);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Get User ClientId into the system 
        /// </summary>
        /// <param name=userId"></param>
        /// <returns></returns>
        /// <summary>
        public string GetMasterAdminId(string userId)
        {
            return _context.ApplicationUsers.Find(userId)?.MasterAdminId;
        }




        /// <summary>
        /// update user detail app 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ApplicationUser UpdateUserDetailApp(ApplicationUser model)
        {
            var response = _context.ApplicationUsers.FirstOrDefault(a => a.Id == model.Id);
            if (response != null)
            {
                response.FirstName = model.FirstName;
                response.LastName = model.LastName;
                //response.ApplicationUserStatusId = model.ApplicationUserStatusId;
                response.Mobile = model.Mobile;
                response.HouseNameNumber = model.HouseNameNumber;
                response.PostCode = model.PostCode;
                response.County = model.County;
                response.Street = model.Street;
                response.Town = model.Town;
                _context.ApplicationUsers.Update(response);
                _context.SaveChanges();
                return _context.ApplicationUsers.Where(a => a.Id == model.Id).Include(a => a.ApplicationUserStatus).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// Get User ClientId into the system 
        /// </summary>
        /// <param name=userId"></param>
        /// <returns></returns>
        /// <summary>
        public List<string> GetSystemMasterAdminIds()
        {
            return _context.ApplicationUsers.Where(a => a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.MasterAdmin).Select(a => a.Id).ToList();
        }


        /// <summary>
        /// Get AlarmAction into the system 
        /// </summary>
        /// <param name=userId"></param>
        /// <returns></returns>
        /// <summary>
        public AlarmAction GetAlarmActionDetail(string userId)
        {
            var applicationUser = _context.Users.FirstOrDefault(u => u.Id == userId);
            if(applicationUser != null)
            {
                return _context.AlarmActions.Where(a => a.ApplicationUser.MasterAdminId == applicationUser.MainAdminId).FirstOrDefault();
            }
            return null;
            
        }


        /// <summary>
        /// get user details 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ApplicationUser GetUserDetailByEmail(string email)
        {
            return _context.ApplicationUsers.Where(a => a.Email.ToLower() == email.ToLower()).Include(a => a.ApplicationUserStatus).Include(a => a.ApplicationUserType).Include(a => a.OperatorStatus).Include(a => a.UserGroup).Include(a => a.Events).FirstOrDefault();
        }
        
        /// <summary>
        /// get existing user details by email in another email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ApplicationUser GetExistingUserDetailByEmail(string email, string UserMasterAdminId)
        {
            return _context.ApplicationUsers.Where(a => a.Email == email && a.MasterAdminId != UserMasterAdminId).Include(a => a.ApplicationUserStatus).Include(a => a.ApplicationUserType).Include(a => a.OperatorStatus).Include(a => a.UserGroup).Include(a => a.Events).FirstOrDefault();
        }
        /// <summary>
        /// get users list 
        /// </summary>
        /// <param name="UserMasterAdminId"></param>
        /// <returns></returns>
        public List<ApplicationUser> GetUsersList(string UserMasterAdminId)
        {
            return _context.ApplicationUsers.Where(a => a.MasterAdminId == UserMasterAdminId).ToList();
        }
        
        /// <summary>
        /// get users list by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public List<ApplicationUser> GetUsersListByEmail(string email)
        {
            return _context.ApplicationUsers.Where(a => a.Email == email).ToList();
        }

        /// <summary>
        /// get users list 
        /// </summary>
        /// <param name="UserMasterAdminId"></param>
        /// <returns></returns>
        public bool IsUserAddedTrainingRecord(Guid trainingRecordId, string userId)
        {
           return _context.TrainingRecordOperatorMappings.Any(a => a.TrainingRecordId == trainingRecordId && a.OperatorId == userId);
          
        }

        /// <summary>
        /// get users list 
        /// </summary>
        /// <param name="UserMasterAdminId"></param>
        /// <returns></returns>
        public string GetFarmNameByMasterAdminId(string masterAdminId)
        {
            return _context.Farms.Where(a => a.MasterAdminId == masterAdminId).FirstOrDefault().FarmName;

        }
        /// <summary>
        /// get users list 
        /// </summary>
        /// <param name="UserMasterAdminId"></param>
        /// <returns></returns>
        public Farm GetFarmByMasterAdminId(string masterAdminId)
        {
           return _context.Farms.Where(a => a.MasterAdminId== masterAdminId).FirstOrDefault();
          
        }
    }
}
