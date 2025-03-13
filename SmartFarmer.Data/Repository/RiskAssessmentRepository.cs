using Microsoft.EntityFrameworkCore;
using SmartFarmer.Data.Context;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;
using System.Reflection.PortableExecutable;

namespace SmartFarmer.Data.Repository
{
    public class RiskAssessmentRepository : IRiskAssessmentRepository
    {
        private SmartFarmerContext _dbContext;
        public RiskAssessmentRepository(SmartFarmerContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Add RiskAssessment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public RiskAssessment AddRiskAssessment(RiskAssessment model)
        {
            _dbContext.RiskAssessments.Add(model);
            _dbContext.SaveChanges();
            var response = _dbContext.RiskAssessments.Where(a => a.RiskAssessmentId == model.RiskAssessmentId).Include(a => a.ApplicationUser).FirstOrDefault();
            return response;
        }

        /// <summary>
        /// Add RiskAssessmentLog
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public RiskAssessmentLog AddRiskAssessmentLog(RiskAssessmentLog model)
        {
            _dbContext.RiskAssessmentLogs.Add(model);
            _dbContext.SaveChanges();
            var response = _dbContext.RiskAssessmentLogs.Where(a => a.RiskAssessmentLogId == model.RiskAssessmentLogId).Include(a => a.ApplicationUser).Include(a=>a.RiskAssessment).FirstOrDefault();
            return response;
        }

        /// <summary>
        /// get RiskAssessment details 
        /// </summary>
        /// <param name="riskAssessmentId"></param>
        /// <returns></returns>
        public RiskAssessment GetRiskAssessmentDetails(Guid riskAssessmentId)
        {
            return _dbContext.RiskAssessments.Where(a => a.RiskAssessmentId == riskAssessmentId).Include(a=>a.RiskAssessmentFiles).Include(a => a.ApplicationUser).FirstOrDefault();
        }

        /// <summary>
        /// get RiskAssessmentLog details 
        /// </summary>
        /// <param name="riskAssessmentLogId"></param>
        /// <returns></returns>
        public RiskAssessmentLog GetRiskAssessmentLogDetails(Guid riskAssessmentLogId)
        {
            return _dbContext.RiskAssessmentLogs.Where(a => a.RiskAssessmentLogId == riskAssessmentLogId).Include(a => a.ApplicationUser).Include(a => a.RiskAssessment).Include(a => a.InitialRisk).Include(a => a.AdjustedRisk).FirstOrDefault();
        }

        /// <summary>
        /// get RiskAssessment list by search pagination 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<RiskAssessment> GetRiskAssessmentListBySearch(int pageNumber, int pageSize, string searchKey, string UserMasterAdminId)
        {
            var riskAssessment = _dbContext.RiskAssessments.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                riskAssessment = riskAssessment.Where(a => a.Name.ToLower().Contains(searchKey));
            }
            return riskAssessment.Include(a => a.RiskAssessmentFiles).Include(a => a.ApplicationUser).Skip((pageNumber - 1) * pageSize).Take(pageSize).OrderByDescending(a => a.CreatedDate).ToList();
        }

        /// <summary>
        /// get RiskAssessmentLog list by search pagination 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<RiskAssessmentLog> GetRiskAssessmentLogListBySearch(int pageNumber, int pageSize, string searchKey, string UserMasterAdminId, bool? Archived, bool? Expires, Guid? RiskAssessmentId)
        {
            var riskAssessmentLog = _dbContext.RiskAssessmentLogs.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                riskAssessmentLog = riskAssessmentLog.Where(a => a.Name.ToLower().Contains(searchKey));
            }
            if (RiskAssessmentId != null)
            {
                riskAssessmentLog = riskAssessmentLog.Where(a => a.RiskAssessmentId == RiskAssessmentId);
            }
            if (Archived != null)
            {
                riskAssessmentLog = riskAssessmentLog.Where(a => a.Archived == Archived);
            }
            if (Expires != null)
            {
                riskAssessmentLog = riskAssessmentLog.Where(a => a.Expires == Expires);
            }
            return riskAssessmentLog.Include(a => a.ApplicationUser).Include(a => a.RiskAssessment).Include(a => a.InitialRisk).Include(a => a.AdjustedRisk).Skip((pageNumber - 1) * pageSize).Take(pageSize).OrderByDescending(a => a.CreatedDate).ToList();
        }

