using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
    public class DashboardRepository : IDashboardRepository
    {
        private readonly SmartFarmerContext _context;
        public DashboardRepository(SmartFarmerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// get issue defect count 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetIssueDefectCount(string CreatedBy)
        {
            var count = _context.Issues.Where(a =>(a.Operator.MasterAdminId==CreatedBy)&& (a.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Defect) && (a.IssueStatusId == (int)Core.Common.Enums.IssueStatusEnum.Open)).Count();
            return count;
        }

        /// <summary>
        /// get issue corrective count 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetIssueCorrectiveCount(string CreatedBy)
        {
            var count= _context.Issues.Where(a => a.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Corrective && a.Operator.MasterAdminId == CreatedBy && a.IssueStatusId == (int)Core.Common.Enums.IssueStatusEnum.Open).Count();
            return count;
        }

        /// <summary>
        /// get issue warning count 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetIssueWarningCount(string CreatedBy)
        {
            return _context.Issues.Where(a => a.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Warning && a.Operator.MasterAdminId == CreatedBy && a.IssueStatusId == (int)Core.Common.Enums.IssueStatusEnum.Open).Count();
        }

        /// <summary>
        /// get training expired count 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetTrainingExpiredCount(string CreatedBy)
        {

            var trainigs = _context.Trainings.Where(a => a.Expires == true  && a.ApplicationUser.MasterAdminId == CreatedBy).ToList();
            var count = 0;
            foreach (var training in trainigs)
            {
                if (training.Expires == true && training.Validity != null)
                {
                    var today = DateTime.Today;
                    if (training.DueDate.Value <= today)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        
        /// <summary>
        /// get training expired count 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetTrainingDueCount(string CreatedBy)
        {
            // return _context.Trainings.Where(a => a.Expires == false && a.ApplicationUser.MasterAdminId == CreatedBy).Count();
            var trainigs = _context.Trainings.Where(a => a.Expires == true && a.ApplicationUser.MasterAdminId == CreatedBy).ToList();
            var count = 0;
            foreach (var training in trainigs)
            {
                if (training.Expires == true && training.Validity != null)
                {
                    var today = DateTime.Today;
                    if (training.DueDate.Value > today)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// get operator active count 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetOperatorActiveCount(string CreatedBy)
        {
            return _context.ApplicationUsers.Where(a => a.MasterAdminId == CreatedBy && ((a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Operator || a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Both) && (a.ApplicationUserStatusId == (int)Core.Common.Enums.ApplicationUserStatusEnum.Live) && (a.OperatorStatusId == (int)Core.Common.Enums.OperatorStatusEnum.Working))).Count();
        }

        /// <summary>
        /// get operator total count 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetOperatorTotalCount(string CreatedBy)
        {
            return _context.ApplicationUsers.Where(a =>a.MasterAdminId == CreatedBy&&(( a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Operator || a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Both) && (a.ApplicationUserStatusId== (int)Core.Common.Enums.ApplicationUserStatusEnum.Live))).Count();
        }

        /// <summary>
        /// get machine total count 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetMachineTotalCount(string CreatedBy)
        {
            return _context.Machines.Where(a => a.ApplicationUser.MasterAdminId == CreatedBy).Count();
        }

        /// <summary>
        /// get machine active count 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetMachineActiveCount(string CreatedBy)
        {
            return _context.Machines.Where(a => a.MachineStatusId == (int)Core.Common.Enums.MachineStatusEnum.Active && a.ApplicationUser.MasterAdminId == CreatedBy).Count();
        }
        
        /// <summary>
        /// get machine idle count 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetMachineIdleCount(string CreatedBy)
        {
            return _context.Machines.Where(a => a.MachineStatusId == (int)Core.Common.Enums.MachineStatusEnum.Idle && a.ApplicationUser.MasterAdminId == CreatedBy).Count();
        }
        
        /// <summary>
        /// get machine idle count 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetMachineOutOfServiceCount(string CreatedBy)
        {
            return _context.Machines.Where(a => a.MachineStatusId == (int)Core.Common.Enums.MachineStatusEnum.OutOfService && a.ApplicationUser.MasterAdminId == CreatedBy).Count();
        } 
        /// <summary>
        /// get machine idle count 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetMachineOutOfSeasonCount(string CreatedBy)
        {
            return _context.Machines.Where(a => a.InSeason == false && a.ApplicationUser.MasterAdminId == CreatedBy).Count();
        }

        /// <summary>
        /// get machine list by search
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<Machine> GetMachineListBySearch(string MasterAdminId, string searchKey)
        {
            var machine = _context.Machines.Where(a => a.ApplicationUser.MasterAdminId == MasterAdminId).Include(a => a.ApplicationUser).Include(a => a.MachineType).Include(a => a.Operator).Include(a => a.MachineCategory).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                machine = machine.Where(a => a.Name.ToLower().Contains(searchKey) ||  a.NickName.ToLower().Contains(searchKey));
            }
            return machine.Include(a => a.MachineStatus).OrderByDescending(a => a.CreatedDate).ToList();
        }

        /// <summary>
        /// get machine count by search 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public int GetMachineCountBySearch(string MasterAdminId, string searchKey)
        {
            var machine = _context.Machines.Where(a => a.ApplicationUser.MasterAdminId == MasterAdminId).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                machine = machine.Where(a => a.Name.ToLower().Contains(searchKey) || a.NickName.ToLower().Contains(searchKey));
            }
            return machine.Count();
        }

        /// <summary>
        /// get operator list by search
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<ApplicationUser> GetOperatorListBySearch(string masterAdminId, string searchKey)
        {
            //var machine = _context.ApplicationUsers.Where(a => a.MasterAdminId == MasterAdminId && a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Operator).Include(a => a.ApplicationUserStatus).AsQueryable();
            //if (searchKey != null)
            //{
            //    searchKey = searchKey.ToLower();
            //    machine = machine.Where(a => a.FirstName.ToLower().Contains(searchKey) || a.LastName.ToLower().Contains(searchKey));
            //}
            //return machine.OrderByDescending(a => a.CreatedDate).ToList();

            var applicationUsers = _context.ApplicationUsers.Where(a => a.MasterAdminId == masterAdminId && a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Operator).Include(a => a.ApplicationUserStatus).AsQueryable();
            //var mappings = _context.FarmOperatorMappings.Where(a => a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Operator && a.FarmId == farmId).Include(a => a.Operator.ApplicationUserStatus).Include(a=>a.Operator).AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                searchKey = searchKey.ToLower();
                applicationUsers = applicationUsers.Where(a => a.FirstName.ToLower().Contains(searchKey) || (a.LastName.ToLower().Contains(searchKey)));

                //var searchWords = searchKey.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                //mappings = mappings.Where(a =>
                //    searchWords.Any(word => a.Operator.FirstName.ToLower().Contains(word) || a.Operator.LastName.ToLower().Contains(word))
                //);
            }
            return applicationUsers.OrderByDescending(a => a.CreatedDate).ToList();
        }

        /// <summary>
        /// get operator count by search 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public int GetOperatorCountBySearch(string masterAdminId, string searchKey)
        {
            var applicationUsers = _context.ApplicationUsers.Where(a => a.ApplicationUserTypeId == (int)Core.Common.Enums.ApplicationUserTypeEnum.Operator && a.MasterAdminId == masterAdminId).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                applicationUsers = applicationUsers.Where(a => a.FirstName.ToLower().Contains(searchKey) || (a.LastName.ToLower().Contains(searchKey)));
            }
            //if (!string.IsNullOrWhiteSpace(searchKey))
            //{
            //    searchKey = searchKey.ToLower();
            //    var searchWords = searchKey.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            //    mappings = mappings.Where(a =>
            //        searchWords.Any(word => a.Operator.FirstName.ToLower().Contains(word) || a.Operator.LastName.ToLower().Contains(word))
            //    );
            //}
            return applicationUsers.Count();
        }
    }
}
