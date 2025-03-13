using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using SmartFarmer.Data.Context;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Data.Repository
{
    public class EquipmentRepository : IEquipmentRepository
    {

        private readonly SmartFarmerContext _context;
        public EquipmentRepository(SmartFarmerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// get Equipment list by search with pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public IEnumerable<Domain.Model.Machine> GetEquipmentListBySearch(int pageNumber, int pageSize, string searchKey, int? equipmentStatusId, bool? hasIssues, bool? isOUtOfSeason, string loginUserMasterAdminId)
        {
            var machines = _context.Machines.Include(a=>a.Operator).Include(a => a.Issues).Include(a=>a.MachineStatus).Include(a => a.Events).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                machines = machines.Where(a => a.Name.ToLower().Contains(searchKey) || (a.NickName.ToLower().Contains(searchKey)));
            }
            if (equipmentStatusId != null)
            {
                machines = machines.Where(a => a.MachineStatusId == equipmentStatusId);
            }
            if (isOUtOfSeason != null)
            {
                machines = machines.Where(a => a.InSeason == false);
            }
            if (hasIssues != null)
            {
                if (hasIssues.Value)
                {
                    machines = machines.Where(a => a.Issues.Any());
                }
                else
                {
                    machines = machines.Where(a => !a.Issues.Any());
                }
            }
            return machines.Where(a => a.ApplicationUser.MasterAdminId == loginUserMasterAdminId).OrderByDescending(a => a.CreatedDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// get Equipment count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public int GetEquipmentCountBySearch(string searchKey, int? equipmentStatusId, bool? hasIssues, bool? isOUtOfSeason, string loginUserMasterAdminId)
        {
            var machines = _context.Machines.Include(a => a.Issues).Include(a => a.Events).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                machines = machines.Where(a => a.Name.ToLower().Contains(searchKey) || (a.NickName.ToLower().Contains(searchKey)));
            }
            if (equipmentStatusId != null)
            {
                machines = machines.Where(a => a.MachineStatusId == equipmentStatusId);
            }
            if (isOUtOfSeason != null)
            {
                machines = machines.Where(a => a.InSeason == false);
            }
            if (hasIssues != null)
            {
                if (hasIssues.Value)
                {
                    machines = machines.Where(a => a.Issues.Any());
                }
                else
                {
                    machines = machines.Where(a => !a.Issues.Any());
                }
            }
            return machines.Where(a => a.ApplicationUser.MasterAdminId == loginUserMasterAdminId).Count();
        }

        /// <summary>
        /// get equipment history by machineId
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<Event> GetEquipmentHistory(Guid machineId, int pageNumber, int pageSize, int? EventTypeId, string UserMasterAdminId)
        {
            var equipmentHistory = _context.Events.Where(m => m.MachineId == machineId && m.ApplicationUser.MasterAdminId == UserMasterAdminId).AsQueryable();      

            if (EventTypeId != null)
            {
                equipmentHistory = equipmentHistory.Where(a => a.EventTypeId == EventTypeId);
            }
            return equipmentHistory.Include(a => a.ApplicationUser).Include(a => a.Machine).Include(a=>a.CheckListMachineMapping).Include(a=>a.CheckListMachineMapping.CheckResult).Include(a => a.EventType).OrderByDescending(a => a.CreatedDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// get Equipment history count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public int GetEquipmentHistoryCountBySearch(Guid machineId, int? EventTypeId, string UserMasterAdminId)
        {
            var equipmentHistory = _context.Events.Where(m => m.MachineId == machineId && m.ApplicationUser.MasterAdminId == UserMasterAdminId).Include(a => a.ApplicationUser).Include(a => a.Machine).Include(a => a.EventType).AsQueryable();
            if (EventTypeId != null)
            {
                equipmentHistory = equipmentHistory.Where(a => a.EventTypeId == EventTypeId);
            }
      
            return equipmentHistory.Count();
        }




        /// <summary>
        /// get equipment precheck history by machineId
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<Event> GetEquipmentPreCheckHistory(Guid machineId, string searchKey, Guid? MachineId, int? resultId, string UserMasterAdminId)
        {
            var equipmentHistory = _context.Events.Where(m => m.MachineId == machineId && m.ApplicationUser.MasterAdminId == UserMasterAdminId && m.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Pre_Check).AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                var searchWords = searchKey.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                equipmentHistory = equipmentHistory.Where(a => a.Machine.Name.ToLower().Contains(searchKey) ||
                    searchWords.Any(word => a.ApplicationUser.FirstName.ToLower().Contains(word) || a.ApplicationUser.LastName.ToLower().Contains(word))
                );
            }

            if (MachineId != null)
            {
                equipmentHistory = equipmentHistory.Where(a => a.MachineId == MachineId);
            }
            if (resultId != null)
            {
                equipmentHistory = equipmentHistory.Where(a => a.CheckListMachineMapping.ResultId == resultId);
            }
            return equipmentHistory.Include(a=>a.CheckListMachineMapping).Include(a => a.CheckListMachineMapping.CheckResult).Include(a => a.ApplicationUser).Include(a => a.Machine).ThenInclude(a => a.CheckResult).Include(a => a.EventType).ToList();
        }

        /// <summary>
        /// get Equipment precheck history count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public int GetEquipmentPreCheckHistoryCountBySearch(Guid machineId, string searchKey, Guid? MachineId, int? resultId, string UserMasterAdminId)
        {
            var equipmentHistory = _context.Events.Where(m => m.MachineId == machineId && m.ApplicationUser.MasterAdminId == UserMasterAdminId && m.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Pre_Check).AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                var searchWords = searchKey.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                equipmentHistory = equipmentHistory.Where(a => a.Machine.Name.ToLower().Contains(searchKey) ||
                    searchWords.Any(word => a.ApplicationUser.FirstName.ToLower().Contains(word) || a.ApplicationUser.LastName.ToLower().Contains(word))
                );
            }
            if (MachineId != null)
            {
                equipmentHistory = equipmentHistory.Where(a => a.MachineId == MachineId);
            }
            if (resultId != null)
            {
                equipmentHistory = equipmentHistory.Where(a => a.CheckListMachineMapping.ResultId == resultId);
            }

            return equipmentHistory.Count();
        }


    }
}
