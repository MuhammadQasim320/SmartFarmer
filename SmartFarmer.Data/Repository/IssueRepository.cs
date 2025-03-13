using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
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
    public class IssueRepository : IIssueRepository
    {

        private readonly SmartFarmerContext _context;
        public IssueRepository(SmartFarmerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// add issue into system
        /// </summary>
        /// <param name="issue"></param>
        /// <returns></returns>
        public Issue AddIssue(Issue Issue)
        {
            Issue.IssueNo = 1;
            var latestIssue = _context.Issues.Where(a => a.MachineId == Issue.MachineId).OrderByDescending(a => a.CreatedDate).FirstOrDefault();
            if (latestIssue != null)
            {
                Issue.IssueNo = latestIssue.IssueNo+1;
            }
            if(Issue.IsTargetDateExist == false)
            {
                Issue.TargetDate = null;
            }

            //if (Issue.IssueStatusId == (int)Core.Common.Enums.IssueStatusEnum.Open && Issue.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Defect)
            //{
            //    if (Issue.MachineId != null)
            //    {
            //        Issue.Machine.ResultId = (int)Core.Common.Enums.CheckResultEnum.UnSafe;
            //    }

            //}

            var machine = _context.Machines.FirstOrDefault(m => m.MachineId == Issue.MachineId);

            if (Issue.IssueStatusId == (int)Core.Common.Enums.IssueStatusEnum.Open &&
                Issue.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Defect)
            {
                if (machine != null) 
                {
                    machine.ResultId = (int)Core.Common.Enums.CheckResultEnum.UnSafe;
                }
            }
            _context.Issues.Add(Issue);
            _context.SaveChanges();
            var response = _context.Issues.Where(a => a.IssueId == Issue.IssueId).Include(a => a.RiskAssessmentLog).Include(a => a.IssueType).Include(a=>a.IssueCategory).Include(a => a.ApplicationUser).Include(a => a.IssueStatus).Include(a => a.Machine).Include(a=>a.Operator).FirstOrDefault();
            return Issue;
        }
        
        /// <summary>
        /// add issue into system
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        public bool IsIssueExist(Guid IssueId)
        {
            return _context.Issues.Find(IssueId) == null ? false : true;
        } 
        
        /// <summary>
        /// add issueFile into system
        /// </summary>
        /// <param name="issueFileId"></param>
        /// <returns></returns>
        public bool IsIssueFileExist(Guid IssueFileId)
        {
            return _context.IssueFiles.Find(IssueFileId) == null ? false : true;
        }

        /// <summary>
        /// get issue details
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        public Issue GetIssueDetails(Guid issueId)
        {
            return _context.Issues.Where(a => a.IssueId == issueId).Include(a => a.RiskAssessmentLog).Include(a => a.IssueType).Include(a => a.IssueCategory).Include(a => a.ApplicationUser).Include(a => a.IssueStatus).Include(a => a.Operator).Include(a => a.Machine).FirstOrDefault();
        }

        /// <summary>
        ///update issue details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Issue UpdateIssueDetails(Issue issue)
        {
            var res = _context.Issues.Find(issue.IssueId);
            if (res != null)
            {
                res.ResolvedBy = issue.ResolvedBy;
                res.IssueTitle = issue.IssueTitle;
                res.Description = issue.Description;
                res.IssueCategoryId = issue.IssueCategoryId;
                res.IssueTypeId = issue.IssueTypeId;
                res.MachineId = issue.MachineId;
                res.ResolvedDate = issue?.ResolvedDate;
                res.IssueStatusId = issue.IssueStatusId;
                res.IsTargetDateExist = issue.IsTargetDateExist;
                res.TargetDate = issue.TargetDate;
                res.Note= issue.Note;
                if (res.IssueStatusId == (int)Core.Common.Enums.IssueStatusEnum.Open && res.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Defect)
                {
                    if (res.MachineId != null)
                    {
                        res.Machine.ResultId = (int)Core.Common.Enums.CheckResultEnum.UnSafe;
                    }
                }
                _context.Issues.Update(res);
                _context.SaveChanges();
                var result = _context.Issues.Where(a => a.IssueId == issue.IssueId).Include(a => a.RiskAssessmentLog).Include(a => a.Operator).Include(a => a.IssueType).Include(a => a.IssueCategory).Include(a => a.ApplicationUser).Include(a => a.IssueStatus).Include(a => a.Machine).FirstOrDefault();
                return result;
            }
            return null;
        }

        /// <summary>
        ///get machine issues list
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public List<Issue> GetMachineIssuesList(Guid machineId, string masterAdminId)
        {
            return _context.Issues.Where(a=>a.MachineId == machineId && a.Operator.MasterAdminId == masterAdminId).Include(a => a.RiskAssessmentLog).Include(a => a.Operator).Include(a => a.IssueType).Include(a => a.IssueCategory).Include(a => a.ApplicationUser).Include(a => a.IssueStatus).Include(a => a.Machine).OrderByDescending(a=>a.CreatedDate).ToList();
        }
        /// <summary>
        ///get machine issues list
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public List<Issue> GetMachineIssues(Guid machineId, string UserMasterAdminId)
        {
            return _context.Issues.Where(a=>a.MachineId == machineId && a.Operator.MasterAdminId== UserMasterAdminId && a.IssueStatusId== (int)Core.Common.Enums.IssueStatusEnum.Open).Include(a=>a.IssueCategory).Include(a=>a.IssueType).Include(a=>a.IssueStatus).Include(a=>a.ApplicationUser).Include(a=>a.Operator).ToList();
        }

        /// <summary>
        ///upload issue file
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        public IssueFile UploadIssueFile(IssueFile issueFile)
        {
            var issue = _context.IssueFiles.Add(issueFile);
            if(issue == null)
            {
                return null;
            }
            _context.SaveChanges();
            return issueFile;
        }
        
        /// <summary>
        ///get issue files
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        public List<IssueFile> GetIssueFilesList(Guid issueId)
        {
            return _context.IssueFiles.Where(a=>a.IssueId == issueId).ToList();
        }  
        
        ///// <summary>
        /////get issue files
        ///// </summary>
        ///// <param name="issueId"></param>
        ///// <returns></returns>
        //public  List<IssueComment> GetIssueCommentList(Guid issueId)
        //{
        //    return _context.IssueComments.Where(a=>a.IssueId == issueId).Include(a=>a.ApplicationUser).Include(a=>a.Issue).ToList();
        //}

        /// <summary>
        /// get issue list by search pagination 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<Issue> GetIssueListBySearch(int pageNumber, int pageSize, string searchKey, int? issueStatusId, Guid? issueCategoryId, int? issueTypeId, Guid? machineId, string masterAdminId)
        {

            var issues = _context.Issues.Where(a=>a.Operator.MasterAdminId == masterAdminId).Include(a => a.ApplicationUser).Include(a => a.RiskAssessmentLog).Include(a=>a.Machine).Include(a => a.IssueType).Include(a => a.IssueCategory).Include(a => a.IssueStatus).Include(a => a.Operator).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                issues = issues.Where(a => a.IssueTitle.ToLower().Contains(searchKey)|| a.Machine.Name.ToLower().Contains(searchKey));
              
            }
            if (issueCategoryId != null)
            {
                issues = issues.Where(a => a.IssueCategoryId == issueCategoryId);
            }
            if (issueTypeId != null)
            {
                issues = issues.Where(a => a.IssueTypeId == issueTypeId && a.IssueStatusId== (int)Core.Common.Enums.IssueStatusEnum.Open);
            }
            if (issueStatusId != null)
            {
                issues = issues.Where(a => a.IssueStatusId == issueStatusId);
            }
            if (machineId != null)
            {
                issues = issues.Where(a => a.MachineId == machineId);
            }
            return issues.Skip((pageNumber - 1) * pageSize).Take(pageSize).OrderByDescending(a => a.CreatedDate).ToList();
        }
        
        /// <summary>
        /// get issue count by search pagination 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetIssueCountBySearch(string searchKey, int? issueStatusId, Guid? issueCategoryId, int? issueTypeId, Guid? machineId, string masterAdminId)
        {
            var issues = _context.Issues.Where(a=>a.Operator.MasterAdminId == masterAdminId).Include(a => a.ApplicationUser).Include(a=>a.Machine).Include(a => a.IssueType).Include(a => a.IssueCategory).Include(a => a.IssueStatus).Include(a => a.Operator).AsQueryable();
            if (searchKey != null)
            {
                {
                    searchKey = searchKey.ToLower();
                    issues = issues.Where(a => a.IssueTitle.ToLower().Contains(searchKey) || a.Machine.Name.ToLower().Contains(searchKey));

                }
            }
            if (issueCategoryId != null)
            {
                issues = issues.Where(a => a.IssueCategoryId == issueCategoryId);
            }
            if (issueTypeId != null)
            {
                issues = issues.Where(a => a.IssueTypeId == issueTypeId && a.IssueStatusId == (int)Core.Common.Enums.IssueStatusEnum.Open);
            }
            if (issueStatusId != null)
            {
                issues = issues.Where(a => a.IssueStatusId == issueStatusId);
            }
            if (machineId != null)
            {
                issues = issues.Where(a => a.MachineId == machineId);
            }
            return issues.Count();
        } 
        
        /// <summary>
        /// get issue count by search pagination 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<Issue> GetMachineExistingIssuesList(Guid machineId)
        {
            return _context.Issues.Where(a => a.MachineId == machineId).ToList();
        } 
        /// <summary>
        /// get issue count by search pagination 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<Issue> GetActions(Guid RiskAssessmentLogId, string UserMasterAdminId)
        {
            return _context.Issues.Where(a => a.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Corrective && a.RiskAssessmentLogId == RiskAssessmentLogId && a.Operator.MasterAdminId == UserMasterAdminId).Include(a=>a.IssueStatus).Include(a => a.IssueType).ToList();
        }

        ///// <summary>
        ///// add IssueComment comment into system
        ///// </summary>
        ///// <param name="IssueComment"></param>
        ///// <returns></returns>
        //public IssueComment AddIssueComment(IssueComment IssueComment)
        //{
        //    _context.IssueComments.Add(IssueComment);
        //    _context.SaveChanges();
        //    var response = _context.IssueComments.Where(a => a.IssueCommentId == IssueComment.IssueCommentId).Include(a => a.Issue).Include(a => a.ApplicationUser).FirstOrDefault();
        //    return IssueComment;
        //}

        /// <summary>
        /// add IssueComment comment into system
        /// </summary>
        /// <param name="IssueComment"></param>
        /// <returns></returns>
        public bool AddIssueComment(Guid IssueId, string Comment)
        {
            var issueComment = _context.Issues.FirstOrDefault(a => a.IssueId == IssueId);
            issueComment.Note = Comment;
            var res = _context.Update(issueComment);
            if (res == null)
            {
                return false;
            }
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// get issueFile into system
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public IssueFile GetIssueFile(Guid issueFileId)
        {
            return _context.IssueFiles.Find(issueFileId);
        }

        /// <summary>
        /// delete issueFile into system
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool DeleteIssueFile(Guid issueFileId)
        {
            var res = _context.IssueFiles.FirstOrDefault(a => a.IssueFileId == issueFileId);
            var result = _context.IssueFiles.Remove(res);
            if (result == null)
            {
                return false;
            }
            _context.SaveChanges();
            return true;
        }


        /// <summary>
        /// get default issue into system
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool IsIssuesDefected(IEnumerable<Issue> issues)
        {
            return issues.Any(issue => issue.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Defect);
        }
    }
}
