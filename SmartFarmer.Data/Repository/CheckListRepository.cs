using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartFarmer.Data.Context;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;

namespace SmartFarmer.Data.Repository
{
    public class CheckListRepository : ICheckListRepository
    {
        private SmartFarmerContext _dbContext;
        public CheckListRepository(SmartFarmerContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Add CheckList
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CheckList AddCheckList(CheckList model)
        {
            _dbContext.CheckLists.Add(model);
            _dbContext.SaveChanges();
            return model;
        }

        /// <summary>
        /// Add CheckList
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CheckListMachineMapping StartCheckList(CheckListMachineMapping model)
        {
            var test = _dbContext.CheckListMachineMappings.Add(model);
            foreach (var mapping in model.CheckListItemAnswerMappings)
            {
                var test2 =_dbContext.ChecListItemAnswerMappings.Add(mapping);
            }
            _dbContext.SaveChanges();
            var machineMapping = _dbContext.CheckListMachineMappings
                .Include(c => c.CheckList).Include(a=>a.CheckResult)
                .Include(c => c.CheckListItemAnswerMappings)
                .FirstOrDefault(c => c.CheckListMachineMappingId == model.CheckListMachineMappingId);
            return machineMapping;
        }

        /// <summary>
        /// Get Last CheckList
        /// </summary>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CheckListMachineMapping GetLastCheckList(Guid checkListId, Guid machineId)
        {
            return _dbContext.CheckListMachineMappings.Where(a => a.CheckListId == checkListId && a.MachineId == machineId).Include(a => a.CheckListItemAnswerMappings).Include(a => a.Machine).Include(a => a.CheckList).Include(a => a.Operator).OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        }

        /// <summary>
        /// Get Last CheckList
        /// </summary>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CheckListMachineMapping GetMachineLastCheckList(Guid machineId)
        {
            return _dbContext.CheckListMachineMappings.Where(a => a.MachineId == machineId).Include(a => a.CheckListItemAnswerMappings).Include(a => a.Machine).Include(a => a.CheckList).Include(a => a.Operator).OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        }

        /// <summary>
        /// get CheckList details 
        /// </summary>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        public CheckList GetCheckListDetails(Guid checkListId)
        {
            return _dbContext.CheckLists.Where(a => a.CheckListId == checkListId).Include(a => a.CheckType).Include(a => a.FrequencyType).Include(a => a.MachineType).Include(a => a.ApplicationUser).FirstOrDefault();
        }

        /// <summary>
        /// get CheckList list by search pagination 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<CheckList> GetCheckListListBySearch(int pageNumber, int pageSize, string searchKey, Guid? MachineTypeId, int? CheckTypeId, int? FrequencyTypeId, string UserMasterAdminId)
        {
            var checkList = _dbContext.CheckLists.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).Include(a => a.ApplicationUser).Include(a => a.CheckType).Include(a => a.FrequencyType).Include(a => a.MachineType).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                checkList = checkList.Where(a => a.Name.ToLower().Contains(searchKey));
            }
            if (MachineTypeId != null)
            {
                checkList = checkList.Where(a => a.MachineTypeId == MachineTypeId);
            }
            if (CheckTypeId != null)
            {
                checkList = checkList.Where(a => a.CheckTypeId == CheckTypeId);
            }
            if (FrequencyTypeId != null)
            {
                checkList = checkList.Where(a => a.FrequencyTypeId == FrequencyTypeId);
            }
            return checkList.Skip((pageNumber - 1) * pageSize).Take(pageSize).OrderByDescending(a => a.CreatedDate).ToList();
        }

