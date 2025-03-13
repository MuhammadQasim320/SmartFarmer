using Microsoft.EntityFrameworkCore;
using SmartFarmer.Data.Context;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Data.Repository
{
    public class MachineCategoryRepository : IMachineCategoryRepository
    {
        private readonly SmartFarmerContext _context;
        public MachineCategoryRepository(SmartFarmerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// add machineCategory into system
        /// </summary>
        /// <param name="machine"></param>
        /// <returns></returns>
        public MachineCategory AddMachineCategory(MachineCategory machineCategory)
        {
            _context.MachineCategorys.Add(machineCategory);
            _context.SaveChanges();
            return _context.MachineCategorys.Where(a => a.MachineCategoryId == machineCategory.MachineCategoryId).Include(a => a.ApplicationUser).FirstOrDefault();
        }

        /// <summary>
        /// get machineCategory list by search with pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public IEnumerable<MachineCategory> GetMachineCategoryListBySearch(string masterAdminId,int pageNumber, int pageSize, string searchKey)
        {
            var machineCategory = _context.MachineCategorys.Where(a=>a.ApplicationUser.MasterAdminId== masterAdminId).Include(a => a.ApplicationUser).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                machineCategory = machineCategory.Where(a => a.Name.ToLower().Contains(searchKey));
            }
            return machineCategory.Skip((pageNumber - 1) * pageSize).Take(pageSize).OrderBy(a => a.Name).ToList();
        }

        /// <summary>
        /// get machineCategory count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public int GetMachineCategoryCountBySearch(string masterAdminId,string searchKey)
        {
            var machineCategory = _context.MachineCategorys.Where(a => a.ApplicationUser.MasterAdminId == masterAdminId).Include(a => a.ApplicationUser).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                machineCategory = machineCategory.Where(a => a.Name.ToLower().Contains(searchKey));
            }
            return machineCategory.Count();
        }

        /// <summary>
        /// check machineCategory existence
        /// </summary>
        /// <param name="machineCategoryId"></param>
        /// <returns></returns>
        public bool IsMachineCategoryExist(Guid machineCategoryId)
        {
            return _context.MachineCategorys.Find(machineCategoryId) == null ? false : true;
        }  
        /// <summary>
        /// check machineCategory existence
        /// </summary>
        /// <param name="machineCategoryId"></param>
        /// <returns></returns>
        public bool IsMachineCategoryAssign(Guid machineCategoryId)
        {
            return _context.Machines.Where(a=>a.MachineCategoryId== machineCategoryId).FirstOrDefault() == null ? true : false;
        }

        /// <summary>
        /// get machineCategory details
        /// </summary>
        /// <param name="machineCategoryId"></param>
        /// <returns></returns>
        public MachineCategory GetMachineCategoryDetails(Guid machineCategoryId)
        {
            return _context.MachineCategorys.Where(a => a.MachineCategoryId == machineCategoryId).Include(a => a.ApplicationUser).FirstOrDefault();
        }

        /// <summary>
        ///update machineCategory details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MachineCategory UpdateMachineCategoryDetails(Guid machineCategoryId, MachineCategory machineCategory)
        {
            var res = _context.MachineCategorys.Find(machineCategoryId);
            if (res != null)
            {
                res.Name = machineCategory.Name;
                res.Description = machineCategory.Description;
                _context.MachineCategorys.Update(res);
                _context.SaveChanges();
                return _context.MachineCategorys.Where(a => a.MachineCategoryId == machineCategoryId).Include(a => a.ApplicationUser).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// delete  MachineCategory 
        /// </summary>
        /// <param name="machineCategoryId"></param>
        /// <returns></returns>
        public bool DeleteMachineCategory(Guid machineCategoryId)
        {
            var machineCategory = _context.MachineCategorys.FirstOrDefault(m => m.MachineCategoryId == machineCategoryId);
            if (machineCategory == null)
            {
                return false;
            }
            _context.MachineCategorys.Remove(machineCategory);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Get  MachineCategory Name List
        /// </summary>
        /// <param name="masterAdminId"></param>
        /// <returns></returns>
        public List<MachineCategory> GetMachineCategoryNameList(string masterAdminId)
        {
            return _context.MachineCategorys.Where(a=>a.ApplicationUser.MasterAdminId == masterAdminId).OrderBy(a=>a.Name).ToList();
        }
    }
}