        /// <summary>
        /// get RiskAssessmentLog count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetRiskAssessmentLogCountBySearch(string searchKey, string UserMasterAdminId, bool? Archived, bool? Expires, Guid? RiskAssessmentId)
        {
            var riskAssessmentLog = _dbContext.RiskAssessmentLogs.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                riskAssessmentLog = riskAssessmentLog.Where(a => a.Name.ToLower().Contains(searchKey));
            }
            if (RiskAssessmentId != null)
            {
                riskAssessmentLog = riskAssessmentLog.Where(a => a.RiskAssessmentId == RiskAssessmentId);
            }
            if (Archived != null)
            {
                riskAssessmentLog = riskAssessmentLog.Where(a => a.Archived == Archived);
            }
            if (Expires != null)
            {
                riskAssessmentLog = riskAssessmentLog.Where(a => a.Expires == Expires);
            }
            return riskAssessmentLog.Count();
        }

        /// <summary>
        /// get RiskAssessmentLog count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetRiskAssessmentCountBySearch(string searchKey, string UserMasterAdminId)
        {
            var riskAssessment = _dbContext.RiskAssessments.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                riskAssessment = riskAssessment.Where(a => a.Name.ToLower().Contains(searchKey));
            }
            return riskAssessment.Count();
        }

        /// <summary>
        ///  check the RiskAssessment existence
        /// </summary>
        /// <param name="riskAssessmentId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsRiskAssessmentExist(Guid riskAssessmentId)
        {
            return _dbContext.RiskAssessments.Find(riskAssessmentId) == null ? false : true;
        }

        /// <summary>
        ///  check the RiskAssessmentLog existence
        /// </summary>
        /// <param name="riskAssessmentLogId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsRiskAssessmentLogExist(Guid riskAssessmentLogId)
        {
            return _dbContext.RiskAssessmentLogs.Find(riskAssessmentLogId) == null ? false : true;
        }

        /// <summary>
        ///  check the InitialRisk existence
        /// </summary>
        /// <param name="initialRiskId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsInitialRiskExist(int initialRiskId)
        {
            return _dbContext.InitialRiskAndAdjustedRisks.Find(initialRiskId) == null ? false : true;
        }

        /// <summary>
        ///  check the action existence
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        //public bool IsActionExist(Guid actionId)
        //{
        //    return _dbContext.Actions.Find(actionId) == null ? false : true;
        //}

        /// <summary>
        /// Update RiskAssessment Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public RiskAssessment UpdateRiskAssessmentDetails(RiskAssessment model)
        {
            var riskAssessment = _dbContext.RiskAssessments.FirstOrDefault(a => a.RiskAssessmentId == model.RiskAssessmentId);
            riskAssessment.Name = model.Name;
            riskAssessment.Validity = model.Validity;

            _dbContext.RiskAssessments.Update(riskAssessment);
            _dbContext.SaveChanges();
            var response = _dbContext.RiskAssessments.Where(a => a.RiskAssessmentId == model.RiskAssessmentId).Include(a => a.ApplicationUser).FirstOrDefault();
            return response;
        }

        /// <summary>
        /// Update RiskAssessmentLog Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public RiskAssessmentLog UpdateRiskAssessmentLogDetails(RiskAssessmentLog model)
        {
            var riskAssessmentLog = _dbContext.RiskAssessmentLogs.FirstOrDefault(a => a.RiskAssessmentLogId == model.RiskAssessmentLogId);
            riskAssessmentLog.Name = model.Name;
            riskAssessmentLog.RiskAssessmentId = model.RiskAssessmentId;
            riskAssessmentLog.InitialRiskId = model.InitialRiskId;
            riskAssessmentLog.AdjustedRiskId = model.AdjustedRiskId;
            //riskAssessmentLog.CompletedDate = model.CompletedDate;
            riskAssessmentLog.Expires = model.Expires;
            //riskAssessmentLog.ActionId = model.ActionId;
            riskAssessmentLog.Archived = model.Archived;

            _dbContext.RiskAssessmentLogs.Update(riskAssessmentLog);
            _dbContext.SaveChanges();
            var response = _dbContext.RiskAssessmentLogs.Where(a => a.RiskAssessmentLogId == model.RiskAssessmentLogId).Include(a => a.ApplicationUser)/*.Include(a => a.Action)*/.Include(a => a.RiskAssessment).FirstOrDefault();
            return response;
        }

        /// <summary>
        /// Get  RiskAssessment Name List
        /// </summary>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public List<RiskAssessment> GetRiskAssessmentNameList(string createdBy)
        {
            return _dbContext.RiskAssessments.Where(a => a.ApplicationUser.MasterAdminId == createdBy).OrderBy(a => a.Name).ToList();
        }

        //RiskAssessment files functions.
        public RiskAssessmentFile AddRiskAssessmentFile(RiskAssessmentFile riskAssessmentFile)
        {
            _dbContext.RiskAssessmentFiles.Add(riskAssessmentFile);
            _dbContext.SaveChanges();
            return riskAssessmentFile;
        }
        public IEnumerable<RiskAssessmentFile> GetRiskAssessmentFiles(Guid riskAssessmentId)
        {
            return _dbContext.RiskAssessmentFiles.Where(a => a.RiskAssessmentId == riskAssessmentId).OrderByDescending(a => a.CreatedDate).ToList();
        }
        public RiskAssessmentFile GetRiskAssessmentFile(Guid riskAssessmentFileId)
        {
            return _dbContext.RiskAssessmentFiles.Find(riskAssessmentFileId);
        }

        public bool IsRiskAssessmentFileExist(Guid riskAssessmentFileId)
        {
            return _dbContext.RiskAssessmentFiles.Find(riskAssessmentFileId) == null ? false : true;
        }

        public RiskAssessmentFile UploadRiskAssessmentFile(RiskAssessmentFile riskAssessmentFile)
        {
            var res = _dbContext.RiskAssessmentFiles.Add(riskAssessmentFile);
            if ( res == null)
            {
                return null;
            };
            _dbContext.SaveChanges();
            return _dbContext.RiskAssessmentFiles.FirstOrDefault(a=>a.RiskAssessmentFileId == riskAssessmentFile.RiskAssessmentFileId);
        }
        public bool DeleteRiskAssessmentFile(Guid riskAssessmentFileId)
        {
            var res = _dbContext.RiskAssessmentFiles.FirstOrDefault(a => a.RiskAssessmentFileId == riskAssessmentFileId);
            var result = _dbContext.RiskAssessmentFiles.Remove(res);
            if (result == null)
            {
                return false;
            }
            _dbContext.SaveChanges();
            return true;
        }
        
        public List<Issue> GetCorrectiveIssueList(Guid riskAssessmentLogId, string UserMasterAdminId)
        {
            return _dbContext.Issues.Where(a => a.RiskAssessmentLogId == riskAssessmentLogId && a.Operator.MasterAdminId == UserMasterAdminId && a.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Corrective).Include(a=>a.IssueCategory).Include(a=>a.IssueStatus).Include(a=>a.IssueType).Include(a=>a.RiskAssessmentLog).Include(a=>a.ApplicationUser).Include(a => a.Operator).Include(a=>a.Machine).OrderByDescending(a=>a.CreatedDate).ToList();
        }


        /// <summary>
        /// Get  RiskAssessmentLog Name List
        /// </summary>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public List<RiskAssessmentLog> GetRiskAssessmentLogNameList(string createdBy)
        {
            return _dbContext.RiskAssessmentLogs.Where(a => a.ApplicationUser.MasterAdminId == createdBy).OrderBy(a => a.Name).Include(a=>a.RiskAssessment).ToList();
        }

        public IEnumerable<RiskAssessmentFile> GetRiskAssessmentFilesForMachine(Guid? riskAssessmentId)
        {
            return _dbContext.RiskAssessmentFiles.Where(a => a.RiskAssessmentId == riskAssessmentId).OrderByDescending(a => a.CreatedDate).ToList();
        }




        /// <summary>
        /// get RiskAssessmentLog details by RiskAssessment
        /// </summary>
        /// <param name="riskAssessmentLogId"></param>
        /// <returns></returns>
        public RiskAssessmentLog GetRiskAssessmentLogDetailsByRiskAssessment(Guid riskAssessmentId)
        {
            return _dbContext.RiskAssessmentLogs.Where(a => a.RiskAssessmentId == riskAssessmentId).Include(a => a.ApplicationUser).Include(a => a.RiskAssessment).Include(a => a.InitialRisk).Include(a => a.AdjustedRisk).FirstOrDefault();
        }
    }
}