        /// <summary>
        /// get CheckList list 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<CheckList> GetCheckListList(string UserMasterAdminId)
        {
            var checkList = _dbContext.CheckLists.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).AsQueryable();
            return checkList.ToList();
        }

        /// <summary>
        /// get CheckList count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetCheckListCountBySearch(string searchKey, Guid? MachineTypeId, int? CheckTypeId, int? FrequencyTypeId, string UserMasterAdminId)
        {
            var checkList = _dbContext.CheckLists.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                checkList = checkList.Where(a => a.Name.ToLower().Contains(searchKey));
            }
            if (MachineTypeId != null)
            {
                checkList = checkList.Where(a => a.MachineTypeId == MachineTypeId);
            }
            if (CheckTypeId != null)
            {
                checkList = checkList.Where(a => a.CheckTypeId == CheckTypeId);
            }
            if (FrequencyTypeId != null)
            {
                checkList = checkList.Where(a => a.FrequencyTypeId == FrequencyTypeId);
            }
            return checkList.Count();
        }

        /// <summary>
        /// check the CheckList existence
        /// </summary>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsCheckListExist(Guid checkListId)
        {
            return _dbContext.CheckLists.Find(checkListId) == null ? false : true;
        } 
        
        /// <summary>
        /// check the CheckListItem existence
        /// </summary>
        /// <param name="checkListItemId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsCheckListItemExist(Guid checkListItemId)
        {
            return _dbContext.CheckListItems.Find(checkListItemId) == null ? false : true;
        }

        /// <summary>
        /// Update CheckList Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CheckList UpdateCheckListDetails(CheckList model)
        {
            var checkList = _dbContext.CheckLists.FirstOrDefault(a => a.CheckListId == model.CheckListId);
            if (checkList == null)
            {
                return null;
            }
            checkList.Name = model.Name;
            checkList.Frequency = model.Frequency;
            checkList.FrequencyTypeId = model.FrequencyTypeId;
            checkList.CheckTypeId = model.CheckTypeId;
            checkList.MachineTypeId = model.MachineTypeId;
            _dbContext.CheckLists.Update(checkList);
            _dbContext.SaveChanges();
            var result = _dbContext.CheckLists.Where(a => a.CheckListId == model.CheckListId).Include(a => a.ApplicationUser).FirstOrDefault();
            return checkList;
        }

        /// Update Operator CheckList Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CheckList UpdateCheckListDetailByOperatorId(CheckList model)
        {
            var checkList = _dbContext.CheckLists.FirstOrDefault(a => a.CheckListId == model.CheckListId);
            if (checkList == null)
            {
                return null;
            }
            checkList.LastCheckDate = model.LastCheckDate;
            checkList.NextDueDate = model.NextDueDate;
            checkList.OperatorId = model.OperatorId;
            _dbContext.CheckLists.Update(checkList);
            _dbContext.SaveChanges();
            var result = _dbContext.CheckLists.Where(a => a.CheckListId == model.CheckListId).Include(a => a.ApplicationUser).FirstOrDefault();
            return checkList;
        }

        /// <summary>
        /// delete  CheckList 
        /// </summary>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        public bool DeleteCheckList(Guid checkListId)
        {
            var checkList = _dbContext.CheckLists.FirstOrDefault(m => m.CheckListId == checkListId);
            if (checkList == null)
            {
                return false;
            }
            _dbContext.CheckLists.Remove(checkList);
            _dbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Add checklistitem 
        /// </summary>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        public CheckListItem AddCheckListItems(CheckListItem checkListItem)
        {
            var resp = _dbContext.CheckListItems.Add(checkListItem);
            if (resp == null)
            {
                return null;
            }
            _dbContext.SaveChanges();
            var checklistitem = _dbContext.CheckListItems.FirstOrDefault(a => a.CheckListItemId == checkListItem.CheckListItemId);
            return checklistitem;
        }

        /// <summary>
        /// update checklistitem 
        /// </summary>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        public CheckListItem UpdateCheckListItems(CheckListItem checkListItem)
        {
            var resp = _dbContext.CheckListItems.Update(checkListItem);
            if (resp == null)
            {
                return null;
            }
            _dbContext.SaveChanges();
            var checklistitem = _dbContext.CheckListItems.FirstOrDefault(a => a.CheckListItemId == checkListItem.CheckListItemId);
            return checklistitem;
        }

        /// <summary>
        /// Get checklistitems
        /// </summary>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        public IEnumerable<CheckListItem> GetCheckListItems(Guid checkListId)
        {
            return _dbContext.CheckListItems.Where(a => a.CheckListId == checkListId).OrderBy(a => a.Priority);
        }

        /// <summary>
        /// Get checklistitems
        /// </summary>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        public bool IsCheckListItemExists(Guid checkListId, Guid checkListItemId)
        {
            return _dbContext.CheckListItems.Where(a => a.CheckListItemId == checkListItemId && a.CheckListId == checkListId) == null ? false : true;
        }

        /// <summary>
        /// Get All the Checklists against MachineTypeId
        /// </summary>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        public IEnumerable<CheckList> GetCheckListOfMachineType(Guid MachineTypeId)
        {
            return _dbContext.CheckLists.Where(a => a.MachineTypeId == MachineTypeId).Include(a => a.MachineType).Include(a => a.CheckType).Include(a => a.FrequencyType).ToList();
        }
        
        /// <summary>
        /// Get Pre Checks against MachineTypeId
        /// </summary>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        public IEnumerable<CheckList> GetPreCheckListOfMachineType(Guid MachineTypeId, string searchKey, int? FrequencyTypeId)
        {
            var checklists = _dbContext.CheckLists.Where(a => a.MachineTypeId == MachineTypeId && a.CheckTypeId == (int)Core.Common.Enums.CheckTypeEnum.PreCheck).Include(a => a.MachineType).Include(a => a.CheckType).Include(a => a.FrequencyType).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                checklists = checklists.Where(a => a.Name.ToLower().Contains(searchKey));
            }

            if (FrequencyTypeId != null)
            {
                checklists = checklists.Where(a => a.FrequencyTypeId == FrequencyTypeId);
            }
            return checklists.ToList();
        }
        /// <summary>
        /// Get Pre Checks against MachineTypeId
        /// </summary>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        public IEnumerable<CheckList> GetPreCheckListOfMachineTypeforDashboard(Guid MachineTypeId)
        {
            var checklists = _dbContext.CheckLists.Where(a => a.MachineTypeId == MachineTypeId && a.CheckTypeId == (int)Core.Common.Enums.CheckTypeEnum.PreCheck).Include(a => a.MachineType).Include(a => a.CheckType).Include(a => a.FrequencyType).AsQueryable();
           
            return checklists.ToList();
        }

        /// <summary>
        /// Get CheckList Date of Machine
        /// </summary>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        public CheckListMachineMapping GetMachineCheckListDate(Guid MachineId, Guid CheckListId)
        {
            var checklist = _dbContext.CheckListMachineMappings
                .Where(a => a.MachineId == MachineId && a.CheckListId == CheckListId).Include(a=>a.CheckResult)
                .OrderByDescending(a => a.CreatedDate)
                .FirstOrDefault();

            return checklist; // Use null-conditional operator to avoid NullReferenceException
        }

        /// <summary>
        /// Get Pre-check logs list by search
        /// </summary>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        public IEnumerable<CheckListMachineMapping> GetPreCheckLogsBySearchWithPagination(int pageNumber, int pageSize, string searchKey, Guid? MachineId, int? FrequencyTypeId, string UserMasterAdminId)
        {
            var checkListMachineMapping = _dbContext.CheckListMachineMappings.Where(a => a.Operator.MasterAdminId == UserMasterAdminId && a.CheckList.CheckTypeId == (int)Core.Common.Enums.CheckTypeEnum.PreCheck).Include(a => a.Operator).Include(a => a.CheckList.FrequencyType).Include(a => a.Machine).Include(a => a.CheckList.CheckType).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                checkListMachineMapping = checkListMachineMapping.Where(a => a.CheckList.Name.ToLower().Contains(searchKey) || a.Machine.Name.ToLower().Contains(searchKey));
            }
            if (MachineId != null)
            {
                checkListMachineMapping = checkListMachineMapping.Where(a => a.MachineId == MachineId);
            }
            if (FrequencyTypeId != null)
            {
                checkListMachineMapping = checkListMachineMapping.Where(a => a.CheckList.FrequencyTypeId == FrequencyTypeId);
            }
            return checkListMachineMapping.Skip((pageNumber - 1) * pageSize).Take(pageSize).OrderByDescending(a => a.CreatedDate).ToList();
        }

        /// <summary>
        /// get CheckList count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetPreCheckLogsCountBySearch(string searchKey, Guid? MachineId, int? FrequencyTypeId, string UserMasterAdminId)
        {
            var checkListMachineMapping = _dbContext.CheckListMachineMappings.Where(a => a.Operator.MasterAdminId == UserMasterAdminId && a.CheckList.CheckTypeId == (int)Core.Common.Enums.CheckTypeEnum.PreCheck).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                checkListMachineMapping = checkListMachineMapping.Where(a => a.CheckList.Name.ToLower().Contains(searchKey) || a.Machine.Name.ToLower().Contains(searchKey));
            }
            if (MachineId != null)
            {
                checkListMachineMapping = checkListMachineMapping.Where(a => a.MachineId == MachineId);
            }
            if (FrequencyTypeId != null)
            {
                checkListMachineMapping = checkListMachineMapping.Where(a => a.CheckList.FrequencyTypeId == FrequencyTypeId);
            }
            return checkListMachineMapping.Count();
        }


        /// <summary>
        /// get checkList list by machineId
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<CheckList> GetCheckListList(Guid machineId, string userMasterAdminId)
        {
            var checkLists = _dbContext.CheckListMachineMappings.Where(m => m.MachineId == machineId && m.CheckList.ApplicationUser.MasterAdminId == userMasterAdminId).Select(m => m.CheckList).Distinct().Include(a => a.ApplicationUser).Include(a => a.CheckType).ToList();
            return checkLists;
        }



        /// <summary>
        /// delete  CheckListItem 
        /// </summary>
        /// <param name="checkListItemId"></param>
        /// <returns></returns>
        public bool DeleteCheckListItem(Guid checkListItemId)
        {
            var checkListItem = _dbContext.CheckListItems.Find(checkListItemId);
            if (checkListItem != null)
            {
                var checkListItemAnswerMapping = _dbContext.ChecListItemAnswerMappings.FirstOrDefault(m => m.CheckListItemId == checkListItemId);
                if (checkListItemAnswerMapping != null)
                {
                    return false;
                }
                _dbContext.CheckListItems.Remove(checkListItem);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

    }
}
